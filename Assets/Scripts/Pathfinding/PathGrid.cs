using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
    public Vector2 offset;

    public int width;
    public int height;

    public PathNode[,] nodes;

    public GameObject smallEnemyPrefab;
    public GameObject debugNodePrefab;
    public GameObject debugEdgePrefab;
    public float UnitSize = 1f; // TODO: make this handle now square sizes.
    public Vector2 UnitVector;

    private List<Point> points = new List<Point>();

    void Awake()
    {
        // set up grid from an enemy collider size;
        BoxCollider2D smallCollider = smallEnemyPrefab.GetComponents<BoxCollider2D>()[1];
        UnitSize = smallCollider.size.x; // TODO: this seems so dangerous....
        UnitVector = new Vector2(UnitSize, UnitSize);
        Debug.Log("UNIT SIZE: " + UnitSize); // Should be about 1.5 (150 pixels)

        // set up grid based on the sprite transform
        offset = this.transform.position;

        width = (int)(this.transform.localScale.x / UnitSize) + 1;
        height = (int)(this.transform.localScale.y / UnitSize) + 1;

        nodes = new PathNode[width, height];

        // init a square grid
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                float ptx = offset.x + (x * UnitSize);
                float pty = offset.y - (y * UnitSize);
                nodes[x, y] = new PathNode(x, y, new Vector2(ptx, pty), this);
            }
        }

        // generate edges
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (nodes[x, y] == null) continue;
                nodes[x, y].InitEdges(this);
            }
        }

        // remove nodes that have less than 3 edges to other nodes
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (nodes[x, y] == null) continue;
                nodes[x, y].CullWeakNodes(this);
            }
        }

        // remove edges that lead to untraversable nodes
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (nodes[x, y] == null) continue;
                nodes[x, y].RemoveEdgesToUntraversableNodes(this);
                nodes[x, y].DrawEdges();
            }
        }

        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Returns the position on the grid closest to the world position provided.  Returns null if the point is not on the grid.
    public Point WorldToGrid(Vector2 worldPos)
    {
        Vector2 gridPosition = new Vector2(worldPos.x - offset.x, -(worldPos.y - offset.y)) / UnitSize;

        // round to nearest int
        int x = Mathf.RoundToInt(gridPosition.x);
        int y = Mathf.RoundToInt(gridPosition.y);

        if (x < 0 || y < 0 || x > width || y > height)
            return null;

        PathNode node = nodes[x, y];

        if (node != null && node.traversable) {
            return new Point(node.x, node.y);
        }

        return null;
    }

    // Returns the position on the grid closest to the world position provided.  Returns the position closest to the grid if the point is not on the grid.
    public Point WorldToClosestGridPoint(Vector2 worldPos)
    {
        Vector2 gridPosition = new Vector2(worldPos.x - offset.x, -(worldPos.y - offset.y)) / UnitSize;

        // round to nearest int
        int x = Mathf.RoundToInt(gridPosition.x);
        int y = Mathf.RoundToInt(gridPosition.y);

        Point point;

        if (x < 0) { // left of the grid
            if (y < 0) { // above the grid
                point = new Point(0, 0);
            } else if (y > height) { // below the grid
                return new Point(0, height);
            } else { // in grid y-axis
                point = new Point(0, y);
            }
        } else if (x > width) { // right of the grid
            if (y < 0) { // above the grid
                point = new Point(width, 0);
            } else if (y > height) { // below the grid
                point = new Point(width, height);
            } else { // in grid y-axis
                point = new Point(width, y);
            }
        } else { // in grid x-axis
            if (y < 0) { // above the grid
                point = new Point(x, 0);
            } else if (y > height) { // below the grid
                point = new Point(x, height);
            } else { // in grid y-axis
                point = new Point(x, y);
            }
        }

        PathNode node = nodes[point.X, point.Y];

        if (node != null) {
            // If the node found isn't traversable, pick one near it randomly to go to.
            if (!node.traversable) {
                point = GetRandomPoint(point, 3);
            }
        }

        return point;
    }

    public Vector2 GridToWorld(Point point)
    {
        return new Vector2(offset.x + point.X * UnitSize, offset.y - point.Y * UnitSize);
    }

    // returns a random node point node distance from the point provided.
    public Point GetRandomPoint(Point point, int nodeDist)
    {
        points.Clear();

        for (int x = point.X - nodeDist; x < point.X + nodeDist; x++) {
            for (int y = point.Y - nodeDist; y < point.Y + nodeDist; y++) {
                // ignore the point itself
                if (point.X == x && point.Y == y)
                    continue;

                // ignore points that are too far away.
                if (Mathf.Abs(point.X - x) + Mathf.Abs(point.Y - y) > nodeDist)
                    continue;

                // ignore points off the grid.
                if (x < 0 || x >= width || y < 0 || y >= height)
                    continue;

                // assuming the node exists and is traversable, add it to the list.
                if (nodes[x, y] != null && nodes[x, y].traversable) {
                    points.Add(new Point(x, y));
                }
            }
        }

        // return a random, valid point.
        if (points.Count > 0) {
            points.Shuffle();
            return points[0];
        }

        return null;
    }

    public bool IsEdgeValid(Point p1, Point p2, neighbor_direction dir)
    {
        // same node
        if (p1.X == p2.X && p1.Y == p2.Y)
            return false;

        // empty node
        if (nodes[p1.X, p1.Y] == null)
            return false;

        switch (dir) {
            case neighbor_direction.DOWN:
                if (nodes[p1.X, p1.Y].bottom == null) return false;
                return nodes[p1.X, p1.Y].bottom.Valid;

            case neighbor_direction.UP:
                if (nodes[p1.X, p1.Y].top == null) return false;
                return nodes[p1.X, p1.Y].top.Valid;

            case neighbor_direction.RIGHT:
                if (nodes[p1.X, p1.Y].right == null) return false;
                return nodes[p1.X, p1.Y].right.Valid;

            case neighbor_direction.LEFT:
                if (nodes[p1.X, p1.Y].left == null) return false;
                return nodes[p1.X, p1.Y].left.Valid;

            case neighbor_direction.DOWN_LEFT:
                if (nodes[p1.X, p1.Y].bottomleft == null) return false;
                return nodes[p1.X, p1.Y].bottomleft.Valid;

            case neighbor_direction.UP_LEFT:
                if (nodes[p1.X, p1.Y].topleft == null) return false;
                return nodes[p1.X, p1.Y].topleft.Valid;

            case neighbor_direction.DOWN_RIGHT:
                if (nodes[p1.X, p1.Y].bottomright == null) return false;
                return nodes[p1.X, p1.Y].bottomright.Valid;

            case neighbor_direction.UP_RIGHT:
                if (nodes[p1.X, p1.Y].topright == null) return false;
                return nodes[p1.X, p1.Y].topright.Valid;

            default:
                return false;
        }
    }
}