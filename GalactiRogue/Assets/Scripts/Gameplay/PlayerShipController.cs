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
            // TODO: This is very hacky
            Rigidbody rb = _ship.GetControlled().GetComponent<Rigidbody>();
            BasicShip ship = _ship.GetControlled().GetComponent<BasicShip>();
            Vector3 mousePos = mouseRay.origin + mouseRay.direction * distance;
            Vector3 toMousePos = mousePos - _ship.GetControlled().transform.position;
            toMousePos.Normalize();
            float angleBetween = Vector3.Angle(toMousePos, _ship.GetControlled().transform.forward);
            float timeToMouse = angleBetween / (Mathf.Abs(rb.angularVelocity.y * Mathf.Rad2Deg));
            //Debug.Log(timeToMouse + " " + Mathf.Abs(rb.angularVelocity.y * Mathf.Rad2Deg) / ship.Engine.TurnAcceleration);
            //Debug.Log(Mathf.Abs(rb.angularVelocity.y * Mathf.Rad2Deg) / ship.Engine.TurnAcceleration);
            float direction = Vector3.Dot(toMousePos, _ship.GetControlled().transform.right);
            //if(timeToMouse <= Mathf.Abs(rb.angularVelocity.y * Mathf.Rad2Deg) / ship.Engine.TurnAcceleration)
            //{
            //    _ship.Turn(-Mathf.Sign(direction));
            //}
            if(Mathf.Abs(rb.angularVelocity.y * Mathf.Rad2Deg) * Time.fixedDeltaTime >= angleBetween)
            {
                ship.GetControlled().transform.LookAt(mousePos);
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                _ship.Turn(Mathf.Sign(direction));
            }
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
