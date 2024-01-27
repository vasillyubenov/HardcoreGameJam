using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{
    private Vignette vignette;

    private float vignetteTarget = 0.15f;

    private void Start()
    {
        var v = GetComponent<Volume>();
        v.profile.TryGet(out vignette);
    }
    private void FixedUpdate()
    {
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, vignetteTarget, Time.deltaTime * 40f);
    }

    public void VignetteEffect(float duration)
    {
        vignette.intensity.value = vignetteTarget = 0.3f;
        Invoke("CancelVignette", duration);
    }
    public void CancelVignette()
    {
        CancelInvoke("CancelVignette");
        vignetteTarget = 0.15f;
    }
}
