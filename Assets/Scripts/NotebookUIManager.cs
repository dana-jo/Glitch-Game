using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class NotebookUIManager : MonoBehaviour
{
    public static NotebookUIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject notebookPanel;
    [SerializeField] private RectTransform pagesContainer; 

    [Header("Prefabs")]
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject textElementPrefab;
    [SerializeField] private GameObject imageElementPrefab;

    [Header("Navigation Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

   /* [Header("Settings")]
    [SerializeField] private float maxPageHeight = 800f; */

    private List<GameObject> dynamicPages = new List<GameObject>();
    private int currentPageIndex = 0;
    private RectTransform activePage;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Instance created");
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (notebookPanel != null)
        {
            notebookPanel.SetActive(false);
        }
    

      //  CreateNewPage();
    }

    
    public void ToggleNotebook()
    {
        if (notebookPanel == null) return;

        notebookPanel.SetActive(!notebookPanel.activeSelf);

        PauseController.Instance.OpenNotebook(notebookPanel.activeSelf);

        if (notebookPanel.activeSelf)
        {
            UpdateUI();
        }
    }


   
    private void CreateNewPage()
    {
        GameObject newPage = Instantiate(pagePrefab, pagesContainer);
        activePage = newPage.GetComponent<RectTransform>();
        dynamicPages.Add(newPage);
        UpdateUI();
    }


    public void WriteNote(NoteSO note)
    {
        foreach (NoteElement element in note.Elements)
        {
            
            CreateNewPage();

            
            if (element.Type == NoteElementType.Text)
            {
                GameObject textObj = Instantiate(textElementPrefab, activePage);
                textObj.GetComponent<TMP_Text>().text = element.TextContent;
            }
            else if (element.Type == NoteElementType.Image)
            {
                GameObject imgObj = Instantiate(imageElementPrefab, activePage);
                Image img = imgObj.GetComponent<Image>();
                img.sprite = element.ImageContent;
                img.preserveAspect = true; 
            }
        }

       
        UpdateUI();
    }



    public void NextPage()
    {
        if (currentPageIndex < dynamicPages.Count - 1)
        {
            currentPageIndex++;
            UpdateUI();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < dynamicPages.Count; i++)
        {
            dynamicPages[i].SetActive(i == currentPageIndex);
        }

        if (prevButton != null)
            prevButton.interactable = (currentPageIndex > 0);

        if (nextButton != null)
            nextButton.interactable = (currentPageIndex < dynamicPages.Count - 1);
    }
}