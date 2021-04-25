using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSectorData", menuName = "Data/SectorData")]
public class SectorData : ScriptableObject
{
    [SerializeField]
    private string _type;
    [SerializeField][Range(0f, 100f)]
    private float _density = 50f;
    [SerializeField]
    private int _minPOICount = 5;
    [SerializeField][Tooltip("Total size is in square meters")]
    private float _totalSize = 1000f;
    [SerializeField][Tooltip("Grid size is in square meters")]
    private float _gridSize = 100f;
    [SerializeField]
    private List<GameObject> _POIList = default;

    public List<GameObject> POIList
    {
        get { return _POIList; }
    }

    public float Density
    {
        get { return _density; }
    }

    public int MinPOICount
    {
        get { return _minPOICount; }
    }

    // I should use the quick refactoring more often...
    public float TotalSize { get => _totalSize; }
    public float GridSize { get => _gridSize; }
}
