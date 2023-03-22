using UnityEngine;

public class WaypointFollower : MonoBehaviour {
    [SerializeField] GameObject[] Waypoints;
    [SerializeField] float Speed = 2f;

    int CurrentWaypointIndex = 0;

    void Update() {
        float distanceToWaypoint = Vector2.Distance(
            Waypoints[CurrentWaypointIndex].transform.position,
            transform.position
        );

        if (distanceToWaypoint < .1f) {
            CurrentWaypointIndex++;

            if (CurrentWaypointIndex >= Waypoints.Length) {
                CurrentWaypointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            Waypoints[CurrentWaypointIndex].transform.position,
            Time.deltaTime * Speed
        );
    }
}
