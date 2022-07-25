using System;
using System.Reflection;

public enum AmmoType
{
    [AmmoTypeAttr(150)] PISTOL,
    [AmmoTypeAttr(300)] SMG,
    [AmmoTypeAttr(250)] RIFLE,
    [AmmoTypeAttr(500)] LMG,
    [AmmoTypeAttr(100)] SHOTGUN,
    [AmmoTypeAttr(50)] SNIPER,
    [AmmoTypeAttr(0)] NONE
};

public class AmmoTypeAttr : Attribute
{
    internal int maxAmmo;
    internal AmmoType ammoType;

    internal AmmoTypeAttr(int maxAmmo)
    {
        this.maxAmmo = maxAmmo;
    }
}

public static class AmmoTypes
{
    public static int GetMaxAmmo(this AmmoType _at) => GetAttr(_at).maxAmmo;

    public static AmmoTypeAttr GetAttr(AmmoType _at)
        => (AmmoTypeAttr)Attribute.GetCustomAttribute(ForValue(_at), typeof(AmmoTypeAttr));

    public static MemberInfo ForValue(AmmoType _at)
        => typeof(AmmoType).GetField(Enum.GetName(typeof(AmmoType), _at));
}