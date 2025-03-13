using ActionBehaviour;
using System.Collections.Generic;

public class Sequence : Node
{
    public Sequence(string name) : base(name)
    {
    }

    public override bool Process(out ActionInfo actionInfo)
    {
        actionInfo = default;
        while (currentChild < Children.Count)
        {
            if (!Children[currentChild].Process(out actionInfo))
            {                
                return false;
            }

            currentChild++;
        }

        return true;
    }
}

public class Selector : Node
{
    public Selector(string name) : base(name)
    {
    }

    public override bool Process(out ActionInfo actionInfo)
    {
        actionInfo = default;
        while (currentChild < Children.Count)
        {
            if (Children[currentChild].Process(out actionInfo))
            {
                return true;
            }
            currentChild++;
        }

        return false;
    }
}

public abstract class Node { 

    public readonly string Name;

    public readonly List<Node> Children = new();
    protected int currentChild;

    public Node(string name)
    {
        Name = name;
    }

    public void AddChild(Node child) => Children.Add(child);

    public virtual bool Process(out ActionInfo actionInfo) => Children[currentChild].Process(out actionInfo);

    public virtual void Reset()
    {
        currentChild = 0;
        foreach(var child in Children)
        {
            child.Reset();
        }
    }
}
