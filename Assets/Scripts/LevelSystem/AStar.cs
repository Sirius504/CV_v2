/*
Unity C# Port of Andrea Giammarchi's JavaScript A* algorithm (http://devpro.it/javascript_id_137.html)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar
{
    private class Node
    {
        public Vector2Int position;
        public Node parent;
        public double g;
        public double f;

        public Node(int x, int y)
        {
            this.position = new Vector2Int(x, y);
        }

        public Node(Vector2Int position)
        {
            this.position = position;
        }

        public bool IsAdjacent(Node another)
        {
            var absX = Mathf.Abs(position.x - another.position.x);
            var absY = Mathf.Abs(position.y - another.position.y);
            return absX == 1 && absY == 0
                || absX == 0 && absY == 1;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
        
        public override bool Equals(object other)
        {
            if (other is not Node) return false;

            return Equals((Node)other);
        }

        public bool Equals(Node other)
        {
            return this.position == other.position;
        }
    }

    private readonly Level _level;
    private readonly int cols;
    private readonly int rows;
    private readonly HashSet<Node> checkedNodes;
    private readonly HashSet<Node> uncheckedNodes;

    public Astar(Level level)
    {
        _level = level;
        cols = _level.Size.x;
        rows = _level.Size.y;

        checkedNodes = new HashSet<Node>(cols * rows);
        uncheckedNodes = new HashSet<Node>(cols * rows);
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Func<ICellInfo, bool> predicate)
    {
        CheckArguments(start, end);

        uncheckedNodes.Add(new Node(start.x, start.y)
        {
            f = 0,
            g = 0
        });

        var endNode = new Node(end.x, end.y);
        Node current = null;

        do
        {
            current = uncheckedNodes.Aggregate((minWeightNode, nextNode) => nextNode.f < minWeightNode.f ? nextNode : minWeightNode);
            var successorsToCheck = Successors(current, predicate).Where(node => !checkedNodes.Contains(node));

            foreach (var next in successorsToCheck)
            {
                next.parent = current;

                next.g = current.g + ManhattanDistance(next.position, current.position);
                next.f = next.g + ManhattanDistance(next.position, endNode.position);
                uncheckedNodes.Add(next);
            }
            checkedNodes.Add(current);
            uncheckedNodes.Remove(current);
        }
        while (uncheckedNodes.Count > 0 && !current.IsAdjacent(endNode));

        uncheckedNodes.Clear();
        checkedNodes.Clear();

        return current.IsAdjacent(endNode)
            ? CollectResult(end, current)
            : null;
    }

    private void CheckArguments(Vector2Int start, Vector2Int end)
    {
        if (!_level.InBounds(start))
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        if (!_level.InBounds(end))
        {
            throw new ArgumentOutOfRangeException(nameof(end));
        }
    }

    private static List<Vector2Int> CollectResult(Vector2Int end, Node current)
    {
        var result = new List<Vector2Int>();
        do
        {
            result.Add(current.position);
            current = current.parent;
        }
        while (current != null);

        result.Reverse();
        result.Add(end);

        return result;
    }

    private Node[] Successors(Node node, Func<ICellInfo, bool> obstaclePredicate)
    {
        var position = node.position;
        var adjacent = new Vector2Int[]
        {
            position + Vector2Int.up,
            position + Vector2Int.down,
            position + Vector2Int.right,
            position + Vector2Int.left,
        };

        var clarified = adjacent
            .Where(node => _level.InBounds(node)
            && obstaclePredicate.Invoke(_level.GetCell(node)))
            .ToList();

        return clarified
            .Select(ci => new Node(ci.x, ci.y))
            .ToArray();
    }

    public double ManhattanDistance(Vector2Int start, Vector2Int end)
    {
        return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
    }
}