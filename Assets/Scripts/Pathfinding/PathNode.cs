using System.Collections;
using UnityEngine;

public class PathNode {
    public bool traversable = true;

    public int x;
    public int y;

    public Vector2 position;

    public PathEdge top;
    public PathEdge left;
    public PathEdge bottom;
    public PathEdge right;
    public PathEdge topleft;
    public PathEdge topright;
    public PathEdge bottomleft;
    public PathEdge bottomright;

    GameObject DebugNode;

    private const string COLLISION_TAG = "tiles";
    private int TILE_LAYER;
    private static Vector2 V_UP = Vector2.up;
    private static Vector2 V_LEFT = Vector2.left;
    private static Vector2 V_RIGHT = Vector2.right;
    private static Vector2 V_DOWN = Vector2.down;
    private static Vector2 V_UP_LEFT = new Vector2(-1, 1);
    private static Vector2 V_DOWN_LEFT = new Vector2(-1, -1);
    private static Vector2 V_UP_RIGHT = new Vector2(1, 1);
    private static Vector2 V_DOWN_RIGHT = new Vector2(1, -1);

    public PathNode(int _x, int _y, Vector2 pos, PathGrid grid)
    {
        TILE_LAYER = LayerMask.NameToLayer("tiles");
        x = _x;
        y = _y;

        position = pos;

        // NOTE: Grid pivot needs to be set to top left.  position will be (0,0)
        // Mark nodes untraversable if they are outside the transform.
        if (position.x < grid.offset.x || position.y > grid.offset.y) {
            DisableEdges();
            traversable = false;
        }
        else if (position.x > grid.offset.x + grid.transform.localScale.x) {
            DisableEdges();
            traversable = false;
        }
        else if (position.y < grid.offset.y - grid.transform.localScale.y) {
            DisableEdges();
            traversable = false;
        }

#if DEBUG_PATHFINDING
        //Draw Node on screen for debugging purposes
        DebugNode = GameObject.Instantiate(grid.debugNodePrefab) as GameObject;
        DebugNode.transform.position = position;
        DebugNode.transform.parent = grid.transform;
#endif
    }

    // Raycast in all 8 directions to find open spots.
    public void InitEdges(PathGrid grid)
    {
        RaycastHit2D hit = new RaycastHit2D();

        if (x > 0) {
            // left
            left = new PathEdge(this, grid.nodes[x - 1, y], IsDirectionValid(hit, V_LEFT, grid), grid);

            // top left
            if (y > 0)
                topleft = new PathEdge(this, grid.nodes[x - 1, y - 1], IsDirectionValid(hit, V_UP_LEFT, grid), grid);

            // bottom left
            if (y < grid.height - 1)
                bottomleft = new PathEdge(this, grid.nodes[x - 1, y + 1], IsDirectionValid(hit, V_DOWN_LEFT, grid), grid);
        }

        if (x < grid.width - 1) {
            // right
            right = new PathEdge(this, grid.nodes[x + 1, y], IsDirectionValid(hit, V_RIGHT, grid), grid);

            // top right
            if (y > 0)
                topright = new PathEdge(this, grid.nodes[x + 1, y - 1], IsDirectionValid(hit, V_UP_RIGHT, grid), grid);

            // bottom right
            if (y < grid.height - 1)
                bottomright = new PathEdge(this, grid.nodes[x + 1, y + 1], IsDirectionValid(hit, V_DOWN_RIGHT, grid), grid);
        }

        // top
        if ( y > 0 )
            top = new PathEdge(this, grid.nodes[x, y - 1], IsDirectionValid(hit, V_UP, grid), grid);

        // bottom
        if (y < grid.height - 1)
            bottom = new PathEdge(this, grid.nodes[x, y + 1], IsDirectionValid(hit, V_DOWN, grid), grid);
    }

    // cull nodes with less than 3 edges.
    public void CullWeakNodes(PathGrid grid)
    {
        if (traversable) {
            int edgeCount = 0;

            if (top != null && top.Valid)
                edgeCount++;
            if (bottom != null && bottom.Valid)
                edgeCount++;
            if (left != null && left.Valid)
                edgeCount++;
            if (right != null && right.Valid)
                edgeCount++;
            if (topleft != null && topleft.Valid)
                edgeCount++;
            if (topright != null && topright.Valid)
                edgeCount++;
            if (bottomleft != null && bottomleft.Valid)
                edgeCount++;
            if (bottomright != null && bottomright.Valid)
                edgeCount++;

            if (edgeCount < 3) {
                traversable = false;
                DisableEdges();
            }
        }
    }

    // 
    public void RemoveEdgesToUntraversableNodes(PathGrid grid)
    {
        if (top != null && top.Node != null && !top.Node.traversable)
            top.Valid = false;
        if (bottom != null && bottom.Node != null && !bottom.Node.traversable)
            bottom.Valid = false;
        if (left != null && left.Node != null && !left.Node.traversable)
            left.Valid = false;
        if (right != null && right.Node != null && !right.Node.traversable)
            right.Valid = false;
        if (topleft != null && topleft.Node != null && !topleft.Node.traversable)
            topleft.Valid = false;
        if (topright != null && topright.Node != null && !topright.Node.traversable)
            topright.Valid = false;
        if (bottomleft != null && bottomleft.Node != null && !bottomleft.Node.traversable)
            bottomleft.Valid = false;
        if (bottomright != null && bottomright.Node != null && !bottomright.Node.traversable)
            bottomright.Valid = false;
    }

    // clears out all the edges for this node
    public void DisableEdges()
    {
        if (top != null) top.Valid = false;
        if (bottom != null) bottom.Valid = false;
        if (left != null) left.Valid = false;
        if (right != null) right.Valid = false;
        if (bottomleft != null) bottomleft.Valid = false;
        if (bottomright != null) bottomright.Valid = false;
        if (topright != null) topright.Valid = false;
        if (topleft != null) topleft.Valid = false;

#if DEBUG_PATHFINDING
        DebugNode.transform.GetComponent<SpriteRenderer>().color = Color.red;
#endif
    }

    // Draw edges for debugging.
    public void DrawEdges()
    {
        if (top != null) top.Draw();
        if (bottom != null) bottom.Draw();
        if (left != null) left.Draw();
        if (right != null) right.Draw();
        if (bottomleft != null) bottomleft.Draw();
        if (bottomright != null) bottomright.Draw();
        if (topright != null) topright.Draw();
        if (topleft != null) topleft.Draw();
    }

    // Helpers

    // Returns if a connection from one node to another is valid using a box case in a direction at UnitSize distance.
    private bool IsDirectionValid(RaycastHit2D hit, Vector2 direction, PathGrid pathGrid)
    {
        // Trying with a boxcast instead of rays
        hit = Physics2D.BoxCast(position, pathGrid.UnitVector, 0f, direction, pathGrid.UnitSize, 1 << TILE_LAYER);
        if (hit.collider != null) {
            return false;
        }

        return true;
    }
}
