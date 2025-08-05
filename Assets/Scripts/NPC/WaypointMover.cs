using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
  
    [Header("Waypoints")]
    public GameObject[] waypoints;

    public float minDistance = 1f;
    public float speed = 1f;
    public float rotationSpeed = 180f; // degrees per second
    public float timeToWait = 2f;


    private bool _isWaiting = false;
    private int _currentIndex = 0;
    
        

    private void Update()
    {
        if (_isWaiting)
        {
            Transform closestTarget = FindClosestTarget();
            if (closestTarget != null)
                RotateTowardsTarget(closestTarget);
        }
        else
        {
            RotateTowardsTarget(waypoints[_currentIndex].transform);

            transform.position = Vector2.MoveTowards(transform.position, waypoints[_currentIndex].transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, waypoints[_currentIndex].transform.position) < 0.01f)
            {
                StartCoroutine(WaitAndChooseNext());
            }
        }
    }

    private IEnumerator WaitAndChooseNext()
    {
        _isWaiting = true;

        yield return new WaitForSeconds(timeToWait);

        int newIndex;
        do
        {
            newIndex = Random.Range(0, waypoints.Length);
        } while (newIndex == _currentIndex);

        _currentIndex = newIndex;
        _isWaiting = false;
    }

    private Transform FindClosestTarget()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("PointToLook");

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in possibleTargets)
        {
            if (target == this.gameObject) continue;

            bool isWaypoint = false;
            foreach (var wp in waypoints)
            {
                if (target == wp)
                {
                    isWaypoint = true;
                    break;
                }
            }

            if (isWaypoint) continue;

            float dist = Vector2.Distance(transform.position, target.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = target.transform;
            }
        }

        return closest;
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector2 direction = (Vector2)(target.position - transform.position);

        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
 
}
