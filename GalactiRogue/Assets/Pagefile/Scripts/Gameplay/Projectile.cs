using System.Collections;
using System.Collections.Generic;
using Pagefile.Components;
using UnityEngine;

namespace Pagefile.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : Entity
    {
        #region Editor Variables
        [SerializeField]
        float Damage = 1.0f;
        [SerializeField]
        bool ExplodeOnCollision = true;
        [SerializeField]
        bool ExplodeOnDamage = true;
        #endregion

        // Use this for initialization
        override protected void Start()
        {
            base.Start();
	    }

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO: Might be a good idea to handle this differently...
            HandleCollision(other);
        }

        // HACK: hack for Breaker. For now I'm just going to handle collider, but for a
        // more robust future proof version it should also handle the fx properly
        private void HandleCollision(Collider other)
        {
            IDamageable damageObj = other.gameObject.GetComponent<IDamageable>();
            if(damageObj != null)
            {
                damageObj.TakeDamage(Damage);
                if(ExplodeOnDamage)
                {
                    Kill();
                    return;
                }
            }
            if(ExplodeOnCollision)
            {
                Kill();
            }
        }
    }
}
