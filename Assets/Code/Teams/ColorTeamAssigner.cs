using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorTeamConfiguration
{
    #region hard-coded stuff in need of refactor (i.e. not network Color, and just network a byte!
    public const byte BLUE_INDEX = 0;
    public const byte RED_INDEX = 1;
    public const byte GREEN_INDEX = 2;
    public const byte GRAY_INDEX = 3;

    public static readonly Dictionary<Color, byte> IndexByColor = new Dictionary<Color, byte>();
    public static readonly Dictionary<byte, Color> ColorByIndex = new Dictionary<byte, Color>();
    #endregion

    public Color color;
    public byte colorByte;
    public Collider bounds;

    public ColorTeamConfiguration(Color color, Collider collider, byte colorByte)
    {
        this.color = color;
        this.bounds = collider;
        this.colorByte = colorByte;
    }
}

public class ColorTeamAssigner
{
    private readonly ColorTeam[] _availableColorTeams;

    public ColorTeamAssigner(ColorTeamConfiguration[] availableColorTeamConfigurations)
    {
        _availableColorTeams = new ColorTeam[availableColorTeamConfigurations.Length];

        for (int i = 0; i < availableColorTeamConfigurations.Length; ++i)
        {
            byte colorByte = (byte)i;
            availableColorTeamConfigurations[i].colorByte = colorByte;
            _availableColorTeams[i] = new ColorTeam(availableColorTeamConfigurations[i]);
            ColorTeamConfiguration.IndexByColor[_availableColorTeams[i].TeamColor] = colorByte;
            ColorTeamConfiguration.ColorByIndex[(byte)i] = _availableColorTeams[i].TeamColor;
        }
    }

    public byte AssignMemberToTeam(INetworkEntity member, byte? colorToExclude = null)
    {
        int colorTeamIndex = GetColorTeamIndexBasedOnPopulation(colorToExclude);

        _availableColorTeams[colorTeamIndex].AddMember(member);

        return _availableColorTeams[colorTeamIndex].TeamColorByte;
    }

    private int GetRandomColorTeamIndex(byte? colorToExclude = null)
    {
        int colorTeamIndex = Random.Range(0, _availableColorTeams.Length);

        if (colorToExclude != null)
        {
            if (colorToExclude == _availableColorTeams[colorTeamIndex].TeamColorByte)
            {
                if (colorTeamIndex == 0)
                {
                    colorTeamIndex = _availableColorTeams.Length - 1;
                }
                else
                {
                    --colorTeamIndex;
                }
            }
        }

        return colorTeamIndex;
    }

    private int GetColorTeamIndexBasedOnPopulation(byte? colorToExclude = null)
    {
        int colorTeamIndex = -1;
        int lowestNumberOfPlayers = int.MaxValue;

        for (int i = 0; i < _availableColorTeams.Length; ++i)
        {
            if (_availableColorTeams[i].NumberOfMembers < lowestNumberOfPlayers)
            {
                if(colorToExclude != null)
                {
                    if(colorToExclude == _availableColorTeams[i].TeamColorByte)
                    {
                        continue;
                    }
                }

                colorTeamIndex = i;
                lowestNumberOfPlayers = _availableColorTeams[i].NumberOfMembers;
            }
        }

        return colorTeamIndex;
    }

    public void RemoveMemberFromTeam(INetworkEntity member)
    {
        for (int i = 0; i < _availableColorTeams.Length; ++i)
        {
            if (_availableColorTeams[i].ContainsMember(member))
            {
                _availableColorTeams[i].RemoveMember(member);
                break;
            }
        }
    }

    public bool TryGetColorTeamZoneOfPosition(Vector3 position, out byte color)
    {
        bool foundSuccesfully = false;
        color = default;

        for (int i = 0; i < _availableColorTeams.Length; ++i)
        {
            if (_availableColorTeams[i].IsPositionInside(position))
            {
                color = _availableColorTeams[i].TeamColorByte;
                foundSuccesfully = true;
                break;
            }
        }

        return foundSuccesfully;
    }

    public Color GetColor(byte colorByte)
    {
        return ColorTeamConfiguration.ColorByIndex[colorByte];
    }
}
