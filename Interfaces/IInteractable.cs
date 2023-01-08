using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        public void OnInteract(RaycastHit hit);
        public IInteractableData GetData();
    }
}
