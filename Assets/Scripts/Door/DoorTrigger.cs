using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Door
{
    public class DoorTrigger : MonoBehaviour
    {
        public List<String> allowedTags;
        public SlidingDoor[] slidingDoors;
        private void OnTriggerEnter(Collider other)
        {
            if (allowedTags.Contains(other.gameObject.tag))
            {
                foreach (var door in slidingDoors )
                {
                    door.Open();
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (allowedTags.Contains(other.gameObject.tag))
            {
                foreach (var door in slidingDoors)
                {
                    door.Close();
                }
            }
        }
    }
}