using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingObject : MonoBehaviour
    {
        public List<GameObject> gameObjects;
        public float speed;

        private Rigidbody _rb;
        private int _currentIndex = 0;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            Vector3 target = gameObjects[_currentIndex].transform.position;
            float step = speed * Time.deltaTime;
            _rb.MovePosition(Vector3.MoveTowards(transform.position, target, step));

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                _currentIndex = (_currentIndex + 1) % gameObjects.Count;
            }
        }

    }
}
