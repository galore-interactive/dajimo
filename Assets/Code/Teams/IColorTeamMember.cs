using UnityEngine;

public interface IColorTeamMember
{
    public void Server_SetColorTeam(byte color);
    public byte GetColorTeam();
    public Vector3 GetPositon();
}
