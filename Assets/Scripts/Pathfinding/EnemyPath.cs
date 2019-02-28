using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour {
    public bool DebugClickMode;
    public bool hasSeperateFacingAnimations = false;
    public BoxCollider2D bodyCollider;
    public BreadCrumb path;
    public GameObject DebugPathEdge;
    private LineRenderer line;

    // Use this for initialization
    void Awake () {
        var go = GameObject.Instantiate(DebugPathEdge);
        line = go.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.startWidth = 0.25f;
        line.endWidth = .25f;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.transform.parent = transform;
    }	

    // Pass this a distance (speed * Time.deltaTime) each frame to smoothly move through the path nodes.
    // Returns false if the path is null.
    public bool MoveAlongPath(float distance)
    {
        if (path != null) {
            // set a new destination (the point on the grid, offset by where the bodyCollider actually is)
            Vector2 destination = RoomManager.Instance.currentRoom.pathGrid.GridToWorld(path.Position) - bodyCollider.offset * Mathf.Sign(transform.localScale.x);
            transform.position = Vector3.MoveTowards(transform.position, destination, distance);

            if (Vector2.Distance(transform.position, destination) < 0.001f) {
                transform.position = destination;
                path = path.Next;

                // turn toward the next node.
                if (path != null)
                    FaceNextPathNode();
            }
            return true;
        }

        return false;
    }


    public void GeneratePath(PathGrid pathGrid, Point startPoint, Point destPoint)
    {
        if (pathGrid != null && startPoint != null && destPoint != null) {
            // Get the best path to the random point, if it exists....
            path = Pathfinder.FindPath(pathGrid, startPoint, destPoint);

            DrawDebugPath(pathGrid);
        }
    }

    // Generates a straight line to a point on the grid, ignoring any pathing logic.
    public void GenerateQuickPath(PathGrid pathGrid, Point destPoint)
    {
        if (pathGrid != null && destPoint != null) {
            path = Pathfinder.FindQuickPath(destPoint);

            DrawDebugPath(pathGrid);
        }
    }

    // Makes sure this transform faces the next path node.
    public void FaceNextPathNode()
    {
        if (!hasSeperateFacingAnimations)
            transform.localScale = new Vector3(Mathf.Sign(RoomManager.Instance.currentRoom.pathGrid.GridToWorld(path.Position).x - transform.position.x),
                                      transform.localScale.y,
                                      transform.localScale.z);
    }

    // Crappy estimate of how long in seconds it will take to traverse the path.
    public float CalculateMoveEstimate(PathGrid pathGrid, float speed)
    {
        var curr = path;
        int nodeCount = 0;

        // estimate how long it should take the enemy to finish their path.  That way if they get stuck on something for too long they can stop moving if they need to.
        while (curr != null) {
            nodeCount++;
            curr = curr.Next;
        }

        return nodeCount * pathGrid.width / speed + 1f;
    }

    public void DrawDebugPath(PathGrid pathGrid)
    {
#if DEBUG_PATHFINDING
        BreadCrumb curr = path;
        int index = 0;

        // Render the path for debugging
        while (curr != null) {
            line.positionCount = index + 1;
            line.SetPosition(index, pathGrid.GridToWorld(curr.Position));
            curr = curr.Next;
            index++;
        }
#endif
    }

    public void ClearPath()
    {
        path = null;

#if DEBUG_PATHFINDING
        line.positionCount = 0;
#endif
    }

    // returns true if the click was processed and attempted to generate a path.
    public bool ProcessDebugClickMode()
    {
        if (DebugClickMode) {
            // left click to path to a point on the grid.
            if (Input.GetMouseButton(0)) {
                //Convert mouse click point to grid coordinates
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                PathGrid pathGrid = RoomManager.Instance.currentRoom.pathGrid;
                if (pathGrid != null) {
                    Point destPos = pathGrid.WorldToGrid(worldPos);
                    if (destPos != null) {
                        Point startPos = pathGrid.WorldToGrid((Vector2)transform.position + bodyCollider.offset);
                        GeneratePath(pathGrid, startPos, destPos);

                        return true;
                    }
                }
            }
        }
        return false;
    }

}
