using Gun_Stuff;
using Interfaces;
using UnityEngine;

namespace Scriptable_Objects.Gun_Objects
{
    [CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
    public class MeleeData : ScriptableObject, IWeaponData
    {
        [Header("Info")]
        [SerializeField] private new string name;
        [Range(0, 100)]
        [Tooltip("In percentage")]
        [SerializeField] private float weight;

        [Header("Attacking")]
        [SerializeField] private float damage;
        [SerializeField] private float maxDistance;
        [Tooltip("Attacks Per Minute")]
        [SerializeField] private float fireRate;
        [Tooltip("How many milliseconds after the key is pressed for the attack to happen\n\nSync with animation")]
        [SerializeField] private float attackDelay;

        public string Name => name;
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

        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }

        public float AttackDelay => attackDelay;
        public bool Reloading => false;
        public uint MagSize => 0;
        public uint MagAmmo { get => 0; set {} }
        public uint Ammo { get => 0; set {} }
        public AmmoType AmmoType => AmmoType.None;
    
        public bool Inspecting { get; set; }
        public bool Attacking { get; set; }
    }
}
