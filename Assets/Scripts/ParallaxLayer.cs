using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
     [Tooltip("Factor of movement speed of layer. Speed will be the same as camera with factor 1.")]
    public float parallaxFactor;

    public void Move(float cameraDeltaMovement)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.x -= parallaxFactor * cameraDeltaMovement;

        transform.localPosition = newPosition;
    }
}
