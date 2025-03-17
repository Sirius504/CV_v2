using System;
using UnityEngine;

namespace ActionBehaviour
{
    public enum Action
    {
        Default,
        Movement,
        Attack,
        Block,
    }

    public enum ActionDirection
    {
        Horizontal,
        Vertical
    }

    public struct ActionInfo
    {
        private readonly Action _action;
        private readonly Vector2Int _from;
        private readonly Vector2Int _to;

        public Action Action => _action;
        public Vector2Int From => _from;
        public Vector2Int To => _to;
        public ActionDirection Direction { get; private set; }

        public ActionInfo(Vector2Int to, Vector2Int from, Action action)
        {
            _from = from;
            _to = to;
            _action = action;
            Direction = GetDirection(to, from);
        }

        private static ActionDirection GetDirection(Vector2Int from, Vector2Int to)
        {
            var direction = to - from;
            if (direction.x == 0)
            {
                return ActionDirection.Vertical;
            }
            if (direction.y == 0)
            {
                return ActionDirection.Horizontal;
            }
            throw new NotImplementedException();
        }
    }
}