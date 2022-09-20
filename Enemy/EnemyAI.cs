using System;
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
    private bool _ray;
    private bool _hasHit;

    private RaycastHit _hit;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player").transform;
    }

    private void Update()
    {
        Attack();
        
        try
        {
            transform.gameObject.GetComponent<NavMeshAgent>().SetDestination(playerTransform.position);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void Attack()
    {
        var transform1 = transform;
        _ray = Physics.Raycast(transform1.position, transform1.forward, out _hit, hitDistance, playerMask);

        if (_ray && !_hasHit)
            StartCoroutine(DamagePlayer());
    }

    private IEnumerator DamagePlayer()
    {
        _hasHit = true;

        IDamageable damageable = _hit.transform.GetComponent<IDamageable>();
        damageable?.Damage(damage);

        yield return new WaitForSeconds(hitDelay);

        _hasHit = false;
    }
}
