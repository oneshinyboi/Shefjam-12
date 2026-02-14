using System;
using UnityEngine;

namespace DefaultNamespace.Door
{
    public class SlidingDoor : MonoBehaviour
    {
        public float speed;
        public Vector3 closedPositionOffset;

        private Vector3 _openPosition;
        private Vector3 _closedPosition;
        private Vector3 _togglePosition;

        private float _travelDistance;
        private float _elapsedTime;

        private enum State
        {
            Closed,
            Closing,
            Open,
            Opening,
        }

        private State _state = State.Closed;
        private bool _shouldBeOpen = false;

        public void Awake()
        {
            _closedPosition = transform.localPosition;
            _openPosition = transform.localRotation * closedPositionOffset + transform.localPosition;
            _travelDistance = (_closedPosition - _openPosition).magnitude;

        }

        public void Open()
        {
            _shouldBeOpen = true;
            if (_state == State.Closed || _state == State.Closing)
            {
                _togglePosition = transform.localPosition;
                _elapsedTime = 0;
                _state = State.Opening;
            }
        }

        public void Close()
        {
            _shouldBeOpen = false;
            if (_state == State.Open || _state == State.Opening)
            {
                _togglePosition = transform.localPosition;
                _elapsedTime = 0;
                _state = State.Closing;
            }

        }

        public void Update()
        {
            if (_state == State.Opening)
            {
                var travelDistance = (_togglePosition - _openPosition).magnitude;
                var lerpFactor =_elapsedTime / (travelDistance / speed);
                transform.localPosition = Vector3.Lerp(_togglePosition, _openPosition, lerpFactor);
                if (lerpFactor >= 1)
                {
                    if (_shouldBeOpen)
                        _state = State.Open;
                    else
                    {
                        _state = State.Closing;
                        _elapsedTime = 0;
                    }

                }
            }

            if (_state == State.Closing)
            {
                var travelDistance = (_togglePosition - _closedPosition).magnitude;
                var lerpFactor = _elapsedTime / (travelDistance / speed);
                transform.localPosition = Vector3.Lerp(_togglePosition, _closedPosition, lerpFactor);
                if (lerpFactor >= 1)
                {
                    if (!_shouldBeOpen)
                        _state = State.Closed;
                    else
                    {
                        _state = State.Opening;
                        _elapsedTime = 0;
                    }
                }
            }

            _elapsedTime += Time.deltaTime;

        }
        
    }
}