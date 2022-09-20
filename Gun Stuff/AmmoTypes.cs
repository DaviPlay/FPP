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
}

public class AmmoTypeAttr : Attribute
{
    internal readonly int MaxAmmo;

    internal AmmoTypeAttr(int maxAmmo)
    {
        MaxAmmo = maxAmmo;
    }
}

public static class AmmoTypes
{
    public static int GetMaxAmmo(this AmmoType at) => GetAttr(at).MaxAmmo;

    private static AmmoTypeAttr GetAttr(AmmoType at)
        => (AmmoTypeAttr)Attribute.GetCustomAttribute(ForValue(at), typeof(AmmoTypeAttr));

    private static MemberInfo ForValue(AmmoType at)
        => typeof(AmmoType).GetField(Enum.GetName(typeof(AmmoType), at));
}