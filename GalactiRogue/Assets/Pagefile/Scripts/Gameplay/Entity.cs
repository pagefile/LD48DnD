using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagefile.Gameplay
{
    public class Entity : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField]
        protected float LifeTime = 0.0f;
        [SerializeField]
        protected TeamType Team = TeamType.None;
        [SerializeField]
        protected bool Active = true;
        #endregion

        #region Public Delegate Events

        public delegate void EntityEvent(Entity e);
        public event EntityEvent OnDeath;
        public event EntityEvent OnDestroyed;

        public bool Dying
        {
            get { return IsDying; }
            private set { }
        }

        #endregion

        #region Private Variables
        #endregion

        #region Protected members
        protected bool IsDying = false;
        #endregion

        #region Unity Functions
        // Use this for initialization
        protected virtual void Start()
        {
            if(LifeTime > 0.0f)
            {
                Invoke("Kill", LifeTime);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {
            if(IsDying)
            {
                Destroy(gameObject);
                OnDeath?.Invoke(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if(OnDestroyed != null)
            {
                OnDestroyed(this);
            }
        }
        #endregion

        #region Public Members
        public enum TeamType
        {
            None,
            GoodGuys,
            BadGuys,
        }
        public TeamType CurrentTeam
        {
            get { return Team; }
        }

        public virtual void Kill()
        {
            CancelInvoke("Kill");
            if(!IsDying)
            {
                // Really unecessary given the code is simple right now, but seems like a safe thing to do to future proof it
                IsDying = true;
            }
        }

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
        #endregion
    }

}