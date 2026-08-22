using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    public float speedMultiplier = 1f;
    private float speed => baseSpeed * speedMultiplier;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    public Vector2 LastFacingDirection { get; private set; } = Vector2.down;

    private bool playingFootsteps = false;
    public float footStepsSpeed = 0.5f;

    public bool isSceneDone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (pauseController.IsGamePaused)
        // {
        //    rb.linearVelocity = Vector2.zero;
        //    animator.SetBool("isWalking", false);

        //    StopFootsteps();
        //    return;
        // }
        rb.linearVelocity = moveInput * speed;
        animator.SetBool("isWalking", rb.linearVelocity.magnitude > 0);

        if (rb.linearVelocity.magnitude > 0 && !playingFootsteps)
        {
           StartFootsteps();
        }
        else if (rb.linearVelocity.magnitude==0)
        {
           StopFootsteps();
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("lastInputX", moveInput.x);
            animator.SetFloat("lastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("inputX", moveInput.x);
        animator.SetFloat("inputY", moveInput.y);

        if(context.performed)
        {
            if (moveInput != Vector2.zero)
            {
                LastFacingDirection = moveInput.normalized;
            }
        }
    }

    void StartFootsteps()
    {
       playingFootsteps = true;
       InvokeRepeating(nameof(PlayFootsteps), 0f, footStepsSpeed);
    }

    void StopFootsteps()
    {
       playingFootsteps = false;
       CancelInvoke(nameof(PlayFootsteps));
    }

    void PlayFootsteps()
    {
           SoundEffectManager.play("FootStep",true);
    }
}
