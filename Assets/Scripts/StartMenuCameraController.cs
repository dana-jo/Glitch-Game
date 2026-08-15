using System.Collections;
using UnityEngine;

public class StartMenuCameraController : MonoBehaviour
{
    public Transform menuPosition;
    public float menuCameraSize = 2f;
    public float duration = 1f;

    [Header("UI")]
    public GameObject menuUI;
    public GameObject drawing;

    [Header("Glitching")]
    public float waitStart = 5;
    public float waitEnd = 10, glitchStart = 1, glitchEnd = 3;

    private Animator animator; // 0: normal    0.5: glitch     0.75: end glitch    1: in menu

    private Camera cam;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalCameraSize;

    private bool isInMenu = false;
    private bool isMoving = false;

    private Coroutine glitchCoroutine;
    private Coroutine currentGlitch;



    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        cam = GetComponent<Camera>();
        originalCameraSize = cam.orthographicSize;

        menuUI.SetActive(false);

        animator = drawing.GetComponent<Animator>();

        GetComponent<ShakeEffect>().StartShake();

        animator.SetFloat("state", 0); // normal

        glitchCoroutine = StartCoroutine(GlitchLoop());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && !isInMenu)
        {

            if (glitchCoroutine != null)
            {
                StopCoroutine(glitchCoroutine);
                glitchCoroutine = null;
            }

            if (currentGlitch != null)
            {
                StopCoroutine(currentGlitch);
                currentGlitch = null;
            }

            animator.SetFloat("state", 0);

            EnterMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isMoving && isInMenu)
        {
            ExitMenu();
        }
    }

    public void EnterMenu()
    {
        GetComponent<ShakeEffect>().StopShake();

        Vector3 targetPosition = menuPosition.position;
        targetPosition.z = originalPosition.z;

        StartCoroutine(MoveCamera(
            targetPosition,
            menuPosition.rotation,
            menuCameraSize,
            true
        ));
    }

    public void ExitMenu()
    {
        GetComponent<ShakeEffect>().StopShake();

        menuUI.SetActive(false);

        StartCoroutine(MoveCamera(
        originalPosition,
        originalRotation,
        originalCameraSize,
        false
        ));
    }

    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation, float targetCameraSize, bool enteringMenu)
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float startCameraSize = cam.orthographicSize;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                t
            );

            cam.orthographicSize = Mathf.Lerp(
                startCameraSize,
                targetCameraSize,
                t
            );

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        cam.orthographicSize = targetCameraSize;

        isMoving = false;
        isInMenu = enteringMenu;

        if (!enteringMenu)
        {
            animator.SetFloat("state", 0); // normal
            GetComponent<ShakeEffect>().StartShake();

            glitchCoroutine = StartCoroutine(GlitchLoop());
        }
        else
        {
            animator.SetFloat("state", 1); // in menu
        }

            yield return new WaitForSeconds(0.7f);

        if(!isMoving)
            menuUI.SetActive( isInMenu );

    }

    private IEnumerator GlitchLoop()
    {
        while (!isInMenu)
        {
            float waitTime = Random.Range(waitStart, waitEnd);
            Debug.Log("waiting " + waitTime);

            yield return new WaitForSeconds(waitTime);

            if (isInMenu)
                yield break;

            currentGlitch = StartCoroutine(StartGlitch());
            yield return currentGlitch;
            currentGlitch = null;
        }

        glitchCoroutine = null;
    }
    private IEnumerator StartGlitch()
    {
        float randomDuration = Random.Range(glitchStart, glitchEnd);

        Debug.Log("glitching " +  randomDuration);

        animator.SetFloat("state", 0.5f); // glitch

        yield return new WaitForSeconds(randomDuration);

        yield return StartCoroutine(EndGlitch());
    }

    private IEnumerator EndGlitch()
    {
        animator.SetFloat("state", 0.75f); // glitch end

        yield return new WaitForSeconds(0.5f); // glitch end animation duration

        animator.SetFloat("state", 0f); // normal
    }
}