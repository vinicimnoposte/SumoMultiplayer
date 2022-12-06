using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;


public class PlayerMovAdv : NetworkBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed;
    //public float sprintSpeed;
    //public float slideSpeed;
    //public float wallrunSpeed;
    //public float climbSpeed;
    //public float vaultSpeed;
    public float airMinSpeed;
    public bool dashCD;
    public float dashForce;
    public float dashCoolDown;
    public float dashDuration;
    public GameObject collisionDetect;
    public float knockbackForce;



    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    //public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    //[Header("Crouching")]//a
    //public float crouchSpeed;
    //public float crouchYScale;
    //private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    //public KeyCode sprintKey = KeyCode.LeftShift;
    //public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    //public Climbing climbingScript;
    //private ClimbingDone climbingScriptDone;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    private PlayerMovAdv pma;

    [SerializeField] private float spinSpeed;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public TMP_Text playerNameText;
    [SyncVar]
    public string nomeVar;

    public GameObject canvasChooseName;

      

    [Command(requiresAuthority = false)]
    public void CMD_NameConfirm(PlayerMovAdv playerMovementScript, string nome54)
    {
        RPC_NameConfirm(playerMovementScript, nome54);
        print("dota3");
    }
    [ClientRpc]
    void RPC_NameConfirm(PlayerMovAdv playerMovementScript, string nome54)
    {

        playerMovementScript.playerNameText.text = nome54;
        nomeVar = nome54;
        print("dota4");
    }
    public enum MovementState
    {
        //freeze,
        //unlimited,
        walking,
        //sprinting,
        //wallrunning,
        //climbing,
        //vaulting,
        //crouching,
        //sliding,
       // air
    }



    // public bool sliding;
    // public bool crouching;
    // public bool wallrunning;
    // public bool climbing;
    // public bool vaulting;

    //public bool freeze;
    //public bool unlimited;

    public bool restricted;

    //public TextMeshProUGUI text_speed;
    //public TextMeshProUGUI text_mode;

    private void Start()
    {
        pma = GetComponent<PlayerMovAdv>();
        if(!isLocalPlayer)
        {
            playerNameText.text = nomeVar;
        }

        //climbingScriptDone = GetComponent<ClimbingDone>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        //startYScale = transform.localScale.y;
    }
    public override void OnStartLocalPlayer()
    {
        canvasChooseName.SetActive(true);
        base.OnStartLocalPlayer();

    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        //TextStuff();
        if (Input.GetKeyDown(KeyCode.E) && dashCD == false)
        {
            dashCD = true;
            DashEffect();
            Invoke("DashCooldown", dashCoolDown);
            collisionDetect.SetActive(true);
            Invoke("DashEnd", dashDuration);
        }
        // handle drag
        //if (state == MovementState.walking || state == MovementState.sprinting)
        //    rb.drag = groundDrag;
        //else
        //    rb.drag = 0;
    }

    private void DashEffect()
    {
        rb.AddForce(transform.forward * dashForce);
    }

    public void Knockback(Vector3 KBDirection)
    {
        CMDKnockback(KBDirection);
    }

    [Command]
    public void CMDKnockback(Vector3 KBDirection)
    {
        rb.AddForce(KBDirection * knockbackForce);
    }

    private void DashCooldown()
    {
        dashCD = false;
    }

    private void DashEnd()
    {
        collisionDetect.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        MovePlayer();
    }

    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        //transform.Translate(Vector3.forward * verticalInput * walkSpeed * Time.deltaTime);
        rb.AddForce(orientation.forward * verticalInput * walkSpeed, ForceMode.Impulse);
        transform.Rotate(Vector3.up, horizontalInput * spinSpeed);

        

        // when to jump
        //if (Input.GetKey(jumpKey) && readyToJump && grounded)
        //{
        //    readyToJump = false;
        //
        //    Jump();
        //
        //    Invoke(nameof(ResetJump), jumpCooldown);
        //}

        // start crouch
        //if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
        //{
        //    transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        //    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        //
        //    //crouching = true;
        //}

        // stop crouch
        // if (Input.GetKeyUp(crouchKey))
        // {
        //     transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        //
        //     crouching = false;
        // }
    }

    bool keepMomentum;
    private void StateHandler()
    {
        // Mode - Freeze
        //if (freeze)
        //{
        //    state = MovementState.freeze;
        //    rb.velocity = Vector3.zero;
        //    desiredMoveSpeed = 0f;
        //}
        //
        //// Mode - Unlimited
        //else if (unlimited)
        //{
        //    state = MovementState.unlimited;
        //    desiredMoveSpeed = 999f;
        //}
        //
        //// Mode - Vaulting
        //else if (vaulting)
        //{
        //    state = MovementState.vaulting;
        //    desiredMoveSpeed = vaultSpeed;
        //}

        // Mode - Climbing
        // else if (climbing)
        // {
        //     state = MovementState.climbing;
        //     desiredMoveSpeed = climbSpeed;
        // }

        // Mode - Wallrunning
        //else if (wallrunning)
        //{
        //    state = MovementState.wallrunning;
        //    desiredMoveSpeed = wallrunSpeed;
        //}
        //
        //// Mode - Sliding
        //else if (sliding)
        //{
        //    state = MovementState.sliding;
        //
        //    // increase speed by one every second
        //    if (OnSlope() && rb.velocity.y < 0.1f)
        //    {
        //        desiredMoveSpeed = slideSpeed;
        //        keepMomentum = true;
        //    }
        //
        //    else
        //        desiredMoveSpeed = sprintSpeed;
        //}

        // Mode - Crouching
        //else if (crouching)
        //{
        //    state = MovementState.crouching;
        //    desiredMoveSpeed = crouchSpeed;
        //}

        // Mode - Sprinting
        //if (grounded && Input.GetKey(sprintKey))
        //{
        //    state = MovementState.sprinting;
        //    //desiredMoveSpeed = sprintSpeed;
        //}

        // Mode - Walking
        if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
       // else
       // {
       //     state = MovementState.air;
       //
       //     if (moveSpeed < airMinSpeed)
       //         desiredMoveSpeed = airMinSpeed;
       // }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        // deactivate keepMomentum
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        //if (climbingScript.exitingWall) return;
        //if (climbingScriptDone.exitingWall) return;
        if (restricted) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        //else if (grounded)
        //    rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // turn gravity off while on slope
        //if(!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    //private void TextStuff()
    //{
    //    Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    //
    //    if (OnSlope())
    //        text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1) + " / " + Round(moveSpeed, 1));
    //
    //    else
    //        text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));
    //
    //    text_mode.SetText(state.ToString());
    //}

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
