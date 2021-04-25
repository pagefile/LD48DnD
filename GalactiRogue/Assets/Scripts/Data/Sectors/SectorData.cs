using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSectorData", menuName = "Data/SectorData")]
public class SectorData : ScriptableObject
{
    [SerializeField]
    private string _type;
    [SerializeField]
    private List<GameObject> _POIList;

    public List<GameObject> POIList
    {
        get { return _POIList; }
    }
}
