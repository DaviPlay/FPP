using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
public class MeleeData : ScriptableObject, IData
{
    [Header("Info")]
    public new string name;
    [Range(0, 100)]
    [Tooltip("In percentage")]
    public float weight;

    [Header("Attacking")]
    public float damage;
    public float maxDistance;
    [Tooltip("Attacks Per Minute")]
    public float fireRate;
    [Tooltip("How many milliseconds after the key is pressed for the attack to happen\n\nSync with animation")]
    public float attackDelay;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool inspecting;

    public string Name => name;
    public float Weight => weight;
    public AmmoType AmmoType => AmmoType.NONE;
    public int Ammo => 0;
}
