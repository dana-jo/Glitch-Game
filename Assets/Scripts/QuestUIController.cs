using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIController : MonoBehaviour
{
    public GameObject questsPage;
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;
    public GameObject noQuests;


    // this is just for testing
    // -----------
    //public int testQuestAmount = 10;
    //private List<QuestProgress> testQuests = new();
    // -----------

    void Start()
    {
        // -----------
        //for (int i = 0; i < testQuestAmount; i++)
        //{
        //    testQuests.Add(new QuestProgress(0));
        //}
        // -----------
        UpdateQuestUI();
    }

    public void ToggleQuestsPage()
    {
        if (questsPage == null) return;

        questsPage.SetActive(!questsPage.activeSelf);

        PauseController.Instance.OpenQuests(questsPage.activeSelf);
    }
    public void UpdateQuestUI()
    {
        Debug.Log("Update Quest UI");

        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        bool hasQuests = QuestController.Instance.activeQuests.Count > 0;
        noQuests.SetActive(!hasQuests);
        if (!hasQuests)
            return;

        // build quest entries
        // -----------
        //foreach (var quest in testQuests)
        // -----------
        foreach (var quest in QuestController.Instance.activeQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestName").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectivesList");

            questNameText.text = quest.quest.questName;

            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = "";
                if (objective.type == ObjectiveType.CollectItem)
                    objText.text += $"({objective.currentAmount} / {objective.requiredAmount}) ";
                objText.text += $"{objective.description}";
            }
        }
    }
}
