

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // different states for the player
    public enum PlayerState
    {
        Idle,
        Moving,
        Hurt
    }

    public PlayerState CurrentState { get; private set; }

    // timer for the hurt state
    private float hurtTimer;

    private void Start()
    {
        // gets the player's rigidbody
        rb = GetComponent<Rigidbody2D>();

        // gets the player's sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // starts the player in the idle state
        ChangeState(PlayerState.Idle);

        // subscribes to the lives changed event
        GameManager.Instance.LivesChanged += OnLivesChanged;
    }

    private void OnDestroy()
    {
        // unsubscribes from the lives changed event
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LivesChanged -= OnLivesChanged;
        }
    }

    private void Update()
    {
        // stops movement when paused and game is lost
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        HandleMovement();

        // counts down the hurt state timer
        if (CurrentState == PlayerState.Hurt)
        {
            hurtTimer -= Time.deltaTime;

            if (hurtTimer <= 0f)
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    private void HandleMovement()
    {
        // reads the inputs for ad or left and right arrows
        float horizontal = Input.GetAxisRaw("Horizontal");

        // sets horizontal speed to a value and sets vertical speed to 0
        rb.linearVelocity = new Vector2(moveSpeed * horizontal, 0f);

        // changes the players state when moving
        if (CurrentState != PlayerState.Hurt)
        {
            if (horizontal == 0f)
            {
                ChangeState(PlayerState.Idle);
            }
            else
            {
                ChangeState(PlayerState.Moving);
            }
        }
    }

    public void ChangeState(PlayerState newState)
    {
        // changes the players state
        CurrentState = newState;

        // changes the players color based on its state
        switch (CurrentState)
        {
            case PlayerState.Idle:
                spriteRenderer.color = Color.white;
                break;

            case PlayerState.Moving:
                spriteRenderer.color = Color.green;
                break;

            case PlayerState.Hurt:
                spriteRenderer.color = Color.red;
                hurtTimer = 0.5f;
                break;
        }
    }

    private void OnLivesChanged(int newLives)
    {
        // changes to red when a life is lost
        if (newLives > 0)
        {
            ChangeState(PlayerState.Hurt);
        }
    }
}
