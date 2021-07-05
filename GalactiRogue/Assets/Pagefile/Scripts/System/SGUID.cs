using System;
using UnityEngine;

[Serializable]
public class SGUID : ISerializationCallbackReceiver
{
    [SerializeField]
    private string _strGuid;
    private Guid _guid;

    public static implicit operator SGUID(Guid guid) => new SGUID(guid);

    public SGUID(Guid guid)
    {
        _guid = guid;
        _strGuid = _guid.ToString();
    }

    public void OnAfterDeserialize()
    {
        _guid = Guid.Parse(_strGuid);
    }

    public void OnBeforeSerialize()
    {
        _strGuid = _guid.ToString();
    }

    static public bool operator==(SGUID l, SGUID r)
    {
        return l?._guid == r?._guid;
    }

    static public bool operator!=(SGUID l, SGUID r)
    {
        return l?._guid != r?._guid;
    }

    public override bool Equals(object obj)
    {
        SGUID guidobj = obj as SGUID;
        return guidobj != null && _guid == guidobj._guid;
    }

    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }

    public override string ToString()
    {
        return _guid.ToString();
    }
}
