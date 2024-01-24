using System.Collections.Generic;
using UnityEngine;

public class ColorTeam
{
    private readonly HashSet<INetworkEntity> _teamEntities;
    private readonly Color _teamColor;
    private readonly byte _teamColorByte;
    private readonly Collider _colorTeamZoneCollider;

    public Color TeamColor => _teamColor;
    public byte TeamColorByte => _teamColorByte;

    public int NumberOfMembers => _teamEntities.Count;

    public ColorTeam(ColorTeamConfiguration config)
    {
        _teamColor = config.color;
        _teamColorByte = config.colorByte;
        _teamColor.a = 1f;

        _colorTeamZoneCollider = config.bounds;
        _teamEntities = new HashSet<INetworkEntity>();
    }

    public void AddMember(INetworkEntity member)
    {
        if(!_teamEntities.Contains(member))
        {
            _teamEntities.Add(member);
        }
    }

    public void RemoveMember(INetworkEntity memberToRemove)
    {
        _teamEntities.Remove(memberToRemove);
    }

    public bool ContainsMember(INetworkEntity memberToRemove)
    {
        return _teamEntities.Contains(memberToRemove);
    }

    public bool IsPositionInside(Vector3 position)
    {
        return _colorTeamZoneCollider.bounds.Contains(position);
    }
}
