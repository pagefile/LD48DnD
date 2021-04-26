using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Systems;
using Pagefile.Systems.Messaging;

public class RadarGUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerShip = default;
    [SerializeField]
    private GameObject _radarPingPrefab = default;

    private List<RadarPingGUI> _pingList = default;
    private Camera _mainCamera;

    private void Start()
    {
        _pingList = new List<RadarPingGUI>();
        MessagePublisher.Instance.Subscribe(typeof(POICreatedMessage), POICreatedHandler);
        MessagePublisher.Instance.Subscribe(typeof(POIClearMessage), POIClearHandler);
        _mainCamera = Camera.main;
    }

    private void POICreatedHandler(Message msg)
    {
        POICreatedMessage poiMsg = msg as POICreatedMessage;
        // HACK: Should use some sort of IFF component or something that'll carry
        // more information
        if(!poiMsg.POI.CompareTag("Nebula"))
        {
            GameObject pingObj = Instantiate(_radarPingPrefab, gameObject.transform);
            RadarPingGUI ping = pingObj.GetComponent<RadarPingGUI>();
            ping.PingObject = poiMsg.POI;
            _pingList.Add(ping);
            RectTransform rTransform = pingObj.GetComponent<RectTransform>();
            rTransform.position = _mainCamera.WorldToScreenPoint(poiMsg.POI.transform.position - _playerShip.transform.position);
        }
    }

    private void POIClearHandler(Message msg)
    {
        foreach(RadarPingGUI ping in _pingList)
        {
            Destroy(ping.gameObject);
        }
        _pingList.Clear();
    }

    private void Update()
    {
        foreach(RadarPingGUI ping in _pingList)
        {
            UpdatePingPosition(ping);
            ping.SetDistance(Vector3.Distance(ping.PingObject.transform.position, _playerShip.transform.position).ToString("0.00"));
        }
    }

    private void UpdatePingPosition(RadarPingGUI ping)
    {
        // HACK: Lots of magic numbers here, but I GOTTA GO FAST
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Rect radarRect = new Rect(Screen.width * 0.1f, Screen.height + 0.1f, Screen.width * 0.8f, Screen.height * 0.8f);
        RectTransform rTransform = ping.gameObject.GetComponent<RectTransform>();
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(ping.PingObject.transform.position);
        Vector3 distance = screenPos - screenCenter;
        if(distance.magnitude > Screen.height * 0.45f)
        {
            screenPos = screenCenter + distance.normalized * screenCenter.y * 0.9f;
            //screenPos.x = Mathf.Clamp(screenPos.x, radarRect.xMin, radarRect.xMax);
            //screenPos.y = Mathf.Clamp(screenPos.y, radarRect.yMin, radarRect.yMax);
        }
        rTransform.position = screenPos;
        ping.SetDistance(Vector3.Distance(ping.PingObject.transform.position, _playerShip.transform.position).ToString("0.00"));
    }
}
