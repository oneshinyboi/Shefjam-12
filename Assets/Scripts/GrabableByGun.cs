using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GrabableByGun : RopeableObject
    {
        public float grabForce = 1;
        private Rigidbody _rb;

        public void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void MoveTowards(Vector3 pos)
        {
            Vector3 distance = pos - transform.position;
            float force = 1f - (float) Math.Pow(Math.E, -distance.magnitude);
            _rb.AddForce(distance.normalized * (force * grabForce), ForceMode.Force);
        }

        public void DisableGravity()
        {
            _rb.useGravity = false;
        }

        public void EnableGravity()
        {
            _rb.useGravity = true;
        }
    }
}