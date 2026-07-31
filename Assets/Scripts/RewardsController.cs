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

        foreach (var reward in quest.rewards)
        {
            switch (reward.rewardType)
            {
                case RewardType.Item:
                    GiveItemReward(reward.ID, reward.amount);
                    break;
                case RewardType.Cutscene:
                    SceneController.Instance.PlayCutscene(reward.sceneName);
                    break;
                case RewardType.Quest:
                    QuestController.Instance.AcceptQuest(reward.ID);
                    break;
                case RewardType.Note:
                    // ------------------------- nouraaaaa
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
