using System.Collections;
using UnityEngine;
 
public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    public new Camera camera;
    public float fov = 40f;
    public float sprintFovMultiplier = 1.25f;
    public float slideFovMultiplier = 1.375f;
    [Range(0, 1)]
    public float cameraLerpTime = 0.5f;

    [Header("Movement")]
    public CharacterController playerController;
    private float currentSpeed;
    public float walkSpeed = 10f;
    public float sprintSpeedMultiplier = 1.5f;
    public float crouchSpeedMultiplier = 0.5f;
    public float slideForce = 5f;
    public float slideCounterForce = 0.01f;
    public float slideAirCounterForce = 0.005f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float ledgeJumpHeight = 1f;
    [Range(0, 1)]
    public float scaleLerpTime = 0.5f;

    [Header("Others")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float rayDistance = 0.5f;
    public float rayLength = 5f;
    public Vector3 rayOffset = new Vector3(0f, 1f, 0f);
    public Transform gunContainer;

    private bool isGrounded;
    private bool isJumping;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isSprinting;
    private bool justSprinted, stoppedSprinting;
    private bool isCrouching, justCrouched, stoppedCrouching;
    private bool isSliding;
    private bool hasJumped;
    private bool hasLedged;
    private float x, z;

    Vector3 velocity;
    Vector3 move;
    private Vector3 walkScale = new Vector3(1, 1.5f, 1);
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 walkTpPosition;
    private Vector3 crouchTpPosition;

    private bool isChangingFov;
    private bool isChangingScale;
    private Coroutine changeFov;
    private Coroutine changeScale;

    private void Start()
    {
        transform.localScale = walkScale;
        currentSpeed = walkSpeed;
        camera.fieldOfView = fov;
    }

    private void Update()
    {
        //Getting the active gun's data
        IData gunData = Shooting.GetData();

        //Checking if the player is touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Getting the axis inputs
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //Moving
        move = transform.right * x + transform.forward * z;
        playerController.Move((gunData.Weight / 100) * currentSpeed * Time.deltaTime * move);

        //Calling the movement methods
        Inputs();
        Gravity();
        Jump();
        Sprint();
        Crouch();
        Slide();
        LedgeGrab();
    }

    private void Inputs()
    {
        //Bool for walking
        isWalking = (z > 0.5 || z < -0.5) && !isSprinting;

        //Input for jumping
        isJumping = Input.GetButtonDown("Jump");

        //Input for sprinting
        isSprinting = Input.GetButton("Sprint");
        //Input for starting sprinting
        justSprinted = Input.GetButtonDown("Sprint");
        //Input for stopping sprinting
        stoppedSprinting = Input.GetButtonUp("Sprint");

        //Input for crouching
        isCrouching = Input.GetButton("Crouch");
        //Input for starting crouching
        justCrouched = Input.GetButtonDown("Crouch");
        //Input for stopping crouching
        stoppedCrouching = Input.GetButtonUp("Crouch");

        //Setting the slide check
        if (isCrouching && isGrounded && currentSpeed == walkSpeed * sprintSpeedMultiplier)
            isSliding = true;
    }

    private void Gravity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (isJumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            hasJumped = true;
        }

        if (isGrounded && hasJumped && velocity.y < 0) hasJumped = false;
    }

    private void Sprint()
    {
        if (isGrounded && !isCrouching && z > 0)
        {
            if (justSprinted)
            {
                isWalking = false;
                ChangeSpeed(sprintSpeedMultiplier);
                if (isChangingFov) StopCoroutine(changeFov);
                changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
            }
        }

        if (stoppedSprinting && !isCrouching && z > 0)
        {
            isWalking = true;
            ChangeSpeed();
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov));
        }

        //Prevents you to sprint while going backwards
        if (isSprinting && !isCrouching && z <= 0 && isGrounded)
        {
            ChangeSpeed();
            isSprinting = false;
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov));
        }
    }

    private void Crouch()
    {
        if (justCrouched)
        {

            //Setting the crouch speed
            if (!isSprinting)
                ChangeSpeed(crouchSpeedMultiplier);

            //Changing the size of the player and teleporting it
            crouchTpPosition = new Vector3(
                transform.position.x + currentSpeed * move.x / 10, 
                transform.position.y - (walkScale.y - crouchScale.y), 
                transform.position.z + currentSpeed * move.z / 10
                );
            if (isChangingScale) StopCoroutine(changeScale);
            changeScale = StartCoroutine(ScalePlayer(crouchScale, crouchTpPosition));
        }

        if (stoppedCrouching)
        {
            //Setting the walkSpeed and changing the fov
            if (isGrounded)
            {
                ChangeSpeed();
                if (isChangingFov) StopCoroutine(changeFov);
                changeFov = StartCoroutine(ChangeFov(fov));
            }

            //Resetting the sliding check
            if (isSliding)
                isSliding = false;

            //Changing the size of the player and teleporting it
            walkTpPosition = new Vector3(
                transform.position.x + currentSpeed * move.x / 10, 
                transform.position.y + (walkScale.y - crouchScale.y), 
                transform.position.z + currentSpeed * move.z / 10
                );
            if (isChangingScale) StopCoroutine(changeScale);
            changeScale = StartCoroutine(ScalePlayer(walkScale, walkTpPosition));
        }
    }

    private void Slide()
    {
        //Changing speed and fov
        if (isGrounded && justCrouched && isSprinting)
        {
            ChangeSpeed(slideForce);
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov * slideFovMultiplier));
        }

        //Slowing down sliding
        if (isSliding && currentSpeed > walkSpeed * crouchSpeedMultiplier)
        {
            if (isGrounded)
                currentSpeed -= currentSpeed * slideCounterForce;
            else
                currentSpeed -= currentSpeed * slideAirCounterForce;
        }
        else if (isSliding && currentSpeed < walkSpeed * crouchSpeedMultiplier)
        {
            ChangeSpeed(crouchSpeedMultiplier);
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov));
            isSliding = false;
        }

        //Resetting the speed and fov
        if (stoppedCrouching && isSprinting)
        {
            ChangeSpeed(sprintSpeedMultiplier);
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
        }
        else if (stoppedCrouching && !isSprinting)
        {
            ChangeSpeed();
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov));
        }

    }

    private void LedgeGrab()
    {
        //Checks for collisions and makes the player jump
        if (hasJumped)
        {
            //Creates the raycast
            bool ray = Physics.Raycast(transform.position + rayOffset + transform.forward * rayDistance, Vector3.down, rayLength, groundMask);

            if (!hasLedged && ray)
            {
                velocity.y = Mathf.Sqrt(ledgeJumpHeight * -2 * gravity);
                currentSpeed /= 2;
                hasLedged = true;
            }
        }
        
        if (isGrounded && velocity.y < 0) hasLedged = false;
    }

    private IEnumerator ScalePlayer(Vector3 newScale, Vector3 newPosition)
    {
        isChangingScale = true;

        for (float i = 0; i < 1; i += Time.deltaTime / scaleLerpTime)
        {
            transform.localScale = Vector3.Lerp(walkScale, newScale, i);
            if (isGrounded)
                transform.position = Vector3.Lerp(transform.position, newPosition, i);

            yield return null;
        }

        isChangingScale = false;
    }

    private IEnumerator ChangeFov(float newFov)
    {
        isChangingFov = true;
        float elapsed = 0;

        while (elapsed < cameraLerpTime)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newFov, elapsed / cameraLerpTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camera.fieldOfView = newFov;

        isChangingFov = false;
    }

    private void ChangeSpeed(float multiplier = 1)
    {
        currentSpeed = walkSpeed * multiplier;
    }
}
