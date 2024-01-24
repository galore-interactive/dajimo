using System;
using UnityEngine;
using GONet;

public class RaycastShooter : MonoBehaviour
{
    public event Action<ShotHitData> OnShotHit;
    private NetworkController _networkController;

    private void Awake()
    {
        _networkController = FindObjectOfType<NetworkController>();
    }

    public void ShootWithRaycast(Vector3 shotPoint, Vector3 shotDirection, int layerMask = ~0, float distance = Mathf.Infinity)
    {
        if (GONetMain.IsServer)
        {
            PerformRaycast(shotPoint, shotDirection, layerMask, distance);
        }
        else if (GONetMain.IsClient)
        {
            PerformRaycast(shotPoint, shotDirection, layerMask, distance);
        }
    }

    public void Server_ShootRegisterShot(ShotConfiguration shotConfig)
    {
        if (GONetMain.IsServer)
        {
            _networkController.Server_RegisterEntityShot(shotConfig);
        }
    }

    private void PerformRaycast(Vector3 shotPoint, Vector3 shotDirection, int layerMask, float distance)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(shotPoint, shotDirection, out hitInfo, distance, layerMask))
        {
            ShotHitData hitData = new ShotHitData(hitInfo.point, hitInfo.normal, hitInfo.distance, hitInfo.transform.tag, hitInfo.transform.gameObject.layer, hitInfo.transform.gameObject);
            OnShotHit?.Invoke(hitData);
        }
    }
}
