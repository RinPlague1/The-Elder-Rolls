using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Settings")]
    public KeyCode switchCharacterKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.E;

    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.isActivePlayer) return;

        // Handle character switching
        if (Input.GetKeyDown(switchCharacterKey))
        {
            GameManager.Instance.CycleActivePartyMember();
        }

        // Add other input handling here
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // Implement interaction logic here
        if (playerController.currentTile != null)
        {
            Debug.Log($"Attempting to interact with tile at {playerController.currentTile.coordinates}");
        }
    }
}