using System;
using UnityEngine;

namespace DefaultNamespace.Door
{
    public class SlidingDoor : MonoBehaviour
    {
        public GameObject openPosition;
        private Vector3 _openPosition;
        private Vector3 _closePosition;

        public void Awake()
        {
            _closePosition = transform.localPosition;
            _openPosition = openPosition.transform.localPosition;

        }

        public void Update()
        {

        }
        
    }
}