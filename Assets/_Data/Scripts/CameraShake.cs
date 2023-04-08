using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        CameraShake.instance = this;
    }

    public void ShakeCamera()
    {
        this.cinemachineImpulseSource.GenerateImpulse(Camera.main.transform.forward);
    }
}
