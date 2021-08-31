using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    private IBasicShipControl _ship;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _ship = GetComponent<IBasicShipControl>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(_ship == null)
        {
            return;
        }

        Ray mouseRay = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        Plane playPlane = new Plane(Vector3.up, _ship.GetControlled().transform.position);
        float distance = 0f;
        if(playPlane.Raycast(mouseRay, out distance))
        {
            Vector3 mousePos = mouseRay.origin + mouseRay.direction * distance;
            Vector3 toMousePos = mousePos - _ship.GetControlled().transform.position;
            float direction = Vector3.Dot(toMousePos.normalized, _ship.GetControlled().transform.right);
            Debug.Log(direction);
            _ship.Turn(Mathf.Clamp(direction, -1f, 1f));
        }


        _ship.Thrust(Input.GetAxis("Vertical"));
        _ship.LateralThrust(Input.GetAxis("Horizontal"));
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
        if(Input.GetButtonDown("Warp"))
        {
            _ship.EngageWarp();
        }
        else if(Input.GetButtonUp("Warp"))
        {
            _ship.DisengageWarp();
        }
    }
}
