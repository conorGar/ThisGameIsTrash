using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEdge {
    public PathNode _parent;
    public PathNode _node;
    public bool _valid;

    private LineRenderer DebugLineRenderer;

    public PathNode Parent {
        get { return _parent; }
    }

    public PathNode Node {
        get { return _node; }
    }

    public bool Valid {
        get { return _valid; }
        set { _valid = value; }
    }

    public PathEdge(PathNode parent, PathNode node, bool valid, PathGrid grid)
    {
        _valid = valid;
        _node = node;
        _parent = parent;

        if (_node != null && !_node.traversable)
            _valid = false;

        if (_parent != null && !_parent.traversable)
            _valid = false;

#if DEBUG_PATHFINDING
        var go = GameObject.Instantiate(grid.debugEdgePrefab) as GameObject;
        DebugLineRenderer = go.GetComponent<LineRenderer>();
        DebugLineRenderer.startWidth = 0.01f;
        DebugLineRenderer.endWidth = .05f;
        DebugLineRenderer.startColor = Color.green;
        DebugLineRenderer.endColor = Color.green;
        DebugLineRenderer.gameObject.SetActive(false);
        DebugLineRenderer.transform.parent = grid.transform;
#endif
    }

    public void Draw()
    {
#if DEBUG_PATHFINDING
        if (_valid) {
            if (_parent != null && _node != null) {
                DebugLineRenderer.gameObject.SetActive(true);
                DebugLineRenderer.SetPosition(0, _parent.position);
                DebugLineRenderer.SetPosition(1, _node.position);
            }
        }
#endif
    }
}
