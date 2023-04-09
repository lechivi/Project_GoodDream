using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingTest : MonoBehaviour
{
    public PostProcessVolume PostProcessVolume;
    private float value = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.OnDamage();
        }
    }

    private void OnDamage()
    {
        if (this.PostProcessVolume.profile.TryGetSettings(out Vignette vignette))
        {
            this.value += 0.2f;
            this.value = Mathf.Clamp(value, 0f, 0.6f); //ham clamp: gia tri truyen vao trong khoang min - max
            vignette.intensity.value = value;
        }
    }
}
