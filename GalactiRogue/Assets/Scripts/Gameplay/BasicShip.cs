using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicShip : MonoBehaviour, IBasicShipControl
{
    [SerializeField]
    private EngineStats _engine = default;

    private Rigidbody _rb;
    private float _throttle = 0f;
    private float _latAxis = 0f;
    private float _turnAxis = 0f;
    private bool _fullStop = false;
    private Pagefile.Gameplay.Gun _mainWeapon = default;

    #region IBasicShipControl Implementation
    public void Thrust(float amount)
    {
        _throttle = amount;
    }

    public void LateralThrust(float amount)
    {
        _latAxis = amount;
    }

    public void Turn(float amount)
    {
        _turnAxis = amount;
    }

    public void MainWeaponTriggerDown()
    {
        _mainWeapon.OnTriggerDown();
    }

    public void MainWeaponTriggerUp()
    {
        _mainWeapon.OnTriggerUp();
    }

    public void SecondaryWeaponTriggerDown()
    {

    }


    public void SecondaryWeaponTriggerUp()
    {

    }

    public void FullStopUp()
    {
        _fullStop = false;
    }

    public void FullStopDown()
    {
        _fullStop = true;
    }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainWeapon = GetComponent<Pagefile.Gameplay.Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Probably shoot the guns or something
    }

    void FixedUpdate()
    {
        float accel = _engine.Acceleration * _throttle;
        _rb.AddForce(transform.forward * accel, ForceMode.Acceleration);

        // I don't understand what is happening here, but this is the only form of the function call that
        // gives me what I want. Using ForceMode.Impulse or ForceMode.Force causes the parent connstraint on
        // the camera to wig out and slingshot around while the ship is spinning. This doesn't
        // happen with ForceMode.Acceleration for some reason, and at first glace it seems the
        // physics of it are all basically the same, except the camera doesn't wig out.
        // This is also a formula I'd use with ForceMode.Impulse and not ForceMode.Accerlation, so this
        // Really is a case of "My code works and I don't understand why". If it works don't
        // fix it?
        // TODO: Test the more "proper" AddTorque call with the new OffsetCamera component
        float turn = _engine.TurnAcceleration * _latAxis;
        _rb.AddTorque(Vector3.up * _latAxis * _rb.mass * Time.deltaTime, ForceMode.Acceleration);

        // Process "Full Stop" physics (more like flight assist/E-Brake)
        if(_fullStop)
        {
            Vector3 velocity = _rb.velocity;
            float speed = velocity.magnitude;
            Vector3 forward = Vector3.zero;
            if(_throttle > 0f)
            {
                // If the ship is under powered flight, dampen the newtonian physics, otherwise
                // come to a full stop
                forward = transform.forward;
            }
            Vector3 counterVelocity = _engine.AssistRating * ((forward  * speed)  - velocity);
            _rb.AddForce(counterVelocity, ForceMode.Acceleration);
            _rb.AddTorque(-_rb.angularVelocity * _rb.mass * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    void LateUpdate()
    {
        _rb.inertiaTensorRotation = new Quaternion(0f, _rb.inertiaTensorRotation.y, 0f, _rb.inertiaTensorRotation.w);
    }
    #endregion
}
