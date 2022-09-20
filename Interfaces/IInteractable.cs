using UnityEngine;

public interface IInteractable
{
    public void OnInteract(RaycastHit hit);
    public IInteractableData GetData();
}
