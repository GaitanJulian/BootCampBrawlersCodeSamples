using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages players in the game.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    // Singleton instance of the PlayerManager
    public static PlayerManager Instance { get; private set; }

    // List of every player in the game
    public List<PlayerInput> players = new List<PlayerInput>();

    // Dictionary to keep track of each player's ready status
    private Dictionary<PlayerInput, bool> playerReadyStatus = new Dictionary<PlayerInput, bool>();

    // Spawn points for each player
    [SerializeField]
    private List<Transform> startingPoints;

    // Layer masks for each player
    [SerializeField]
    private List<LayerMask> playerLayers;

    // Reference to the PlayerInputManager
    private PlayerInputManager playerInputManager;

    // Delegate for player added event
    public delegate void PlayerAddedHandler(PlayerInput player);
    public event PlayerAddedHandler OnPlayerAdded;

    // Delegate for all players ready event
    public delegate void AllPlayersReady();
    public event AllPlayersReady OnPlayersReady;

    /// <summary>
    /// Initializes the PlayerManager.
    /// </summary>
    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Subscribes to the player joined event when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    /// <summary>
    /// Unsubscribes from the player joined event when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    /// <summary>
    /// Adds a player to the game and sets their spawn point.
    /// </summary>
    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);        

        // Need to use the parent due to the structure of the prefab
        Transform playerParent = player.transform.parent;

        playerParent.position = startingPoints[player.playerIndex].position;

        playerParent.SetParent(startingPoints[player.playerIndex]);

        playerParent.localRotation = Quaternion.Euler(0, 0, 0);

        // Add the player to the dictionary with a default ready status of false
        playerReadyStatus.Add(player, false);

        // Invoke the event
        OnPlayerAdded?.Invoke(player);
    }

    /// <summary>
    /// Sets the positions of all players based on the starting points.
    /// </summary>
    public void SetPlayersPositions(float yrotation)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Get the player's parent transform
            Transform playerParent = players[i].transform.parent;

            // Set the player's position to the corresponding starting point
            playerParent.position = startingPoints[i].position;

            // Set the player's parent to the corresponding starting point
            playerParent.SetParent(startingPoints[i]);

            // Set the player's rotation
            playerParent.localRotation = Quaternion.Euler(0, yrotation, 0);
        }
    }

    /// <summary>
    /// Toggles the ready status of a player.
    /// </summary>
    public void ChangePlayerReadyStatus(PlayerInput player)
    {
        // Update the player's ready status in the dictionary
        playerReadyStatus[player] = !playerReadyStatus[player];
    }

    /// <summary>
    /// Removes a player from the game.
    /// </summary>
    public void RemovePlayer(PlayerInput player)
    {
        // Remove the player from the players list
        players.Remove(player);

        // Remove the player's ready status from the dictionary
        playerReadyStatus.Remove(player);

        // Destroy the player's game object
        Destroy(player.gameObject.transform.parent.gameObject);
    }

    public void RemoveAllPlayers()
    {
        PlayerInput[] currentPlayers = GetComponentsInChildren<PlayerInput>();

        foreach (PlayerInput player in currentPlayers) 
        {
            RemovePlayer(player);
        }
    }

    /// <summary>
    /// Sets the camera layer for a player.
    /// </summary>
    private void SetCameraLayer(PlayerInput player)
    {
        // Find the index of the player in the list
        int playerIndex = players.IndexOf(player);

        // Convert layer mask (bit) to an integer 
        int layerToAdd = (int)Mathf.Log(playerLayers[playerIndex].value, 2);

        Transform playerTransform = player.transform;

        player.camera = playerTransform.GetComponentInChildren<Camera>();

        // TODO MAKE THE CAMERA WORK FOR MORE MINIGAMES
        playerTransform.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        // Set the layer
        playerTransform.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        // Add the layer
        playerTransform.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;

        SetupPlayerCamera(player);
    }

    /// <summary>
    /// Sets the camera layers for all players.
    /// </summary>
    public void SetCameraLayers()
    {
        foreach(PlayerInput player in players)
        {
            if(player!= null)
            {
                SetCameraLayer(player);
            }  
        }
    }

    private void SetupPlayerCamera(PlayerInput playerInput)
    {
        int playerCount =players.Count;

        switch (playerCount)
        {
            case 1:
                playerInput.camera.rect = new Rect(0, 0, 1, 1); // Full screen
                break;
            case 2:
                playerInput.camera.rect = new Rect(playerInput.playerIndex * 0.5f, 0, 0.5f, 1); // Vertical split
                break;
            case 3:
                if (playerInput.playerIndex == 0)
                {
                    playerInput.camera.rect = new Rect(0, 0.5f, 1, 0.5f); // Top half of the screen for player 1
                }
                else
                {
                    playerInput.camera.rect = new Rect((playerInput.playerIndex - 1) * 0.5f, 0, 0.5f, 0.5f); // Bottom half split into two for players 2 and 3
                }
                break;
            case 4:
                playerInput.camera.rect = new Rect((playerInput.playerIndex % 2) * 0.5f, (playerInput.playerIndex / 2) * 0.5f, 0.5f, 0.5f); // Quadrants
                break;
        }
    }

    /// <summary>
    /// Gets the ready status of a player.
    /// </summary>
    public bool GetPlayerReadyStatus(PlayerInput player)
    {
        // Get the player's ready status from the dictionary
        return playerReadyStatus[player];
    }

    /// <summary>
    /// Checks if all players are ready.
    /// </summary>
    public bool AreAllPlayersReady()
    {
        // Check if there are at least two players
        // FOR NOW ITS GOING TO BE DISABLED
         /*
        if (playerReadyStatus.Count < 2)
        {
            return false;
        }
        */
        // Check if all players are ready
        foreach (var isReady in playerReadyStatus.Values)
        {
            if (!isReady)
            {
                // If any player is not ready, return false
                return false;
            }
        }

        // If all players are ready, return true
        OnPlayersReady?.Invoke();
        return true;
    }

    /// <summary>
    /// Prepares players for a scene change.
    /// </summary>
    public void PreparePlayersForSceneChange()
    {
        foreach (PlayerInput player in players)
        {
            player.transform.parent.SetParent(transform);
            player.transform.parent.localPosition = Vector3.zero;
            player.transform.GetComponentInChildren<CinemachineFreeLook>().enabled = false;
        }
    }

    /// <summary>
    /// Updates the starting points for players.
    /// </summary>
    public void UpdateStartingPoints(List<Transform> newStartingPoints)
    {
        startingPoints = newStartingPoints;
    }
}