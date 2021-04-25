using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pagefile.System;
using Pagefile.System.Messaging;

public class MainGUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _fuelText = default;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MessagePublisher.Instance.Subscribe(typeof(FuelUpdateMessage), UpdateFuelHandler);
    }

    void UpdateFuelHandler(Message msg)
    {
        FuelUpdateMessage fuelMsg = msg as FuelUpdateMessage;
        _fuelText.text = fuelMsg.TotalFuel.ToString("0");
    }
}
