using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Referencia a la cámara VR
    public float distance;
    public float yOffset;

    void Update()
    {
        Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distance;
        newPosition.y -= yOffset; // Reduce la altura en el eje Y
        transform.position = newPosition;
        transform.rotation = cameraTransform.rotation;
    }
}
