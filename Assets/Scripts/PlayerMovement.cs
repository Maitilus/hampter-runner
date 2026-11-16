using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Lane Movement")]
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 15f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float gravity = -30f;

    [Header("Crouch Settings")]
    public float crouchDuration = 0.65f;
    public float crouchHeightMultiplier = 0.45f;
    public float squashSpeed = 20f;
    [Tooltip("Optional: assign your child 3D mesh here so only the visual squashes while the collider resizes cleanly.")]
    public Transform visualModel;

    private int currentLane = 1;
    private Vector2 touchStartPos;
    private readonly float swipeThreshold = 50f;

    // Jump & Ground state
    private float groundY;
    private float verticalVelocity;
    private bool isGrounded = true;

    // Crouch & Collider state
    private float crouchTimer;
    private bool isCrouching;
    private Vector3 normalVisualScale;
    private Vector3 crouchVisualScale;

    private CapsuleCollider capsuleCol;
    private BoxCollider boxCol;
    private float normalColliderHeight;
    private Vector3 normalColliderCenter;

    void Start()
    {
        // Locks to whatever Y you set in Edit Mode before pressing Play
        groundY = transform.position.y;

        if (visualModel == null) visualModel = transform;
        normalVisualScale = visualModel.localScale;
        crouchVisualScale = new Vector3(normalVisualScale.x, normalVisualScale.y * crouchHeightMultiplier, normalVisualScale.z);

        // Cache original collider dimensions so we can shrink it strictly downward
        capsuleCol = GetComponent<CapsuleCollider>();
        boxCol = GetComponent<BoxCollider>();

        if (capsuleCol != null)
        {
            normalColliderHeight = capsuleCol.height;
            normalColliderCenter = capsuleCol.center;
        }
        else if (boxCol != null)
        {
            normalColliderHeight = boxCol.size.y;
            normalColliderCenter = boxCol.center;
        }
    }

    void Update()
    {
        HandleInput();
        UpdateJump();
        UpdateCrouch();

        // Only move X horizontally; Y is handled strictly by Jump
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 targetPosition = new(targetX, transform.position.y, 0f);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float xDiff = touch.position.x - touchStartPos.x;
                float yDiff = touch.position.y - touchStartPos.y;

                if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff) && Mathf.Abs(xDiff) > swipeThreshold)
                {
                    if (xDiff > 0) currentLane = Mathf.Min(2, currentLane + 1);
                    else currentLane = Mathf.Max(0, currentLane - 1);
                }
                else if (Mathf.Abs(yDiff) > Mathf.Abs(xDiff) && Mathf.Abs(yDiff) > swipeThreshold)
                {
                    if (yDiff > 0) Jump();
                    else StartCrouch();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) currentLane = Mathf.Min(2, currentLane + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) currentLane = Mathf.Max(0, currentLane - 1);
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) StartCrouch();
    }

    private void Jump()
    {
        if (!isGrounded) return;

        isCrouching = false;
        crouchTimer = 0f;
        SetColliderCrouched(false);

        isGrounded = false;
        verticalVelocity = jumpForce;
    }

    private void UpdateJump()
    {
        if (isGrounded) return;

        verticalVelocity += gravity * Time.deltaTime;
        float newY = transform.position.y + verticalVelocity * Time.deltaTime;

        if (newY <= groundY)
        {
            newY = groundY;
            verticalVelocity = 0f;
            isGrounded = true;
        }

        transform.position = new Vector3(transform.position.x, newY, 0f);
    }

    private void StartCrouch()
    {
        if (!isGrounded)
        {
            verticalVelocity = gravity;
        }

        isCrouching = true;
        crouchTimer = crouchDuration;
        SetColliderCrouched(true);
    }

    private void UpdateCrouch()
    {
        if (isCrouching)
        {
            crouchTimer -= Time.deltaTime;
            if (crouchTimer <= 0f)
            {
                isCrouching = false;
                SetColliderCrouched(false);
            }
        }

        Vector3 targetScale = isCrouching ? crouchVisualScale : normalVisualScale;
        visualModel.localScale = Vector3.Lerp(visualModel.localScale, targetScale, squashSpeed * Time.deltaTime);
    }

    private void SetColliderCrouched(bool crouched)
    {
        float targetHeight = crouched ? normalColliderHeight * crouchHeightMultiplier : normalColliderHeight;
        // Shift center downward by half the lost height so the bottom edge stays planted on the floor
        float centerShift = (normalColliderHeight - targetHeight) * 0.5f;
        Vector3 targetCenter = normalColliderCenter - new Vector3(0f, centerShift, 0f);

        if (capsuleCol != null)
        {
            capsuleCol.height = targetHeight;
            capsuleCol.center = targetCenter;
        }
        else if (boxCol != null)
        {
            Vector3 size = boxCol.size;
            size.y = targetHeight;
            boxCol.size = size;
            boxCol.center = targetCenter;
        }
    }
}