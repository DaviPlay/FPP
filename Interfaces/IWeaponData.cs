using Gun_Stuff;

namespace Interfaces
{
    public interface IWeaponData
    {
        public string Name { get; }
        public float Weight { get; }
        public bool Reloading { get; }
        public uint MagSize { get; }
        public uint MagAmmo { get; set; }
        public uint Ammo { get; set; }
        public AmmoType _AmmoType { get; }
    }
}
