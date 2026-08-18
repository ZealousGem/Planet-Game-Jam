using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class PriortySelector : Selector
{
    List<Node> sortedChildren;
    List<Node> SortedChildren => sortedChildren ??= SortChildren();

    protected virtual List<Node> SortChildren() => children.OrderByDescending(child => child.priorty).ToList(); 

    public PriortySelector(string name, int num = 0) : base(name, num){}

    public override void Reset()
    {
        base.Reset();
        sortedChildren = null;
    }

    public override Status Process()
    {
        foreach( var child in SortedChildren)
        {
            switch (child.Process())
            {
                case Status.Running: ResetOtherChildren(child); return Status.Running; 
                case Status.Success: Reset(); return Status.Success;
                default: continue;
            }     
        }
        
        return Status.Failure;
    }

    private void ResetOtherChildren(Node activeChild)
    {
        foreach (var child in children)
        {
            if (child != activeChild)
            {
                child.Reset();
            }
        }
    }
    
}
public class Selector : Node
{
    public Selector(string name, int priorty) : base(name, priorty)
    { }

    public override Status Process()
    {
        if(currentChild < children.Count)
        {
            switch (children[currentChild].Process())
            {
                case Status.Running: return Status.Running; 
                case Status.Success: Reset(); return Status.Success;
                default: currentChild++; return Status.Running;
            }     
        }
        Reset();
        return Status.Failure;
    }
}

public class Inverter : Node
{
    public Inverter(string name) : base(name)
    {}
    public override Status Process()
    {
            switch (children[0].Process())
            {
                case Status.Running: return Status.Running; 
                case Status.Failure: Reset(); return Status.Success;
                default: return Status.Failure;
            }    
    }

    
}

public class UntilFail : Node
{
    public UntilFail(string name, int num = 0) : base(name, num)
    {}
    public override Status Process()
    {
        
            if (children[0].Process() == Status.Failure)
            {
               Reset();
               return Status.Failure;
            }

            return Status.Running;    

    }

    
}
public class Leaf : Node // opnly behaviour no children
{
    readonly IStrategy strategy; 

    public Leaf(string name, IStrategy strategy, int num = 0) : base(name, num)
    {
        this.strategy = strategy;
    }

    public override Status Process() => strategy.Process();

    public override void Reset() => strategy.Reset();    
    
}

public class Sequence : Node
{

    public Sequence(string name, int priorty = 0) : base(name, priorty){}
    public override Status Process()
    {
        if(currentChild < children.Count)
        {
            switch (children[currentChild].Process())
            {
                case Status.Running: return Status.Running; 
                case Status.Failure: Reset(); return Status.Failure;
                default: currentChild++; return currentChild == children.Count ? Status.Success : Status.Running;
            }    
        }

        Reset();
        return Status.Success;
    }
}

public class RandomSelector : PriortySelector
{
    protected override List<Node> SortChildren()=> children.Shuffle().ToList();

    public RandomSelector(string name) : base(name){}
}

public class BehaviourTree: Node
{
    public BehaviourTree(string name) : base(name){}

    public override Status Process()
    {
        while (currentChild < children.Count)
        {
            var status = children[currentChild].Process();
            if (status != Status.Success)
            {
                return status;
            }
            currentChild++;
        }

        return Status.Success;
    }
}
public class Node 
{
    public enum Status{Success, Failure, Running}

    public readonly string name;
    public readonly int priorty;

    public readonly List<Node> children = new();

    protected int currentChild;

    public Node(string name = "Node", int peioty = 0)
    {
       this.name = name;    
       priorty = peioty;    
    }

    public void AddChild(Node child) => children.Add(child);

    public virtual Status Process() => children[currentChild].Process();

    public virtual void Reset()
    {
        currentChild = 0;

        foreach (var child in children)
        {
            child.Reset();
        }
    }
}

public static class ListExtentions
{
    private static System.Random rng;
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if(rng == null) rng = new System.Random();

        int count = list.Count;

        while (count > 1)
        {
            --count;
            int index = rng.Next(count + 1);
            (list[index], list[count]) = (list[count], list[index]);
            
        }
        return list;
    }
}