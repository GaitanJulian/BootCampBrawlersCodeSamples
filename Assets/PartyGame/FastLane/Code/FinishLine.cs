using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishLine : MonoBehaviour
{
    private FastLaneManager fastLaneManager;

    private PlayerInput index;

    private void Awake()
    {
        fastLaneManager = GameObject.Find("FastLaneManager").GetComponent<FastLaneManager>();

        
    }

    private void OnTriggerExit(Collider other)
    {
        index = other.GetComponentInChildren<PlayerInput>();

        if (fastLaneManager != null && index != null)
        {
            int playerIndex = index.playerIndex;
            int[] actualCheckPointPlayer = fastLaneManager.actualCheckPointPlayer;

            if (playerIndex >= 0 && playerIndex < actualCheckPointPlayer.Length)
            {
                int checkPoint = actualCheckPointPlayer[playerIndex];

                if (checkPoint == 2)
                {
                    fastLaneManager.LapUpdate(playerIndex);
                }
            }
        }

    }
}
