using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Gameplay;
using UnityEngine.Events;

namespace Pagefile.Components
{
    public class HealthBar : MonoBehaviour, IDamageable
    {
        #region Editor Variables
        [SerializeField]
        private float _maxHealth = 100.0f;
        [SerializeField]
        private bool _destroyOnDeath = true;
        #endregion

        #region Public Variables
        [System.Serializable]
        public class OnDeathEvent : UnityEvent<HealthBar> { }
        public OnDeathEvent OnDeath;
        #endregion

        #region Private Variables
        private float _currentHealth;
        #endregion

        #region Properties
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        #endregion

        #region Unity functions
        // Use this for initialization
        void Start () 
        {
            _currentHealth = _maxHealth;
            if(OnDeath == null)
            {
                OnDeath = new OnDeathEvent();
            }
	    }
        #endregion

        #region Public Methods
        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            if(_currentHealth <= 0.0f)
            {
                OnDeath.Invoke(this);
                if(_destroyOnDeath)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        public void HealDamage(float amount)
        {
            _currentHealth += amount;
            if(_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
        #endregion
    }
}
