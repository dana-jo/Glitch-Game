using UnityEngine;
using UnityEngine.UI;

public class NotebookUIManager : MonoBehaviour
{

    public static NotebookUIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject notebookPanel;
    [SerializeField] private GameObject[] pages;

    [Header("Navigation Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private int currentPageIndex = 0;

    void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(gameObject);



        notebookPanel.SetActive(false);
    }



    public void ToggleNotebook()
    {
        notebookPanel.SetActive(!notebookPanel.activeSelf);

        if (notebookPanel.activeSelf)
        {
            currentPageIndex = 0;
            UpdateNotebookUI();
        }
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            UpdateNotebookUI();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateNotebookUI();
        }
    }

    private void UpdateNotebookUI()
    {
        for (int i = 0; i < pages.Length; i++)
        {

            pages[i].SetActive(i == currentPageIndex);
        }

        prevButton.interactable = (currentPageIndex > 0);
        nextButton.interactable = (currentPageIndex < pages.Length - 1);
    }
}