using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{

    [SerializeField] private Vector3 sliderOffset = new Vector3(0, 0.8f, 0);

    [Header("Waypoints")]
    public GameObject[] waypoints;

    public Slider cooldownSlider;
    
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
            RotateTowardsTarget(waypoints[_currentIndex].transform); // Rotate towards the current waypoint

            transform.position = Vector2.MoveTowards(transform.position, waypoints[_currentIndex].transform.position, speed * Time.deltaTime); // Move towards the current waypoint

            if (Vector2.Distance(transform.position, waypoints[_currentIndex].transform.position) < 0.01f)
            {
                StartCoroutine(WaitAndChooseNext());
            }
        }
    }

    private void LateUpdate()
    {
        if (cooldownSlider.gameObject.activeSelf)
        {
            cooldownSlider.transform.position = transform.position + sliderOffset;
            cooldownSlider.transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator WaitAndChooseNext()
    {
        _isWaiting = true;
        cooldownSlider.value = 0f;
        cooldownSlider.gameObject.SetActive(true);
        cooldownSlider.maxValue = timeToWait;

        float elapsedTime = 0f;

        while (elapsedTime < timeToWait)
        {
            elapsedTime += Time.deltaTime;
            cooldownSlider.value = elapsedTime;
            yield return null; // Wait for the next frame
        }

        cooldownSlider.gameObject.SetActive(false);

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
