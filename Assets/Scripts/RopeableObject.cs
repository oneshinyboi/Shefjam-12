using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class RopeableObject : MonoBehaviour
    {
        public SpringJoint CreateJoint()
        {
            var newSpringJoint = this.AddComponent<SpringJoint>();
            newSpringJoint.spring = 50;
            newSpringJoint.autoConfigureConnectedAnchor = false;
            return newSpringJoint;
        }


    }
}