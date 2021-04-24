using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEngineStats", menuName = "Data/EngineStats")]
public class EngineStats : ScriptableObject
{
    [SerializeField]
    private string _name = "Stock Engine";
    [SerializeField]
    private float _acceleration = 2f;
    [SerializeField]
    private float _turnAcceleration = 2f;
    [SerializeField]
    private float _assistRating = 1f;

    public float Acceleration
    {
        get { return _acceleration; }
    }

    public float TurnAcceleration
    {
        get { return _turnAcceleration; }
    }

    public float AssistRating
    {
        get { return _assistRating; }
    }
    
    public string Name
    {
        get { return _name; }
    }
}
