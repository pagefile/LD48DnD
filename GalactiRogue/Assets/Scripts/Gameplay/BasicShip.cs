using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Systems;

[RequireComponent(typeof(Rigidbody))]
public class BasicShip : MonoBehaviour, IBasicShipControl
{
    [SerializeField]
    private EngineStats _engine = default;
    [SerializeField]
    private float _maxFuel = 1000f;
    [SerializeField]
    private float _fuelCollectionRate = 5f; // should really be on the nebula, but I'll sort that out later
    [SerializeField]
    private float _minimumJump = 100f; // measured in units of fuel
    [SerializeField]
    private float _maximumJump = 200f; // measured in units of fuel
    public EngineStats Engine { get { return _engine; } }

    private Rigidbody _rb;
    private float _throttle = 0f;
    private float _latAxis = 0f;
    private float _turnAxis = 0f;
    private bool _fullStop = false;
    private bool _warpEngaged = false;
    private float _fuelForWarp = 0f;
    
    private Pagefile.Gameplay.Gun _mainWeapon = default;
    private float _currentFuel = 0f;

    public void AddFuel(float amount)
    {
        _currentFuel += amount;
        if(_currentFuel > _maxFuel)
        {
            _currentFuel = _maxFuel;
        }
        MessagePublisher.Instance.PublishMessage(new FuelUpdateMessage(this, _currentFuel));
    }

    public void SubtractFuel(float amount)
    {
        _currentFuel -= amount;
        if(_currentFuel < 0f)
        {
            _currentFuel = 0f;
        }
        MessagePublisher.Instance.PublishMessage(new FuelUpdateMessage(this, _currentFuel));
    }

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

    public void EngageWarp()
    {
        _warpEngaged = true;
        // This really needs a different name
    }

    public void DisengageWarp()
    {
        _warpEngaged = false;
        if(_fuelForWarp >= _minimumJump)
        {
            // warp to new sector yay!
            _rb.velocity = Vector3.zero;
            MessagePublisher.Instance.PublishMessage(new WarpSuccessMessage(this, _fuelForWarp));
        }
        _fuelForWarp = 0f;
    }

    public GameObject GetControlled()
    {
        return gameObject;
    }

    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mainWeapon = GetComponent<Pagefile.Gameplay.Gun>();
        // Really stupid this isn't a setting somewhere
        _rb.maxAngularVelocity = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_warpEngaged)
        {
            // Check the fuel level. Can't warp with less than 100 fuel because I said so
            if(_currentFuel < _minimumJump && _fuelForWarp <= 0f)
            {
                // nope.avi
                DisengageWarp();
                return;
            }
            if(_currentFuel <= 0f || _fuelForWarp >= _maximumJump)
            {
                // We had fuel but we've used it all for warp
                DisengageWarp();
            }
            float fuelTransfer = 25f * Time.deltaTime;
            _fuelForWarp += fuelTransfer;
            SubtractFuel(fuelTransfer);
        }
    }

    void FixedUpdate()
    {
        if(_warpEngaged)
        {
            _rb.AddForce(_rb.transform.forward * 5000f);
            ApplyFlightAssist();
            return;
        }
        float accel = _engine.Acceleration * _throttle;
        _rb.AddForce(transform.forward * accel, ForceMode.Acceleration);
        _rb.AddForce(transform.right * _engine.TurnAcceleration * _latAxis, ForceMode.Acceleration);

        // I don't understand what is happening here, but this is the only form of the function call that
        // gives me what I want. Using ForceMode.Impulse or ForceMode.Force causes the parent connstraint on
        // the camera to wig out and slingshot around while the ship is spinning. This doesn't
        // happen with ForceMode.Acceleration for some reason, and at first glace it seems the
        // physics of it are all basically the same, except the camera doesn't wig out.
        // This is also a formula I'd use with ForceMode.Impulse and not ForceMode.Accerlation, so this
        // Really is a case of "My code works and I don't understand why". If it works don't
        // fix it?
        // TODO: Test the more "proper" AddTorque call with the new OffsetCamera component
        // MOAR TODO: Dampen turning as it approaches the mouse curser position
        float turn = _engine.TurnAcceleration * _turnAxis;
        _rb.AddTorque(Vector3.up * turn * _rb.mass, ForceMode.Force);

        // Process "Full Stop" physics (more like flight assist/E-Brake)
        if(_fullStop)
        {
            ApplyFlightAssist();
        }
    }

    void ApplyFlightAssist()
    {
        Vector3 velocity = _rb.velocity;
        float speed = velocity.magnitude;
        Vector3 forward = Vector3.zero;
        if(_throttle > 0f || _warpEngaged)
        {
            // If the ship is under powered flight, dampen the newtonian physics, otherwise
            // come to a full stop
            forward = transform.forward;
        }
        Vector3 counterVelocity = _engine.AssistRating * ((forward * speed) - velocity);
        _rb.AddForce(counterVelocity, ForceMode.Acceleration);
        _rb.AddTorque(-_rb.angularVelocity * _rb.mass * Time.deltaTime, ForceMode.Acceleration);
    }

    void LateUpdate()
    {
        _rb.inertiaTensorRotation = new Quaternion(0f, _rb.inertiaTensorRotation.y, 0f, _rb.inertiaTensorRotation.w);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Nebula"))
        {
            AddFuel(_fuelCollectionRate * Time.deltaTime);
        }
    }

    #endregion
}
