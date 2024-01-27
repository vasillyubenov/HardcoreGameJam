using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    [Header("Main Properties")]
    [SerializeField] private List<Transform> targets;

    [SerializeField] private Vector3 offset = new Vector3(0, 2, -15f);

    [SerializeField] private float dampening = 0.3f;

    [SerializeField] private Vector3 dampeningVelocity;

    [SerializeField] private float minZoom = 20f, maxZoom = 80f, zoomLimit = 15f;

    [Header("Effects")]
    [Range(1, 2)]
    [SerializeField] private float shakeAttenuation = 1.025f;

    [SerializeField] private float shakeSpeed = 8000;

    private Vector3 shakeOffset, shakeInertia;

    public void ShakeCamera(Vector2 forceVector)
    {
        shakeOffset = new Vector3(forceVector.x, forceVector.y, 0);
    }
    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        MoveCamera();
        ZoomCamera();
        ShakeAttenuation();
    }
    private void MoveCamera()
    {
        Vector3 centerPoint = FocusCenter();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition + shakeOffset, ref dampeningVelocity, dampening) + shakeOffset;
    }
    private void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetWidth() / zoomLimit);

        Camera cam = GetComponent<Camera>();
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    private void ShakeAttenuation()
    {
        Vector3 shakeVector = -shakeOffset / shakeAttenuation;

        shakeOffset = Vector3.Lerp(shakeInertia, shakeVector, Time.deltaTime * shakeSpeed);
    }
    private float GetWidth()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }
    private Vector3 FocusCenter()
    {
        if (targets.Count == 1) return targets[0].position;
        else
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.center;
        }
    }
}
