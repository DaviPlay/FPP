using System;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [SerializeField] private int interactDistance;
    private LayerMask _interactableMask;
    private Transform _eyes;

    private static Action _interactInput;

    private void Start()
    {
        _interactableMask = LayerMask.GetMask("Interactable");
        _eyes = Camera.main!.transform;
        _interactInput += Interact;
        
        InteractDistance = interactDistance;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
            _interactInput?.Invoke();
    }

    private void Interact()
    {
        if (!Physics.Raycast(_eyes.position, _eyes.forward, out RaycastHit hit, InteractDistance, _interactableMask)) return;
        
        IInteractable interactable = hit.transform.GetComponent<IInteractable>();
        interactable?.OnInteract(hit);
    }

    private static int InteractDistance { get; set; }
}