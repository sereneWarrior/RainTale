using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    //Delegate
    public delegate void ParallaxCameraDelegate(float cameraDeltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;

    void Start()
    {
        oldPosition = transform.position.x;
    }

    void Update()
    {
        if(oldPosition != transform.position.x)
        {
            float cameraDeltaMovement = oldPosition - transform.position.x;
            onCameraTranslate(cameraDeltaMovement);
        }
        oldPosition = transform.position.x;
    
    }
}
