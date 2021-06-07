using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
 
[ExecuteInEditMode]
public class CameraPostProcess : MonoBehaviour
{
    [SerializeField] Material Mat;
    [SerializeField] float chromaticAberration;
    [SerializeField] float lensDistortion;
    void Awake()
    {
        Player.TakeDamage += TakeDamage;
    }
    void Start()
    {
        chromaticAberration = Mat.GetFloat("_ChromaticAberration");
        lensDistortion = Mat.GetFloat("_LensDistortion");
    }
    void TakeDamage(Player player)
    {
        StartCoroutine(changeFloat("_ChromaticAberration", 30.0f, chromaticAberration));
        StartCoroutine(changeFloat("_LensDistortion", lensDistortion + 0.2f, lensDistortion));
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, Mat);
    }

    IEnumerator changeFloat(string name, float value, float originalValue)
    {
        Mat.SetFloat(name, value);
        yield return new WaitForSeconds(0.5f);
        Mat.SetFloat(name, originalValue);
        yield return null;
    }

    void OnDestroy()
    {
        Player.TakeDamage -= TakeDamage;
    }
}
