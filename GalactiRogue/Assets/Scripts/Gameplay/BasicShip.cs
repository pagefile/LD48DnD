using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicShip : MonoBehaviour, IBasicShipControl
{
    [SerializeField]
    private float _acceleration = 1f;
    [SerializeField]
    private float _turnAccel = 2f;

    private Rigidbody _rb;
    private float _throttle = 0f;
    private float _latAxis = 0f;
    private float _turnAxis = 0f;

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

    }

    public void SecondaryWeaponTriggerDown()
    {

    }

    public void MainWeaponTriggerUp()
    {

    }

    public void SecondaryWeaponTriggerUp()
    {

    }

    public void FullStopUp()
    {
        _rb.drag = 0f;
        _rb.angularDrag = 0f;
    }

    public void FullStopDown()
    {
        _rb.drag = 0.5f;
        _rb.angularDrag = 0.5f;
    }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Probably shoot the guns or something
    }

    void FixedUpdate()
    {
        float accel = _acceleration * _throttle;
        _rb.AddForce(transform.forward * accel, ForceMode.Acceleration);

        float turn = _turnAccel * _latAxis;

        // I don't understand what is happening here, but this is the only form of the function call that
        // gives me what I want. Using ForceMode.Impulse or ForceMode.Force causes the parent connstraint on
        // the camera to wig out and slingshot around while the ship is spinning. This doesn't
        // happen with ForceMode.Acceleration for some reason, and at first glace it seems the
        // physics of it are all basically the same, except the camera doesn't wig out.
        // This is also a formula I'd use with ForceMode.Impulse and not ForceMode.Accerlation, so this
        // Really is a case of "My code works and I don't understand why". If it works don't
        // fix it? 
        _rb.AddTorque(Vector3.up * _latAxis * _rb.mass * Time.deltaTime, ForceMode.Acceleration);
    }
    #endregion
}
