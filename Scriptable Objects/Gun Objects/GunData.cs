using EditorAttributes;
using Gun_Stuff;
using Interfaces;
using UnityEngine;

namespace Scriptable_Objects.Gun_Objects
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Weapon/Gun")]
    public class GunData : ScriptableObject, IWeaponData
    {
        [Header("Info")]
        [SerializeField] private new string name;
        [Tooltip("In percentage"), Range(0, 100)]
        [SerializeField] private float weight = 100;

        [Header("Shooting")]
        [SerializeField] private float damage;
        [SerializeField] private float maxDistance;
        [SerializeField] private bool isAuto;
        [Tooltip("Rounds Per Minute")] 
        [SerializeField] private float fireRate;
        [SerializeField] private bool isMelee;
        [ConditionalHide("isMelee", true, true)]
        [MyEnumFilter(AmmoType.None)]
        [SerializeField] private AmmoType ammoType;
        [ConditionalHide("isMelee", true, true)]
        [SerializeField] private uint magSize;
        [ConditionalHide("isMelee", true, true)]
        [Tooltip("In Seconds")] 
        [SerializeField] private float reloadTime;
    
        public string Name
        {
            get => name;
            set => name = value;
        }

        public float Weight
        {
            get => weight;
            set => weight = value;
        }

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        public float MaxDistance
        {
            get => maxDistance;
            set => maxDistance = value;
        }

        public bool IsAuto
        {
            get => isAuto;
            set => isAuto = value;
        }

        public bool Reloading { get; set; }
        public uint MagSize
        {
            get => isMelee ? 9999 : magSize;
            set => magSize = value;
        }

        public uint MagAmmo { get; set; }
        public uint Ammo { get; set; }
        public AmmoType _AmmoType => isMelee ? AmmoType.None : ammoType;

        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }

        public float ReloadTime
        {
            get => reloadTime;
            set => reloadTime = value;
        }

        public bool Inspecting { get; set; }
    }
}
