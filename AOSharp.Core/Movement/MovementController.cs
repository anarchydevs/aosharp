using AOSharp.Common.GameData;
using System;
using System.Collections.Generic;

namespace AOSharp.Core.Movement
{
    public class MovementController
    {
        private const float UPDATE_INTERVAL = 0.2f;
        private const float UNSTUCK_INTERVAL = 5f;
        private const float UNSTUCK_THRESHOLD = 2f;

        public bool IsNavigating => !(_path.Count == 0);

        private float _timeSinceLastUpdate = 0f;
        private float _timeSinceLastUnstuckCheck = 0f;
        private float _stopDist = 1f;
        private float _lastDist = 0f;
        private bool _drawPath;
        private Queue<Vector3> _path = new Queue<Vector3>();

        public EventHandler<DestinationReachedEventArgs> DestinationReached;

        public MovementController(bool drawPath = false)
        {
            Game.OnUpdate += Update;
            _drawPath = drawPath;
        }

        protected virtual void Update(object s, float deltaTime)
        {
            if (_path.Count == 0)
                return;

            if (!DynelManager.LocalPlayer.IsMoving)
                Game.SetMovement(MovementAction.ForwardStart);

            if (DynelManager.LocalPlayer.IsMoving && DynelManager.LocalPlayer.Position.DistanceFrom(_path.Peek()) <= _stopDist)
            {
                _path.Dequeue();

                if (_path.Count == 0)
                {
                    DestinationReachedEventArgs e = new DestinationReachedEventArgs();
                    DestinationReached?.Invoke(this, e);

                    if(e.Halt)
                        Game.SetMovement(MovementAction.ForwardStop);
                }
            }

            if (_timeSinceLastUnstuckCheck > UNSTUCK_INTERVAL)
            {
                float currentDist = DynelManager.LocalPlayer.Position.DistanceFrom(_path.Peek());

                if (_lastDist - currentDist <= UNSTUCK_THRESHOLD)
                {
                    OnStuck();
                }

                _lastDist = currentDist;
                _timeSinceLastUnstuckCheck = 0;
            }

            LookAt(_path.Peek());

            if (_timeSinceLastUpdate > UPDATE_INTERVAL)
            {
                Game.SetMovement(MovementAction.Update);
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
                Game.SetMovement(MovementAction.ForwardStop);
        }

        public virtual void MoveTo(Vector3 pos)
        {
            RunPath(new List<Vector3> { pos });
        }

        public virtual void RunPath(List<Vector3> path)
        {
            _path.Clear();

            foreach (Vector3 wp in path)
            {
                if(DynelManager.LocalPlayer.Position.DistanceFrom(wp) <= _stopDist)
                _path.Enqueue(wp);
            }
        }

        public void LookAt(Vector3 pos)
        {
            DynelManager.LocalPlayer.Rotation = Quaternion.FromTo(DynelManager.LocalPlayer.Position, pos);
        }

        protected virtual void OnStuck()
        {
            //Chat.WriteLine("Stuck!?");
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
