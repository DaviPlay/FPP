using UnityEngine;

[CreateAssetMenu(fileName = "DoorData", menuName = "Interactable/Door")]
public class DoorData : ScriptableObject, IInteractableData
{
    [Header("Stats")] 
    [SerializeField] private uint cost;
    [Tooltip("In milliseconds")]
    [SerializeField] private float timeToOpen;

    public uint Cost { get; set; }
    public float TimeToOpen => timeToOpen;
    public bool IsOpen { get; set; }
}