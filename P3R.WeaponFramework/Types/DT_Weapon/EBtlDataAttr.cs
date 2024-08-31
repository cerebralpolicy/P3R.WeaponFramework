namespace P3R.WeaponFramework.Weapons;
[Flags]
public enum EBtlDataAttr : ushort
{
    /// <summary>
    /// Does slash damage
    /// </summary>
    BTL_DATA_ATTR_SLASH,
    /// <summary>
    /// Does strike damage
    /// </summary>
    BTL_DATA_ATTR_STRIKE,
    /// <summary>
    /// Does pierce damage
    /// </summary>
    BTL_DATA_ATTR_PIERCE,
    /// <summary>
    /// Does fire damage
    /// </summary>
    BTL_DATA_ATTR_FIRE,
    /// <summary>
    /// Does ice damage
    /// </summary>
    BTL_DATA_ATTR_ICE,
    /// <summary>
    /// Does electric damage
    /// </summary>
    BTL_DATA_ATTR_ELECTRIC,
    /// <summary>
    /// Does wind damage
    /// </summary>
    BTL_DATA_ATTR_WIND,
    /// <summary>
    /// Does almighty damage
    /// </summary>
    BTL_DATA_ATTR_ALMIGHTY,
    /// <summary>
    /// Does light damage
    /// </summary>
    BTL_DATA_ATTR_LIGHT,
    /// <summary>
    /// Does dark damage
    /// </summary>
    BTL_DATA_ATTR_DARK,
    BTL_DATA_ATTR_CHARM,
    BTL_DATA_ATTR_POISON,
    BTL_DATA_ATTR_UPSET,
    BTL_DATA_ATTR_PANIC,
    BTL_DATA_ATTR_FEAR,
    BTL_DATA_ATTR_ANGER,
    BTL_DATA_ATTR_RECOVERY,
    BTL_DATA_ATTR_SUPPORT,
    BTL_DATA_ATTR_SPECIAL,
    BTL_DATA_ATTR_NON,
    BTL_DATA_ATTR_MAX,
}