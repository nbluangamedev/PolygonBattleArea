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
    ENEMY_HEADSHOT,
    SELECTED_LEVEL,
    ENEMY_SPAWN,
    GET_AUDIOSOURCE,
    UPDATE_MOUSE_SPEED,
    PLAYER_DEATH,
    HOLSTER_WEAPON_UI,
    DROP_WEAPON_UI,
    ACTIVE_WEAPON_UI
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
    Secondary = 1,
    Knife = 2,
    Grenade = 3
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