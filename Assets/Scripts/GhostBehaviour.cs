using StealthGame;
using System.Collections;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public bool isChasing = false;

    GameObject player;
    GameEnding gameEnding;
    Observer observer;
    float chaseDuration = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameEnding = GameObject.Find("GameEnding").GetComponent<GameEnding>();
        observer = transform.Find("PointOfView").GetComponent<Observer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (observer.isPlayerInSight)
        {
            StartCoroutine(StartChase());
        }
    }

    private IEnumerator StartChase()
    {
        float timer = 0.0f;
        isChasing = true;

        while (timer <= chaseDuration)
        {
            timer += 0.2f;
            Debug.Log(timer);

            if (observer.isPlayerInSight)
            {
                timer = 0.0f;
            }

            yield return new WaitForSeconds(0.2f);
        }

        StopChase();
    }

    private void StopChase()
    {
        isChasing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            gameEnding.CaughtPlayer();
        }
    }
}
