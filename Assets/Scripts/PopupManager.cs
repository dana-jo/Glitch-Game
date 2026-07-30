using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private PopupView popupPrefab;
    [SerializeField] private Transform popupContainer;
    [SerializeField] private float popupDuration = 2f;
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private Sprite questIcon;
    [SerializeField] private Sprite notebookIcon;

    private readonly Queue<PopupRequest> popupQueue = new();

    private bool isProcessingQueue;

    private PopupRequest currentRequest;
    private PopupView currentPopup;

    private class PopupRequest
    {
        public string title;
        public string description;
        public Sprite icon;
        public string combineKey;
        public int amount;

        public PopupRequest(
            string title,
            string description,
            Sprite icon,
            string combineKey = null,
            int amount = 1)
        {
            this.title = title;
            this.description = description;
            this.icon = icon;
            this.combineKey = combineKey;
            this.amount = amount;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void ShowPopup(
        string title,
        string description = "",
        Sprite icon = null)
    {
        PopupRequest request = new PopupRequest(
            title,
            description,
            icon
        );

        EnqueuePopup(request);
    }

    public void ShowItemPopup(//here same ids combine into 1 popup with Xnumber
        int itemId,
        string itemName,
        Sprite itemIcon = null,
        int amount = 1)
    {
        amount = Mathf.Max(1, amount);

        string itemKey = $"item:{itemId}";

        if (TryCombineItemPopup(itemKey, amount))
        {
            return;
        }

        PopupRequest request = new PopupRequest(
            itemName,
            "Added to inventory",
            itemIcon,
            itemKey,
            amount
        );

        EnqueuePopup(request);
    }

    public void ShowQuestPopup(
        string questName,
        string questDescription)
    {
        ShowPopup(
            "New Quest: " + questName,
            questDescription,
            questIcon
        );
    }

    public void ShowNotebookPopup(
        string entryName,
        string entryDescription)
    {
        ShowPopup(
            "New Entry: " + entryName,
            entryDescription,
            notebookIcon
        );
    }


    private bool TryCombineItemPopup(
        string itemKey,
        int addedAmount)
    {

        if (currentRequest != null &&
            currentPopup != null &&
            currentPopup.CanCombine &&
            currentRequest.combineKey == itemKey)
        {
            currentRequest.amount += addedAmount;

            currentPopup.UpdateAmount(
                currentRequest.amount
            );

            currentPopup.RefreshDuration(
                popupDuration
            );

            return true;
        }

        //check popups waiting in the queue if there are repeated items to combine
        foreach (PopupRequest queuedRequest in popupQueue)
        {
            if (queuedRequest.combineKey == itemKey)
            {
                queuedRequest.amount += addedAmount;
                return true;
            }
        }

        return false;
    }

    private void EnqueuePopup(PopupRequest request)
    {
        popupQueue.Enqueue(request);

        if (!isProcessingQueue)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isProcessingQueue = true;

        while (popupQueue.Count > 0)
        {
            currentRequest = popupQueue.Dequeue();

            currentPopup = Instantiate(
                popupPrefab,
                popupContainer
            );

            currentPopup.Setup(
                currentRequest.title,
                currentRequest.description,
                currentRequest.icon,
                currentRequest.amount
            );

            yield return currentPopup.Play(
                popupDuration,
                fadeDuration
            );

            Destroy(currentPopup.gameObject);

            currentPopup = null;
            currentRequest = null;
        }

        isProcessingQueue = false;

    }
}