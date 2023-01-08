using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
    
    public class MyEnumFilterAttribute : PropertyAttribute
    {
        public int[] Values { get; set; }
        public string[] Labels { get; set; }

        public MyEnumFilterAttribute(params AmmoType[] ignoredValues)
        {
            var values = new List<int>((int[]) Enum.GetValues(typeof(AmmoType)));
            var labels = new List<string>(Enum.GetNames(typeof(AmmoType)));
 
            for (int i = ignoredValues.Length - 1 ; i >= 0 ; i--)
            {
                int index = values.IndexOf((int) ignoredValues[i]);
                values.RemoveAt(index);
                labels.RemoveAt(index);
            }
 
            Values = values.ToArray();
            Labels = labels.ToArray();
        }
    }
}