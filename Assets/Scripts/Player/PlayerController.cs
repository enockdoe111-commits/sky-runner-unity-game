// PlayerController.cs - Character movement and input handling

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    private bool isGrounded = true;

    private int currentLane = 1; // 0 = left, 1 = center, 2 = right
    private Vector3 targetPosition;
    private bool isSliding = false;
    private float slideTimer = 0f;
    private bool hasShield = false;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    private InputManager inputManager;
    private AudioManager audioManager;
    private ScoreManager scoreManager;

    public bool IsAlive { get; private set; } = true;

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        audioManager = FindObjectOfType<AudioManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        rb.velocity = Vector3.forward * GameConstants.PLAYER_FORWARD_SPEED;
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        HandleInput();
        UpdateAnimation();
        UpdateSliding();
        UpdateInvincibility();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive) return;

        CheckGrounded();
        MaintainForwardMovement();
        MoveLaterally();
    }

    private void HandleInput()
    {
        if (inputManager.GetMoveLeftInput())
        {
            MoveLane(-1);
        }
        else if (inputManager.GetMoveRightInput())
        {
            MoveLane(1);
        }

        if (inputManager.GetJumpInput() && isGrounded && !isSliding)
        {
            Jump();
        }

        if (inputManager.GetSlideInput() && isGrounded && !isSliding)
        {
            StartSlide();
        }
    }

    private void MoveLane(int direction)
    {
        int newLane = Mathf.Clamp(currentLane + direction, 0, GameConstants.NUM_LANES - 1);
        if (newLane == currentLane) return;

        currentLane = newLane;
        float laneOffset = (currentLane - 1) * GameConstants.LANE_WIDTH;
        targetPosition.x = laneOffset;
    }

    private void MoveLaterally()
    {
        float laneOffset = (currentLane - 1) * GameConstants.LANE_WIDTH;
        Vector3 currentPos = rb.position;
        currentPos.x = Mathf.Lerp(currentPos.x, laneOffset, Time.fixedDeltaTime * GameConstants.PLAYER_MOVE_SPEED);
        rb.position = currentPos;
    }

    private void MaintainForwardMovement()
    {
        float speedMultiplier = GameManager.Instance.GetSpeedMultiplier();
        float currentForwardSpeed = GameConstants.PLAYER_FORWARD_SPEED * speedMultiplier;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, currentForwardSpeed);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * GameConstants.PLAYER_JUMP_FORCE, ForceMode.Impulse);
        isGrounded = false;
        audioManager.PlaySFX("jump");
        animator.SetTrigger("Jump");
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = 0f;
        capsuleCollider.height = 1f; // Reduce collider height for sliding
        audioManager.PlaySFX("slide");
        animator.SetBool("IsSliding", true);
    }

    private void UpdateSliding()
    {
        if (!isSliding) return;

        slideTimer += Time.deltaTime;
        if (slideTimer >= GameConstants.PLAYER_SLIDE_DURATION)
        {
            EndSlide();
        }
    }

    private void EndSlide()
    {
        isSliding = false;
        capsuleCollider.height = 2f; // Restore collider height
        animator.SetBool("IsSliding", false);
    }

    private void UpdateInvincibility()
    {
        if (!isInvincible) return;

        invincibilityTimer -= Time.deltaTime;
        if (invincibilityTimer <= 0f)
        {
            isInvincible = false;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!GameManager.Instance.IsGameActive) return;

        if (collision.CompareTag("Obstacle"))
        {
            HandleObstacleCollision();
        }
        else if (collision.CompareTag("Coin"))
        {
            Coin coin = collision.GetComponent<Coin>();
            if (coin != null)
                coin.Collect();
        }
        else if (collision.CompareTag("PowerUp"))
        {
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            if (powerUp != null)
                powerUp.Activate(this);
        }
    }

    private void HandleObstacleCollision()
    {
        if (isInvincible) return;

        if (hasShield)
        {
            hasShield = false;
            audioManager.PlaySFX("shieldBreak");
            return;
        }

        IsAlive = false;
        audioManager.PlaySFX("collision");
        animator.SetTrigger("Die");
    }

    public void ActivateShield()
    {
        hasShield = true;
        audioManager.PlaySFX("powerUp");
    }

    public void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = GameConstants.INVINCIBILITY_DURATION;
        audioManager.PlaySFX("powerUp");
    }

    public bool HasShield => hasShield;
    public bool IsInvincible => isInvincible;
    public int CurrentLane => currentLane;
}
