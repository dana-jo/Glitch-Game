using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    //private bool playingFootsteps = false;
    //public float footStepsSpeed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (pauseController.IsGamePaused)
        //{
        //    rb.linearVelocity = Vector2.zero;
        //    animator.SetBool("isWalking", false);

        //    StopFootsteps();
        //    return;
        //}
        rb.linearVelocity = moveInput * speed;
        //animator.SetBool("isWalking", rb.linearVelocity.magnitude > 0);

        // footsteps sound
        //if (rb.linearVelocity.magnitude > 0 && !playingFootsteps)
        //{
        //    StartFootsteps();
        //}
        //else if (playingFootsteps)
        //{
        //    StopFootsteps();
        //}
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
    }

    //void StartFootsteps()
    //{
    //    playingFootsteps = true;
    //    InvokeRepeating(nameof(PlayFootsteps), 0f, footStepsSpeed);
    //}

    //void StopFootsteps()
    //{
    //    playingFootsteps = false;
    //    CancelInvoke(nameof(PlayFootsteps));
    //}

    //void PlayFootsteps()
    //{
    //    if (!SoundEffectManager.IsRandomAudioPlaying())
    //    {
    //        SoundEffectManager.Play("Footstep", true);
    //    }
    //}
}
