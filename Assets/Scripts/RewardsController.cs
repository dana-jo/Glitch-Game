using UnityEngine;

public class RewardsController : MonoBehaviour
{
    public static RewardsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GiveQuestRewards(int questID)
    {
        Quest quest = QuestDictionary.Instance.GetQuest(questID);

        if (quest?.rewards == null)
            return;

        foreach (Reward reward in quest.rewards)
        {
            switch (reward.rewardType)
            {
                case RewardType.Item:
                    GiveItemReward(reward.itemID, reward.amount);
                    break;

                case RewardType.Cutscene:
                    SceneController.Instance.PlayCutscene(reward.sceneName);
                    break;

                case RewardType.Quest:
                    QuestController.Instance.AcceptQuest(reward.questID);
                    break;

                case RewardType.Dialogue:
                    if (NPCDicionary.Instance.GetNPC(reward.npcID) == null)
                        Debug.Log($"NPC id is wrong in quest {questID}");
                    else
                        NPCDicionary.Instance.GetNPC(reward.npcID).GetComponent<DialogueManager>().ChangeDialogue(reward.dialogue);
                    break;

                case RewardType.Note:
                    if(NoteDictionary.Instance.GetNoteByID(reward.noteID) != null)
                        NotebookController.Instance.AddNote(reward.noteID);
                    break;

                case RewardType.Map:
                    // ------------------------- for later
                    break;
            }
        }
    }
    private void GiveItemReward(int itemID, int amount)
    {
        var itemPrefab = ItemDictionary.Instance.GetItemPrefab(itemID);

        if (itemPrefab == null)
            return;

        for (int i = 0; i < amount; i++)
        {
            if (!InventoryController.Instance.AddItem(itemPrefab))
            {
                GameObject dropItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
                dropItem.GetComponent<BounceEffect>().StartBounce();
            }
            else
            {
                itemPrefab.GetComponent<Item>().ShowPopup();
            }
        }
    }
}
