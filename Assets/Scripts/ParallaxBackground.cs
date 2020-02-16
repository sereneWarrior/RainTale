using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera camera;
    private List<ParallaxLayer> layerList= new List<ParallaxLayer>();


    void Start()
    {
        if(!camera)
            camera = Camera.main.GetComponent<ParallaxCamera>();
        
        camera.onCameraTranslate += MoveLayers;
        SetLayers();
    }

    private void SetLayers()
    {
        layerList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if(layer)
            {
                layerList.Add(layer);
            }
        }
    }

    private void MoveLayers(float cameraDeltaMovement)
    {
        foreach ( ParallaxLayer layer in layerList)
            layer.Move(cameraDeltaMovement);
    }
    
}
