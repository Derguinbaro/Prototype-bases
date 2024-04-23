using System.Collections.Generic;
using UnityEngine;

public enum WaypointDirection
{
    Forward,
    Left,
    Right,
    Backward
}

[System.Serializable]
public class WaypointInfo
{
    public Transform waypoint;
    public List<WaypointDirection> directions;
}

public class WaypointManager : MonoBehaviour
{
    public List<WaypointInfo> waypoints = new List<WaypointInfo>();
    private int currentWaypointIndex = 0;

    private void Start()
    {
        if (waypoints.Count > 0)
        {
            Transform startingWaypoint = waypoints[currentWaypointIndex].waypoint;
            MoveToWaypoint(startingWaypoint);
        }
    }

    private void Update()
    {
        if (waypoints.Count == 0)
            return;

        if (Input.GetKeyDown(KeyCode.W))
            MoveToDirection(WaypointDirection.Forward);
        else if (Input.GetKeyDown(KeyCode.A))
            MoveToDirection(WaypointDirection.Left);
        else if (Input.GetKeyDown(KeyCode.D))
            MoveToDirection(WaypointDirection.Right);
        else if (Input.GetKeyDown(KeyCode.S))
            MoveToDirection(WaypointDirection.Backward);
    }

    private void MoveToDirection(WaypointDirection direction)
    {
        WaypointInfo currentWaypoint = waypoints[currentWaypointIndex];

        if (currentWaypoint.directions.Contains(direction))
        {
            int newIndex = currentWaypointIndex;

            // Determine the index of the next waypoint based on the chosen direction.
            switch (direction)
            {
                case WaypointDirection.Forward:
                    newIndex++;
                    break;
                case WaypointDirection.Left:
                    newIndex--;
                    break;
                case WaypointDirection.Right:
                    newIndex = (newIndex + 2) % waypoints.Count; // Skip one waypoint ahead.
                    break;
                case WaypointDirection.Backward:
                    newIndex = (newIndex + waypoints.Count - 1) % waypoints.Count; // Move backwards.
                    break;
            }

            currentWaypointIndex = newIndex;
            Transform nextWaypoint = waypoints[currentWaypointIndex].waypoint;
            MoveToWaypoint(nextWaypoint);
        }
    }

    private void MoveToWaypoint(Transform waypoint)
    {
        // Implement your movement logic here.
        // For example, you might want to use NavMeshAgent or a custom movement script.
        // For demonstration purposes, let's teleport the GameObject to the waypoint.
        transform.position = waypoint.position;
    }
}



