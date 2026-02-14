using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class RopeObject : MonoBehaviour
    {
        public float distanceBetweenSegments;
        public Transform attachPoint1;
        public Transform attachPoint2;

        private List<GameObject> _ropeSegments;

        public void Init(Transform at1, Transform at2, List<GameObject> segments)
        {
            attachPoint1 = at1;
            attachPoint2 = at2;
            _ropeSegments = segments;
            CreateVisualRope();
        }

        public void ConnectEndTo(Rigidbody connectTo, Transform newTransform)
        {
            attachPoint2 = newTransform;
            _ropeSegments.Last().GetComponent<Joint>().connectedBody = connectTo;
            _ropeSegments.Last().GetComponent<Joint>().connectedAnchor = newTransform.localPosition;
        }
        private List<GameObject> _cylinders = new List<GameObject>();

        public void CreateVisualRope()
        {
            for (int i = 0; i < _ropeSegments.Count + 1; i++)
            {
                GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                Destroy(cylinder.GetComponent<Collider>()); // don't need physics on the visual
                _cylinders.Add(cylinder);
            }
        }

        private void Update()
        {
            if (attachPoint1 == null || attachPoint2 == null)
            {
                Destroy(transform.gameObject);

            }
            for (int i = -1; i < _cylinders.Count -1; i++)
            {
                Vector3 start, end;
                if (i == -1)
                {
                    start = attachPoint1.position;
                    end = _ropeSegments[0].transform.position;

                }
                else if (i == _cylinders.Count - 2)
                {
                    start =_ropeSegments[i].transform.position;
                    end = attachPoint2.position;
                }
                else
                {
                    start = _ropeSegments[i].transform.position;
                    end = _ropeSegments[i + 1].transform.position;
                }
                Vector3 midpoint = (start + end) / 2f;
                float distance = Vector3.Distance(start, end);

                _cylinders[i + 1].transform.position = midpoint;
                _cylinders[i + 1].transform.up = end - start; // cylinder's long axis is local Y
                _cylinders[i + 1].transform.localScale = new Vector3(0.05f, distance / 2f, 0.05f);
            }
        }

    }
}