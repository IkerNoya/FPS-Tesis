using System;
using UnityEngine;
 
[ExecuteInEditMode]
public class CameraPostProcess : MonoBehaviour
{
    [SerializeField] Material Mat;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, Mat);
    }
}
