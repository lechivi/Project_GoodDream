using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource cinemachineImpulseSource;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.cinemachineImpulseSource.GenerateImpulse(Camera.main.transform.forward);
        }
    }
}
