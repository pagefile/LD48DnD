using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    private IBasicShipControl _ship;

    // Start is called before the first frame update
    void Start()
    {
        _ship = GetComponent<IBasicShipControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_ship == null)
        {
            return;
        }

        _ship.Thrust(Input.GetAxis("Throttle"));
        _ship.LateralThrust(Input.GetAxis("Lateral"));
        if(Input.GetButtonDown("PrimaryFire"))
        {
            _ship.MainWeaponTriggerDown();
        }
        else if(Input.GetButtonUp("PrimaryFire"))
        {
            _ship.MainWeaponTriggerUp();
        }
        if(Input.GetButtonDown("SecondaryFire"))
        {
            _ship.SecondaryWeaponTriggerDown();
        }
        else if(Input.GetButtonUp("SecondaryFire"))
        {
            _ship.SecondaryWeaponTriggerUp();
        }
        if(Input.GetButtonDown("FullStop"))
        {
            _ship.FullStopDown();
        }
        if(Input.GetButtonUp("FullStop"))
        {
            _ship.FullStopUp();
        }
    }
}
