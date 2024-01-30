using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraToMazeCamera : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CinemachineFreeLook freeLookCamera = other.transform.parent.GetComponentInChildren<CinemachineFreeLook>();


        if (freeLookCamera != null)
        {

            freeLookCamera.m_Orbits[0].m_Height = 10f;
            freeLookCamera.m_Orbits[1].m_Height = 10f;
            freeLookCamera.m_Orbits[2].m_Height = 10f;
        }
    }
}
