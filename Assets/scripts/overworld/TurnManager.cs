using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public PlayerController player;
    public Button endTurnButton;

    private void Awake()
    {
        Instance = this;
        endTurnButton.onClick.AddListener(EndTurn);
    }

    public void StartTurn()
    {
        player.ResetMoves();
        // Other turn start logic
    }

    public void EndTurn()
    {
        // Enemy turns or other end-of-turn logic
        StartCoroutine(StartNextTurn());
    }

    IEnumerator StartNextTurn()
    {
        yield return new WaitForSeconds(1f); // Delay for enemy turns
        StartTurn();
    }
}