using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

public class Observer : MonoBehaviour
{
    [Range(0, 360)]
    public float fovAngle = 60f;
    public float radius = 7;
    public GameEnding gameEnding;
    public LayerMask targetMask;
    public bool isPlayerInSight = false;
    public GameObject player;

    Vector3 lookDirection = Vector3.forward;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform player = rangeChecks[0].transform;
                Vector3 targetDirection = (player.transform.position - transform.position).normalized;
                float playerAngle = Vector3.Angle(transform.forward, targetDirection);

                if (playerAngle <= fovAngle / 2)
                {
                    //isPlayerInSight = hit.transform.CompareTag("Player");
                    if (isPlayerInSight) Debug.Log($"I see you");
                }
                else
                {
                    isPlayerInSight = false;
                }
            }
            else
            {
                isPlayerInSight = false;
            }
        }
    }
}
