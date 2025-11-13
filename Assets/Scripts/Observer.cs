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
                Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
                Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);

                Vector3 targetDirection = (playerPos - pos).normalized;
                float playerAngle = Vector3.Angle(transform.forward, targetDirection);


                isPlayerInSight = playerAngle <= fovAngle / 2 && canSeePlayer(player);
            }
            else
            {
                isPlayerInSight = false;
            }
        }
    }

    private bool canSeePlayer(Transform playerTransform)
    {
        Vector3 direction = playerTransform.position - transform.position + Vector3.up;
        Ray ray = new(transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject.CompareTag("Player");
        }

        return false;
    }
}
