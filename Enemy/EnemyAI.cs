using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public LayerMask playerMask;
    public Transform playerTransform;

    public float hitDistance;
    public float hitDelay;
    public int damage;
    private bool ray;
    private bool hasHit = false;

    RaycastHit hit;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
    }

    void Update()
    {
        Attack();

        transform.gameObject.GetComponent<NavMeshAgent>().SetDestination(playerTransform.position);
    }

    private void Attack()
    {
        ray = Physics.Raycast(transform.position, transform.forward, out hit, hitDistance, playerMask);

        if (ray && !hasHit)
            StartCoroutine(DamagePlayer());
    }

    private IEnumerator DamagePlayer()
    {
        hasHit = true;

        IDamageable damageable = hit.transform.GetComponent<IDamageable>();
        damageable?.Damage(damage);

        yield return new WaitForSeconds(hitDelay);

        hasHit = false;
    }
}
