using System;
using UnityEngine;

namespace Game
{
    public class Missile : MonoBehaviour
    {
        private int _damage;
        private float _maxDistance;
        private Action<Missile> _returnInActive;
        private Vector3 _startPosition;

        public void Init(int damage, float maxDistance, Action<Missile> inActive)
        {
            _damage = damage;
            _maxDistance = maxDistance;
            _returnInActive = inActive;

            _startPosition = transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(_startPosition, transform.position) > _maxDistance)
            {
                _startPosition = transform.position;
                _returnInActive?.Invoke(this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out EntityView entity))
            {
                entity.Entity.GetSystem<HealthSystem>().TakeDamage(_damage);
            }

            _startPosition = transform.position;
            _returnInActive?.Invoke(this);
        }
    }
}
