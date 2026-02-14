using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class RopeObjectFactory : MonoBehaviour
    {

        public GameObject RopeSegment;
        public RopeObject CreateRopeObject(int segmentCount, Transform attachPoint1, Transform attachPoint2, SpringJoint attachJoint)
        {
            GameObject newRope = new GameObject();
            var ropeObject = newRope.AddComponent<RopeObject>();

            List<GameObject> ropeSegments = new List<GameObject>();

            Vector3 distance = attachPoint2.position - attachPoint1.position;
            float distanceBetweenSegments = distance.magnitude / segmentCount;
            for (int i = 1; i <= segmentCount; i++)
            {
                var newSegment = Instantiate(RopeSegment, attachPoint1.position + distance.normalized * distanceBetweenSegments * i,
                    Quaternion.identity);
                newSegment.transform.SetParent(newRope.transform);
                if (ropeSegments.Any())
                {
                    var joint = ropeSegments.Last().GetComponent<SpringJoint>();
                    joint.connectedBody = newSegment.GetComponent<Rigidbody>();
                    joint.maxDistance = distanceBetweenSegments + 0.1f;
                }
                else
                {
                    attachJoint.connectedBody = newSegment.GetComponent<Rigidbody>();
                    attachJoint.maxDistance = distanceBetweenSegments + 0.1f;
                }
                ropeSegments.Add(newSegment);
            }

            ropeObject.Init(attachPoint1, attachPoint2, ropeSegments, 4);
            return ropeObject;
        }
    }
}