public struct EntityInterpolationData
{
    public EntityState entityState;
    public float time;

    public EntityInterpolationData(EntityState entityState = new EntityState(), float time = 0f)
    {
        this.entityState = entityState;
        this.time = time;
    }
}
