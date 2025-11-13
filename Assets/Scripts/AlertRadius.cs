using UnityEngine;

public class AlertRadius : MonoBehaviour
{
    public LayerMask enemyMask;

    float radius = 10f;
    Observer observer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        observer = transform.Find("PointOfView").GetComponent<Observer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (observer.isPlayerInSight)
        {
            AlertNearby();
        }
    }

    private void AlertNearby()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, enemyMask);

        foreach (Collider collider in rangeChecks)
        {
            if (collider.CompareTag("Ghost")) {
                Debug.Log("Ghost alerted");
                collider.GetComponent<WaypointPatrol>().GoToAlert(transform.position);
            }
        }
    }
}
