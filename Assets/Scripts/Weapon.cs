using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    public float SwordVelocity;
    Vector3 lastdirectionRef;
    public Vector3 direct;
    public bool canSlice = false;
    public GameObject slicePanel;
 
    public void Update()
    {
        direct = transform.position - lastdirectionRef;
        lastdirectionRef = transform.position;
        SwordVelocity = direct.magnitude / Time.deltaTime;

        direct = direct.normalized;
        if (Vector3.Angle(direct, transform.forward) <= 60)
        {
            canSlice = true;
        }
        else
        {
            canSlice = false;
        }
    }
}

