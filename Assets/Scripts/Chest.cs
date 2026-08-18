using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour, Interactable
{
    public bool IsOpened { get; private set; }
    public bool IsUnlocked { get; private set; }

    public string ChestID { get; private set; }

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private float itemDropDelay = 0.7f;

    private void Start()
    {
        ChestID ??= GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return IsUnlocked && !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        OpenChest();
    }

    public void Unlock()
    {
        if (IsUnlocked)
            return;

        IsUnlocked = true;

        Debug.Log("Chest unlocked!");
    }

    private void OpenChest()
    {
        SetOpened(true);

        //SoundEffectManager.Play("chest");

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        if (itemPrefab)
        {
            StartCoroutine(DropItemAfterDelay());
        }
    }

    public void OpenFromPuzzle()
    {
        if (IsOpened)
            return;

        OpenChest();
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
    }

    private IEnumerator DropItemAfterDelay()
    {
        yield return new WaitForSeconds(itemDropDelay);

        GameObject droppedItem = Instantiate(
            itemPrefab,
            transform.position + Vector3.down,
            Quaternion.identity
        );

        BounceEffect bounce = droppedItem.GetComponent<BounceEffect>();

        if (bounce != null)
        {
            bounce.StartBounce();
        }
    }

    public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; //chest_1_3
    }

}