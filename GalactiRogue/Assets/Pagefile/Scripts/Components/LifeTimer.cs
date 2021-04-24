using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagefile.Components
{
    // Life time component for Game Objects. For Entities, use Entity.LifeTime
    public class LifeTimer : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField]
        private float LifeTime = 1f;
        #endregion

        #region Public Variables
        #endregion

        #region Private Variables
        #endregion

        #region Public Functions
        #endregion

        #region Unity Functions
	    // Use this for initialization
	    void Start()
        {
            Invoke("KillObject", LifeTime);
	    }
	
	    // Update is called once per frame
	    void Update()
        {
		
	    }
        #endregion Unity Functions

        #region Private Functions
        private void KillObject()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
