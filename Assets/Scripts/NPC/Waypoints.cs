using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float wayPointSize = .3f;

    private void OnDrawGizmos()
    {

        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, wayPointSize);
        }

        Gizmos.color = Color.red;


        for (int i = 0; i < transform.childCount - 1; i++)
        {
            for (int j = i + 1; j < transform.childCount; j++)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
            }
        }
    }
}
