using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI MainQuest;
    public TextMeshProUGUI QuestBox1;
    public TextMeshProUGUI QuestBox2;
    public TextMeshProUGUI QuestBox3;

    [Header("Defaults")]
    public string[] NoQuest = { "None", "No Quest", "Insert Quest Here"};

    private void Start()
    {
        UpdateQuestTracking(QuestBox1);
        UpdateQuestTracking(QuestBox2);
        UpdateQuestTracking(QuestBox3);
    }

    public void UpdateQuestTracking(TextMeshProUGUI QuestBox, string QuestDescription = null)
    {
        QuestBox.text = (QuestDescription != null) ? QuestDescription: NoQuest[Random.Range(0,NoQuest.Length - 1)];
    }
}
