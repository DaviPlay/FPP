﻿using Interfaces;
using Player;
using Scriptable_Objects.Interactable_Objects;
using UnityEngine;

namespace Interactable_Stuff
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private DoorData data;

        //REPLACE WITH ACTUAL ANIMATION YOU IDIOT
        //(I'm not even gonna attempt to lerp it)
        public void OnInteract(RaycastHit hit)
        {
            if (data.Cost > PointManager.Points)
                return;

            PointManager.Points -= data.Cost;
            Shooting.UpdateText?.Invoke();
        
            var hinge = hit.transform.parent;
            var rotation = hinge.rotation.eulerAngles;
        
            if (!data.IsOpen)
            {
                hinge.rotation = Quaternion.Euler(rotation.x, rotation.y - 90, rotation.z);
            
                data.IsOpen = true;
            }
            else
            {
                hinge.rotation = Quaternion.Euler(rotation.x, rotation.y + 90, rotation.z);
            
                data.IsOpen = false;
            }
        }

        public IInteractableData GetData() => data;
    }
}
