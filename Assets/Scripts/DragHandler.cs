using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform target;
    [SerializeField] DragNDropPuzzle puzzleObj;

    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private bool isAttached = false ;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isAttached)
        {
            return;
        }

        GetComponent<ShakeEffect>().StopShake();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;// mini transition during drag 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAttached)
        {
            return;
        }

        //GetComponent<ShakeEffect>().StopShake();
        transform.position = eventData.position;  // follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAttached)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (RectTransformUtility.RectangleContainsScreenPoint(target, eventData.position))
        {
            transform.SetParent(target, false); // the "false" keep the object original scale
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            isAttached = true;

            // update the puzzle state
            puzzleObj.UpdatePuzzleState();
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            GetComponent<ShakeEffect>().StartShake();
        }
    }

    public void SetAttatched()
    {
        transform.SetParent(target, false);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        isAttached = true;
    }
}
