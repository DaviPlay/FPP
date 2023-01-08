using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        
        private Transform _playerTransform;

        public float hitDistance;
        [Tooltip("In seconds")] public float hitDelay;
        public int damage;
        private bool _ray;
        private bool _hasHit;

        private RaycastHit _hit;

        private void Start() => _playerTransform = GameObject.FindWithTag("player").transform;
        
        private void Update()
        {
            var transform1 = transform;
            _ray = Physics.Raycast(transform1.position, transform1.forward, out _hit, hitDistance, playerMask);
            
            if (_ray && !_hasHit)
            {
                StartCoroutine(DamagePlayer());
                return;
            }
        
            transform1.GetComponent<NavMeshAgent>().SetDestination(_playerTransform.position);
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
}
