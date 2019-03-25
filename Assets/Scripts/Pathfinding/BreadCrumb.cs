using System;
using UnityEngine;

// trailer of path node positions, traversed to get a full path
public class BreadCrumb : IComparable<BreadCrumb> {
    private Point _position;
    private BreadCrumb _next;

    public int cost = Int32.MaxValue;
    public int g = 0;
    public bool IsClosed = false;
    public bool IsOpen = false;

    public Point Position {
        get { return _position; }
    }

    public BreadCrumb Next {
        get { return _next; }
        set { _next = value; }
    }

    public BreadCrumb(Point pos)
    {
        _position = pos;
    }

    // comparer overrides to check on position x and y instead of memory hash codes
    public override bool Equals(object obj)
    {
        return (obj is BreadCrumb) && ((BreadCrumb)obj).Position.X == this.Position.X && ((BreadCrumb)obj).Position.Y == this.Position.Y;
    }

    public bool Equals(BreadCrumb breadcrumb)
    {
        return breadcrumb.Position.X == this.Position.X && breadcrumb.Position.Y == this.Position.Y;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public int CompareTo(BreadCrumb other)
    {
        return cost.CompareTo(other.cost);
    }
}
