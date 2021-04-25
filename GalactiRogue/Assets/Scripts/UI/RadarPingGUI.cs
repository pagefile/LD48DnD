using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadarPingGUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _distanceText;

    private GameObject _pingObject;

    public GameObject PingObject { get => _pingObject; set => _pingObject = value; }

    public void SetDistance(string distance)
    {
        _distanceText.text = distance;
    }
}
