using System;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private enum State
    {
        On,
        TurningOn,
        Off,
        TurningOff
    }

    private Vector3 _startY = new Vector3(0, -0.6f, 0);
    private Vector3 _endY = new Vector3(0, -0.5f, 0);
    private float _timeElapsed = 0;
    private float lerpDuration = 3;
    private State _state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _state = State.Off;
    }

    void OnCollisionEnter(Collision collision)
    {
        // if player enters
        if (collision.gameObject.CompareTag("Player"))
        {
            // move button slightly down
            Debug.Log("turning on");
            _state = State.TurningOn;
        }
        else
        {
            if (_state == State.On)
            {
                Debug.Log("turning off");
                _state = State.TurningOff;
            }
        }
        HandleOnOff();
    }

    void HandleOnOff()
    {
        if (_state == State.TurningOn)
        {
            transform.localPosition = Vector3.Lerp(_startY, _endY, _timeElapsed / lerpDuration);
        }
        else if (_state == State.TurningOff)
        {
            transform.localPosition = Vector3.Lerp(_endY, _startY, _timeElapsed / lerpDuration);
        }
    }
}
