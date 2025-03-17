using ActionBehaviour;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string name) : base(name)
    {
    }

    public override bool Process()
    {
        while (currentChild < Children.Count)
        {
            if (!Children[currentChild].Process())
            {                
                return false;
            }

            currentChild++;
        }

        currentChild = 0;
        return true;
    }
}

public class Selector : Node
{
    public Selector(string name) : base(name)
    {
    }

    public override bool Process()
    {
        while (currentChild < Children.Count)
        {
            if (Children[currentChild].Process())
            {
                return true;
            }
            currentChild++;
        }

        currentChild = 0;
        return false;
    }
}

public abstract class Node { 

    public readonly string Name;

    public readonly List<Node> Children = new();
    protected int currentChild;
    protected Dictionary<string, object> State;

    public Node(string name)
    {
        Name = name;
    }

    public void AddChild(Node child) => Children.Add(child);

    public virtual bool Process()
    {
        var result = Children[currentChild].Process();
        currentChild = 0;
        return result;
    }

    public virtual void Reset()
    {
        currentChild = 0;
        foreach(var child in Children)
        {
            child.Reset();
        }
    }
}
