public enum EntityEventType : byte
{
    EV_NONE = 0,
    EV_FIRE_WEAPON = 1,
    EV_RELOAD_WEAPON = 2,
    EV_BULLET_HIT_NON_SHOOTABLE = 3, //Event parameters: shooterEntityID
    EV_BULLET_HIT_SHOOTABLE = 4, //Event parameters: impact position, surface normal, shooterEntityID, vicitmEntityID
    EV_SMASH = 5,
    EV_KILLED = 6,
    EV_COLOR_ASSIGNED = 7,
    EV_IS_PLACING_OBSTACLE = 8,
    EV_ALMOST_KILLED_FORCE_CHANGE_TEAM = 9,
}
