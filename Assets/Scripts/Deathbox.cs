using System;
using UnityEngine;

public class Deathbox : MonoBehaviour {

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("got here 2");
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<PlayerController>()?.Die();

        }
    }
}
