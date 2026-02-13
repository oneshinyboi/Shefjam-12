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

        private RecoilState _recoilState = RecoilState.NotRecoiling;
        private enum RecoilState
        {
            NotRecoiling,
            RecoilingBack,
            Resetting,
        }


        private InputAction _fire;


        private void Awake()
        {
            _fire = InputSystem.actions.FindAction("Attack");
            _initialLocalPosition = transform.localPosition;
        }

        private void Update()
        {
            if (_fire.triggered && _recoilState == RecoilState.NotRecoiling)
            {
                _recoilState = RecoilState.RecoilingBack;
                StartCoroutine(DoRecoil());
            }

        }

        private IEnumerator DoRecoil()
        {
            float elapsedTime = 0f;
            float offsetDistance = recoilOffset.magnitude;
            float lerpValue;

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