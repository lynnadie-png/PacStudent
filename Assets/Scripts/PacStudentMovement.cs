using UnityEngine;
using System.Collections.Generic;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 3f;
    public AudioSource audioSource;
    public AudioClip moveSound;

    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypoint = 0;

    void Start()
    {
        waypoints.Add(new Vector3(2, -2, 0));
        waypoints.Add(new Vector3(5, -2, 0));
        waypoints.Add(new Vector3(5, -4, 0));
        waypoints.Add(new Vector3(2, -4, 0));

        transform.position = waypoints[0];

        if (audioSource != null && moveSound != null)
        {
            audioSource.clip = moveSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        Vector3 target = waypoints[currentWaypoint];
        Vector3 direction = (target - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) <= distanceThisFrame)
        {
            transform.position = target;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }
        else
        {
            transform.position += direction * distanceThisFrame;
        }
    }
}