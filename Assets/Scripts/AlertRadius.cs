using System.Collections;
using UnityEngine;

public class AlertRadius : MonoBehaviour
{
    public LayerMask enemyMask;
    public GameObject alertMark;

    AudioSource gargoyleAlertAudio;
    bool alerted = false;
    bool alertMarkSpawned = false;
    Vector3 alertMarkScale = new Vector3(0.05f, 0.15f, 0.05f);
    float scaleDuration = 0.5f;
    float radius = 10f;
    Observer observer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gargoyleAlertAudio = GetComponent<AudioSource>();
        observer = transform.Find("PointOfView").GetComponent<Observer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (observer.isPlayerInSight)
        {
            if (!alerted)
                AlertNearby();
        }
        else
        {
            alerted = false;
        }
    }

    private void AlertNearby()
    {
        if (!alertMarkSpawned)
            StartCoroutine(SpawnAlertMark());

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, enemyMask);
        foreach (Collider collider in rangeChecks)
        {
            if (collider.CompareTag("Ghost")) {
                Debug.Log("Ghost alerted");
                collider.GetComponent<WaypointPatrol>().GoToAlert(transform.position);
            }
        }

        gargoyleAlertAudio.Play();
        alerted = true;
    }

    private IEnumerator SpawnAlertMark()
    {
        alertMarkSpawned = true;
        Vector3 spawnPos = transform.position + transform.forward * 0.4f - transform.right * 0.4f;
        spawnPos.y = 1.05f;
        GameObject spawnedAlertMark = Instantiate(alertMark, spawnPos, Quaternion.identity);

        yield return StartCoroutine(ScaleAlertMark(spawnedAlertMark, Vector3.zero, alertMarkScale));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ScaleAlertMark(spawnedAlertMark, alertMarkScale, Vector3.zero));
        Destroy(spawnedAlertMark);
        alertMarkSpawned = false;
    }

    private IEnumerator ScaleAlertMark(GameObject spawnedAlertMark, Vector3 startScale, Vector3 endScale)
    {
        float timer = 0f;

        while (timer < scaleDuration)
        {
            float factor = timer / scaleDuration;
            spawnedAlertMark.transform.localScale = Vector3.Lerp(startScale, endScale, factor);
            timer += Time.deltaTime;
            yield return null;
        }

        spawnedAlertMark.transform.localScale = endScale;
    }
}
