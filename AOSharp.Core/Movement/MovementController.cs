using AOSharp.Common.GameData;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using AOSharp.Common.Unmanaged.Imports;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Core.Movement
{
    public class MovementController
    {
        private const float UpdateInterval = 0.2f;
        private const float UnstuckInterval = 5f;
        private const float UnstuckThreshold = 2f;

        public bool IsNavigating => _path.Count != 0;

        private float _timeSinceLastUpdate = 0f;
        private float _timeSinceLastUnstuckCheck = 0f;
        private float _stopDist = 1f;
        private float _lastDist = 0f;
        private float _nodeReachedDist = 1f;
        private bool _drawPath;
        private Queue<Vector3> _path = new Queue<Vector3>();

        private static ConcurrentQueue<MovementAction> _movementActionQueue = new ConcurrentQueue<MovementAction>();

        public EventHandler<DestinationReachedEventArgs> DestinationReached;

        public MovementController(bool drawPath = false)
        {
            Game.OnUpdate += Update;
            _drawPath = drawPath;
        }

        internal static void UpdateInternal()
        {
            while (_movementActionQueue.TryDequeue(out MovementAction action))
                ChangeMovement(action);
        }

        protected virtual void Update(object s, float deltaTime)
        {
            if (_path.Count == 0)
                return;

            if (!DynelManager.LocalPlayer.IsMoving)
                SetMovement(MovementAction.ForwardStart);

            if (DynelManager.LocalPlayer.IsMoving && DynelManager.LocalPlayer.Position.DistanceFrom(_path.Peek()) <= (_path.Count > 1 ? _nodeReachedDist : _stopDist))
            {
                _path.Dequeue();

                if (_path.Count == 0)
                    OnDestinationReached();
            }

            if (_timeSinceLastUnstuckCheck > UnstuckInterval)
            {
                float currentDist = DynelManager.LocalPlayer.Position.DistanceFrom(_path.Peek());

                if (_lastDist - currentDist <= UnstuckThreshold)
                {
                    OnStuck();
                }

                _lastDist = currentDist;
                _timeSinceLastUnstuckCheck = 0;
            }

            LookAt(_path.Peek());

            if (_timeSinceLastUpdate > UpdateInterval)
            {
                SetMovement(MovementAction.Update);
                _timeSinceLastUpdate = 0f;
            }

            if (_drawPath)
            {
                Vector3 lastWaypoint = DynelManager.LocalPlayer.Position;
                foreach (Vector3 waypoint in _path)
                {
                    Debug.DrawLine(lastWaypoint, waypoint, DebuggingColor.Yellow);
                    lastWaypoint = waypoint;
                }
            }

            _timeSinceLastUpdate += deltaTime;
            _timeSinceLastUnstuckCheck += deltaTime;
        }

        public void Halt()
        {
            _path.Clear();

            if(DynelManager.LocalPlayer.IsMoving)
                SetMovement(MovementAction.ForwardStop);
        }

        public virtual void MoveTo(Vector3 pos, float stopDistance = 1f)
        {
            RunPath(new List<Vector3> { pos }, stopDistance);
        }

        public virtual void RunPath(List<Vector3> path, float stopDistance = 1f)
        {
            _stopDist = stopDistance;
            _path.Clear();

            foreach (Vector3 wp in path)
            {
                if (DynelManager.LocalPlayer.Position.DistanceFrom(wp) > _nodeReachedDist)
                    _path.Enqueue(wp);
            }
        }

        public void LookAt(Vector3 pos)
        {
            DynelManager.LocalPlayer.Rotation = Quaternion.FromTo(DynelManager.LocalPlayer.Position, pos);
        }

        protected virtual void OnDestinationReached()
        {
            DestinationReachedEventArgs e = new DestinationReachedEventArgs();
            DestinationReached?.Invoke(this, e);

            if (e.Halt)
                SetMovement(MovementAction.ForwardStop);
        }

        protected virtual void OnStuck()
        {
            //Chat.WriteLine("Stuck!?");
        }

        //Must be called from game loop!
        private static void ChangeMovement(MovementAction action)
        {
            if (action == MovementAction.LeaveSit)
            {
                Network.Send(new CharacterActionMessage()
                {
                    Action = CharacterActionType.StandUp
                });
            }
            else
            {
                IntPtr pEngine = N3Engine_t.GetInstance();

                if (pEngine == IntPtr.Zero)
                    return;

                N3EngineClientAnarchy_t.MovementChanged(pEngine, action, 0, 0, true);
            }       
        }

        public static void SetMovement(MovementAction action)
        {
            _movementActionQueue.Enqueue(action);
        }
    }

    public class DestinationReachedEventArgs : EventArgs
    {
        public bool Halt { get; set; }

        public DestinationReachedEventArgs(bool halt = true)
        {
            Halt = halt;
        }
    }
}
