using System;
using System.Reflection;

namespace Gun_Stuff
{
    public enum AmmoType
    {
        [AmmoTypeAttr(150)] Pistol,
        [AmmoTypeAttr(300)] Smg,
        [AmmoTypeAttr(250)] Rifle,
        [AmmoTypeAttr(500)] Lmg,
        [AmmoTypeAttr(100)] Shotgun,
        [AmmoTypeAttr(50)] Sniper,
        [AmmoTypeAttr(0)] None
    }

    public class AmmoTypeAttr : Attribute
    {
        internal readonly uint MaxAmmo;

        internal AmmoTypeAttr(uint maxAmmo)
        {
            MaxAmmo = maxAmmo;
        }
    }

    public static class AmmoTypes
    {
        public static uint GetMaxAmmo(this AmmoType at) => GetAttr(at).MaxAmmo;

        private static AmmoTypeAttr GetAttr(AmmoType at)
            => (AmmoTypeAttr)Attribute.GetCustomAttribute(ForValue(at), typeof(AmmoTypeAttr));

        private static MemberInfo ForValue(AmmoType at)
            => typeof(AmmoType).GetField(Enum.GetName(typeof(AmmoType), at));
    }
}