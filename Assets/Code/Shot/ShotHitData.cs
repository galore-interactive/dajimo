using UnityEngine;

public struct ShotHitData
{
    public readonly Vector3 position;
    public readonly Vector3 surfaceNormal;
    public readonly string surfaceMaterialType;
    public readonly int surfaceLayerIndex;
    public readonly float distance;
    public readonly GameObject gameObject;

    public ShotHitData(Vector3 position, Vector3 surfaceNormal, float distance, string surfaceMaterialType, int surfaceLayerIndex, GameObject gameObject)
    {
        this.position = position;
        this.surfaceNormal = surfaceNormal;
        this.surfaceMaterialType = surfaceMaterialType;
        this.surfaceLayerIndex = surfaceLayerIndex;
        this.distance = distance;
        this.gameObject = gameObject;
    }
}
