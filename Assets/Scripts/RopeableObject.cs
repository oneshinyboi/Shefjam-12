using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class RopeableObject : MonoBehaviour
    {
        public SpringJoint CreateJoint(Vector3 hitPoint)
        {

            var newSpringJoint = this.AddComponent<SpringJoint>();
            newSpringJoint.spring = 50;
            newSpringJoint.autoConfigureConnectedAnchor = false;
            newSpringJoint.anchor = transform.InverseTransformPoint(hitPoint);

            return newSpringJoint;
        }
        public Transform CreateAttachPointTracker(Vector3 point, Transform attachTo)
        {
            var attachPointTracker = new GameObject();
            attachPointTracker.transform.position = point;
            attachPointTracker.transform.SetParent(attachTo);

            return attachPointTracker.transform;
        }


    }
}