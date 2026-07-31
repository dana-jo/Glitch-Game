using UnityEngine;
public class Draggable : MonoBehaviour 
{
    [SerializeField] float holdDistance = 0.6f;
    [SerializeField] private float maxDragDistance = 0.8f;
    [SerializeField] private float speedReduction = 0.5f; 
    PlayerMovement player;
    private Collider2D playerCollider , objectCollider;
    private Rigidbody2D rb;
    public bool canBeDragged = true;
    public bool IsHeld { get; private set; }
    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        playerCollider = player.GetComponent<Collider2D>();
        objectCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!IsHeld || !CanBeDragged())
            return;

        Vector2 holdPosition =
            (Vector2)player.transform.position +
            player.LastFacingDirection * holdDistance;

        rb.MovePosition(holdPosition);

        float distance = Vector2.Distance(rb.position, holdPosition);
        if (distance > maxDragDistance)
        {
            player.speedMultiplier = 0f; // might cause problems if the player's speed is reduced by many factors, but now should be fine
        }
        else
        {
            player.speedMultiplier = speedReduction; 
        }
    }
    public bool CanBeDragged()
    {
        return canBeDragged;
    }
    public void PickUp()
    {
        if(!CanBeDragged()) 
            return;

        IsHeld = true;
        player.speedMultiplier = speedReduction; 
        Physics2D.IgnoreCollision(playerCollider, objectCollider, true);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
    }
    public void Drop()
    {
        IsHeld = false;
        player.speedMultiplier = 1f;
        Physics2D.IgnoreCollision(playerCollider, objectCollider, false);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezePositionY |
                         RigidbodyConstraints2D.FreezeRotation;
    }
}
