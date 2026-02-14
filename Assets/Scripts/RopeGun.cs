using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class RopeGun : MonoBehaviour
    {
        public float recoilSpeed;
        public float returnSpeed;
        private Vector3 _initialLocalPosition;
        public Vector3 recoilOffset;
        public GameObject firePoint;
        public float defaultGrabDistance = 10f;
        public float grabDistanceChangeSpeed = 1f;
        public float minimumGrabDistance = 4f;
        public float maximumGrabDistance = 20f;

        public Rigidbody _playerRigidBody;

        private RecoilState _recoilState = RecoilState.NotRecoiling;
        private enum RecoilState
        {
            NotRecoiling,
            RecoilingBack,
            Resetting,
        }


        private InputAction _fire;
        private InputAction _scroll;
        private InputAction _interact;

        public RopeObjectFactory ropeObjectFactory;
        private RopeObject _currentRope;


        private void Awake()
        {
            _fire = InputSystem.actions.FindAction("Attack");
            _scroll = InputSystem.actions.FindAction("Scroll");
            _interact = InputSystem.actions.FindAction("Interact");
            _initialLocalPosition = transform.localPosition;
        }

        private void Update()
        {
            if (_fire.triggered && _recoilState == RecoilState.NotRecoiling)
            {
                StartCoroutine(DoRecoil());
                StartCoroutine(TryGrabObject());
            }
            else if (_interact.triggered)
            {
                StartCoroutine(DoRecoil());
                if (_currentRope == null)
                {
                    TryRopeObject();
                }
                else
                {
                    TryConnectCurrentRopeToOther();
                    _currentRope = null;
                }
                Debug.Log("pressed interact");
            }

        }

        private void TryConnectCurrentRopeToOther()
        {
            RaycastHit hit;
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, 100))
            {
                if (hit.collider.TryGetComponent<RopeableObject>(out var ropeObject))
                {
                    _currentRope.ConnectEndTo(ropeObject.GetComponent<Rigidbody>());
                }
            }


        }

        private void TryRopeObject()
        {
            RaycastHit hit;
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, 100))
            {
                if (hit.collider.TryGetComponent<RopeableObject>(out var ropeObject))
                {
                    Debug.Log("we got a ropeobject");
                    var newJoint = ropeObject.CreateJoint();
                    _currentRope = ropeObjectFactory.CreateRopeObject(5, hit.point, firePoint.transform.position, newJoint);
                    _currentRope.ConnectEndTo(_playerRigidBody);
                }
            }
        }

        private IEnumerator TryGrabObject()
        {
            RaycastHit hit;
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, 100))
            {
                if (hit.collider.TryGetComponent<GrabableByGun>(out var component))
                {
                    var grabDistance = defaultGrabDistance;
                    component.DisableGravity();
                    while (_fire.IsPressed())
                    {
                        grabDistance += _scroll.ReadValue<float>() * grabDistanceChangeSpeed;
                        grabDistance = Math.Clamp(grabDistance, minimumGrabDistance, maximumGrabDistance);
                        component.MoveTowards(Camera.main.transform.forward * grabDistance + Camera.main.transform.position);
                        yield return null;
                    }
                    component.EnableGravity();
                }
            }
        }

        private IEnumerator DoRecoil()
        {
            float elapsedTime = 0f;
            float offsetDistance = recoilOffset.magnitude;
            float lerpValue;
            _recoilState = RecoilState.RecoilingBack;

            while (_recoilState == RecoilState.RecoilingBack)
            {
                lerpValue = recoilSpeed * elapsedTime / offsetDistance;
                transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + recoilOffset, lerpValue);

                if (lerpValue >= 1)
                {
                    _recoilState = RecoilState.Resetting;
                    elapsedTime = 0f;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            while (_recoilState == RecoilState.Resetting)
            {
                lerpValue = returnSpeed * elapsedTime / offsetDistance;
                transform.localPosition = Vector3.Lerp(transform.localPosition, _initialLocalPosition, lerpValue);

                if (lerpValue >= 1)
                    _recoilState = RecoilState.NotRecoiling;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

    }
}