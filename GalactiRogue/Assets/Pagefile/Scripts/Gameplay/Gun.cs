using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagefile.Gameplay
{
    public class Gun : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField]
        private float CycleRate = 1.0f;
        [SerializeField]
        private bool Automatic = true;
        [SerializeField][Tooltip("Magzine size of 0 is a bottomless magazine")]
        private int MagazineSize = 0;
        [SerializeField]
        private GameObject Projectile = default;
        [SerializeField]
        private float ProjectileSpeed = 50f;
        [SerializeField]
        private Transform MuzzlePoint = default;
        #endregion

        #region Private Variables
        private int CurrentAmmo = 0;
        private bool TriggerDown = false;
        private float CycleTime = 0.0f;
        private int AmmoDecrementer = 0;
        private bool TriggerReset = true;
        #endregion

        #region Public Functions
        public void OnTriggerDown()
        {
            TriggerDown = true;
        }

        public void OnTriggerUp()
        {
            TriggerDown = false;
            TriggerReset = true;
        }
        #endregion

        #region Unity Functions
	    // Use this for initialization
	    void Start()
        {
		    if(MagazineSize == 0)
            {
                AmmoDecrementer = 0;
                CurrentAmmo = 1;
            }
	    }
	
	    // Update is called once per frame
	    virtual protected void Update()
        {
            CycleTime -= Time.deltaTime;
            if(TriggerReset && TriggerDown && CycleTime <= 0.0f && CurrentAmmo > 0)
            {
                // Fire the gun!
                // TODO: Fix rotation
                GameObject obj = Instantiate(Projectile, MuzzlePoint.transform.position, MuzzlePoint.transform.rotation);
                obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * ProjectileSpeed;
                CycleTime = CycleRate;
                CurrentAmmo -= AmmoDecrementer;
                if(!Automatic)
                {
                    TriggerReset = false;
                }
            }
	    }
        #endregion Unity Functions
    }
}
