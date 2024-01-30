using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraOfMazeToNormal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CinemachineFreeLook freeLookCamera = other.transform.parent.GetComponentInChildren<CinemachineFreeLook>();

        if (freeLookCamera != null)
        {

            freeLookCamera.m_Orbits[0].m_Height = 4.5f;
            freeLookCamera.m_Orbits[1].m_Height = 2.5f;
            freeLookCamera.m_Orbits[2].m_Height = 0.4f;
        }
    }
}
