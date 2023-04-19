using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEndScript : MonoBehaviour
{
    public void FinishAnimation()
    {
        Destroy(gameObject);
    }
}
