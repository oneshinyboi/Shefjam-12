using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class RopeObject : MonoBehaviour
    {
        public float distanceBetweenSegments;
        public Vector3 attachPoint1;
        public Vector3 attachPoint2;

        private List<GameObject> _ropeSegments;

        public void Init(Vector3 at1, Vector3 at2, List<GameObject> segments)
        {
            attachPoint1 = at1;
            attachPoint2 = at2;
            _ropeSegments = segments;
        }

        public void ConnectEndTo(Rigidbody connectTo)
        {
            _ropeSegments.Last().GetComponent<Joint>().connectedBody = connectTo;
        }

    }
}