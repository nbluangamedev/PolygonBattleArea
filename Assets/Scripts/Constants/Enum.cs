public enum ListenType
{
    ANY = 0,
    ON_PLAYER_DEATH,
    ON_ENEMY_DEATH,
    UPDATE_COUNT_TEXT,
    UPDATE_USER_INFO,
    UPDATE_AMMO,
    SCOPE,
    UNSCOPE,
    ACTIVECROSSHAIR,
    UPDATE_HEALTH,
    CHARACTER_SELECTION,
    SELECTED_CHARACTER,
    SELECTED_MAP,
    ENEMY_COUNT,
    SELECTED_LEVEL,
    ENEMY_SPAWN
}

public enum UIType
{
    Unknow = 0,
    Screen = 1,
    Popup = 2,
    Notify = 3,
    Overlap = 4,
}

public enum AIStateID
{
    ChasePlayer,
    Death,
    Idle,
    FindWeapon,
    Attack,
    FindTarget,
    FindHealth,
    FindAmmo,
    WaypointPatrol
}

public enum SocketID
{
    Spine,
    RightHandRifle,
    RightHandPistol,
    Hip
}

public enum WeaponSlot
{
    Primary = 0,
    Secondary = 1
}

public enum EquipWeaponBy
{
    Player,
    AI
}

public enum WeaponState
{
    Holstering,
    Holstered,
    Activating,
    Active,
    Reloading
}

public enum QualitySetting
{
    LOW,
    MEDIUM,
    HIGH
}