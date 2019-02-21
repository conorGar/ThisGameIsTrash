using UnityEngine;

public enum neighbor_direction
{
    UP,
    LEFT,
    RIGHT,
    DOWN,
    UP_LEFT,
    DOWN_LEFT,
    UP_RIGHT,
    DOWN_RIGHT
}

public static class Pathfinder {
    public static BreadCrumb FindPath(PathGrid grid, Point start, Point end)
    {
        BreadCrumb crumb = FindPathReversed(grid, start, end);
        BreadCrumb[] temp = new BreadCrumb[256];

        // found some path
        if (crumb != null) {
            int i = 0;


            // fill the temp array with a path of breadcrumbs in the order they are.
            while (crumb != null) {
                temp[i] = crumb;
                crumb = crumb.Next;
                i++;
            }

            i -= 2;

            // from the the starting point.
            BreadCrumb current = new BreadCrumb(start);

            // store the head crumb.
            BreadCrumb head = current;

            // flip the list.
            while (i >= 0) {
                current.Next = new BreadCrumb(temp[i].Position);
                current = current.Next;
                i--;
            }

            return head;
        }

        return null;
    }

    // A* pathfinding search.
    private static BreadCrumb FindPathReversed(PathGrid grid, Point start, Point end)
    {
        MinHeap<BreadCrumb> openList = new MinHeap<BreadCrumb>(256);
        BreadCrumb[,] crumbGrid = new BreadCrumb[grid.width, grid.height];
        BreadCrumb crumb;
        Point tmp;
        int cost;
        int diff;

        BreadCrumb curr = new BreadCrumb(start);
        curr.cost = 0;

        BreadCrumb endCrumb = new BreadCrumb(end);
        crumbGrid[curr.Position.X, curr.Position.Y] = curr;
        openList.Add(curr);

#if DEBUG_PATHFINDING_DEEP
        Debug.Log("Start Crumb: [" + curr.Position.X + "," + curr.Position.Y + "]");
        Debug.Log("End Crumb: [" + endCrumb.Position.X + "," + endCrumb.Position.Y + "]");
#endif
        while (openList.Count > 0) {
            // Get the node with the lowest cost and close it
            curr = openList.ExtractMin();

            // close this parent node.
            curr.IsClosed = true;

#if DEBUG_PATHFINDING_DEEP
            Debug.Log("Evaluating Crumb: [" + curr.Position.X + "," + curr.Position.Y + "]");
#endif
            // Examine neighbors
            for (int i = 0; i < neighbors.Length; i++) {
                tmp = new Point(curr.Position.X + neighbors[i].X, curr.Position.Y + neighbors[i].Y);

                // ignore edges that go off the grid or into invalid nodes.
                if (!grid.IsEdgeValid(curr.Position, tmp, neighborDirections[i]))
                    continue;

                // Create a new crumb node if we haven't already, or reevaulate the crumb that is there.
                if (crumbGrid[tmp.X, tmp.Y] == null) {
                    crumb = new BreadCrumb(tmp);
                    crumbGrid[tmp.X, tmp.Y] = crumb;
                } else {
                    crumb = crumbGrid[tmp.X, tmp.Y];
                }

                // if the node isn't closed, score it coming from this parent.
                if (!crumb.IsClosed) {
                    // For cardinal directions (n, s, e, w) add a value of 10 (unit length(1) * 10)
                    // For inter Cardinal directions (ne, nw, se, sw) add a value of 14 ( estimate unit diagonal(sqrt(2)) * 10)
                    // cardinal_discount is used when computing the h heuristic.
                    int intercardinal_cost = 14;
                    int cardinal_discount = 4;

                    // the cost of this neighbor node is the cost of g + h.
                    // g is how far the node is to the start (the parent cost + the cost to go from parent to this neighbor node).
                    // h is an estimate of how far the node is to the end (shortest path to the end).  Computation is the max axis difference (x or y) * 14 (inter cardinal cost) - the difference of the x and y distance * 4 (a cardinal discount because you can bee line these nodes).
                    int x_dist = Mathf.Abs(crumb.Position.X - end.X);
                    int y_dist = Mathf.Abs(crumb.Position.Y - end.Y);

                    int g = neighborCosts[i];
                    int h = Mathf.Max(x_dist, y_dist) * intercardinal_cost - Mathf.Abs(x_dist - y_dist) * cardinal_discount;
                    cost = curr.g + g + h;

                    // if this is the lowest cost so far for the crumb, update it's cost and hook it up to this parent.
                    if (cost < crumb.cost) {
                        crumb.cost = cost;
                        crumb.g = curr.g + g;
                        crumb.Next = curr;
                    }

#if DEBUG_PATHFINDING_DEEP
                    Debug.Log("Crumb: " + neighborDirections[i] + " [" + crumb.Position.X + "," + crumb.Position.Y + "]" + " Cost: " + crumb.cost + " G: " + crumb.g + " H: " + h);
#endif

                    // if node isn't on the open list, add it.
                    if (!crumb.IsOpen) {
                        // if this is the end node we've found the best path!  return the node and it will link all the way back to the start.
                        if (crumb.Equals(endCrumb)) {
                            crumb.Next = curr;
                            return crumb;
                        }

                        // if not, add this node to the open list to be evaluated.
                        crumb.IsOpen = true;
                        openList.Add(crumb);
                    }
                }
            }
        }

        // no path found
        return null;
    }

    private static Point[] neighbors = new Point[] {
        new Point( 0,  1), // down
        new Point(-1,  0), // left
        new Point( 1,  0), // right
        new Point( 0, -1), // up
        new Point(-1,  1), // down left
        new Point(-1, -1), // up left
        new Point( 1,  1), // down right
        new Point( 1, -1)  // up right
    };

    private static int[] neighborCosts = new int[]  {
        10,
        10,
        10,
        10,
        14,
        14,
        14,
        14
    };

    private static neighbor_direction[] neighborDirections = new neighbor_direction[]  {
        neighbor_direction.DOWN,
        neighbor_direction.LEFT,
        neighbor_direction.RIGHT,
        neighbor_direction.UP,
        neighbor_direction.DOWN_LEFT,
        neighbor_direction.UP_LEFT,
        neighbor_direction.DOWN_RIGHT,
        neighbor_direction.UP_RIGHT
    };
}
