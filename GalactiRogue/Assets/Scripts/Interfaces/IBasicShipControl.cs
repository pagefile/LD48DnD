using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasicShipControl
{
    void Thrust(float amount);
    void LateralThrust(float amount);
    void Turn(float amount);
    void FullStopDown();
    void FullStopUp();
    void MainWeaponTriggerDown();
    void MainWeaponTriggerUp();
    void SecondaryWeaponTriggerDown();
    void SecondaryWeaponTriggerUp();
}
