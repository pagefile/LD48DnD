using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Gameplay;

namespace Pagefile.Components
{
    // TODO: If Entity is a required component for HealthBar, why not just add health
    // funcionality to Entity?
    [RequireComponent(typeof(Entity))]
    public class HealthBar : MonoBehaviour, IDamageable
    {
        #region Editor Variables
        [SerializeField]
        private float MaxHealth = 100.0f;
        #endregion

        #region Private Variables
        private float _currentHealth;
        Entity Ent;
        #endregion

        #region Properties
        public float CurrentHealth => _currentHealth;
        #endregion

        #region Unity functions
        // Use this for initialization
        void Start () 
        {
            _currentHealth = MaxHealth;
            Ent = GetComponent<Entity>();
	    }
        #endregion

        #region Public Methods

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            if(_currentHealth <= 0.0f)
            {
                Ent.Kill();
            }
        }
        
        public void HealDamage(float amount)
        {
            _currentHealth += amount;
            if(_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
        }

        #endregion
    }
}
