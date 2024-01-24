using System;
using UnityEngine;

public interface IEntityInterpolator
{
    //public static Action OnEmptyBuffer;
    public void AddEntityState(EntityState entityState, float time);
    public EntityState InterpolateEntity(float currentTime);
}
