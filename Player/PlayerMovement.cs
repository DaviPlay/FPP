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
    public float walkSpeed = 10;
    public float sprintSpeedMultiplier = 1.5f;
    public float crouchSpeedMultiplier = 0.5f;
    public float slideForce = 5;
    public float slideCounterForce = 0.01f;
    public float slideAirCounterForce = 0.005f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public float ledgeJumpHeight = 1;
    [Range(0, 1)]
    public float scaleLerpTime = 0.5f;

    [Header("Others")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float rayDistance = 0.5f;
    public float rayLength = 5;
    public Vector3 rayOffset = new Vector3(0, 1, 0);
    public Transform gunContainer;

    private bool isGrounded;
    private bool isJumping;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isCrouching;
    private bool isCurrentlySprinting, justSprinted, stoppedSprinting;
    private bool isCurrentlyCrouching, justCrouched, stoppedCrouching;
    private bool isSliding = false;
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

    private IData gunData;

    private void Start()
    {
        transform.localScale = walkScale;
        currentSpeed = walkSpeed;
        camera.fieldOfView = fov;

        gunData = Shooting.GetData();
        Shooting.weaponSwitchInput += () => gunData = Shooting.GetData();
        Shooting.semiShootInput += StopSprint;
        Shooting.autoShootInput += StopSprint;
    }

    private void Update()
    {
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
        isWalking = (z > 0.5 || z < -0.5 || x > 0.5 || x < -0.5) && !isSprinting;

        isJumping = Input.GetButtonDown("Jump");

        isCurrentlySprinting = Input.GetButton("Sprint");
        justSprinted = Input.GetButtonDown("Sprint");
        stoppedSprinting = Input.GetButtonUp("Sprint");

        isCurrentlyCrouching = Input.GetButton("Crouch");
        justCrouched = Input.GetButtonDown("Crouch");
        stoppedCrouching = Input.GetButtonUp("Crouch");
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

        if (hasJumped && isCrouching) StopCrouch();
        if (hasJumped && isSliding) isSliding = false;
    }

    private void Sprint()
    {
        if (isGrounded && !isCurrentlyCrouching && z > 0)
        {
            if (MenuFunctions.HoldToSprint == 0)
            {
                if (!isSprinting && justSprinted)
                {
                    StartSprint();
                    return;
                }

                if (isSprinting && justSprinted)
                {
                    StopSprint();
                    return;
                }
            }
            else if (MenuFunctions.HoldToSprint == 1)
            {
                if (isCurrentlySprinting)
                {
                    StartSprint();
                    return;
                }

                if (stoppedSprinting)
                {
                    StopSprint();
                    return;
                }
            }
        }

        //Prevents you to sprint while going backwards
        if (isSprinting && !isCurrentlyCrouching && z <= 0 && isGrounded)
        {
            StopSprint();
        }
    }

    private void StartSprint()
    {
        isSprinting = true;

        isWalking = false;
        ChangeSpeed(sprintSpeedMultiplier);
        if (isChangingFov) StopCoroutine(changeFov);
        changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
    }

    private void StopSprint()
    {
        isSprinting = false;

        isWalking = true;
        ChangeSpeed();
        if (isChangingFov) StopCoroutine(changeFov);
        changeFov = StartCoroutine(ChangeFov(fov));
    }

    private void Crouch()
    {
        if (MenuFunctions.HoldToCrouch == 0)
        {
            if (!isCrouching && justCrouched)
            {
                StartCrouch();
                return;
            }

            if (isCrouching && justCrouched)
            {
                StopCrouch();
                return;
            }
        }
        else if (MenuFunctions.HoldToCrouch == 1)
        {
            if (isCurrentlyCrouching)
            {
                StartCrouch();
                return;
            }

            if (stoppedCrouching)
            {
                StopCrouch();
                return;
            }
        }
    }
    
    private void StartCrouch()
    {
        isCrouching = true;

        //Setting the crouch speed
        if (!isSprinting && !isSliding)
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

    private void StopCrouch()
    {
        isCrouching = false;
        isSliding = false;

        //Setting the speed and changing the fov
        if (isGrounded)
        {
            if (isSprinting)
            {
                isSprinting = true;
                ChangeSpeed(sprintSpeedMultiplier);
                if (isChangingFov) StopCoroutine(changeFov);
                changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
            }
            else
            {
                ChangeSpeed();
                if (isChangingFov) StopCoroutine(changeFov);
                changeFov = StartCoroutine(ChangeFov(fov));
            }
        }

        //Changing the size of the player and teleporting it
        walkTpPosition = new Vector3(
            transform.position.x + currentSpeed * move.x / 10,
            transform.position.y + (walkScale.y - crouchScale.y),
            transform.position.z + currentSpeed * move.z / 10
            );
        if (isChangingScale) StopCoroutine(changeScale);
        changeScale = StartCoroutine(ScalePlayer(walkScale, walkTpPosition));
    }

    private void Slide()
    {
        //Changing speed and fov
        if (isGrounded && isCrouching && justCrouched && isSprinting)
        {
            isSliding = true;
            ChangeSpeed(slideForce);
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov * slideFovMultiplier));
        }

        //Slowing down while sliding
        if (isSliding)
        {
            if (isGrounded)
                currentSpeed -= currentSpeed * slideCounterForce;
            else
                currentSpeed -= currentSpeed * slideAirCounterForce;
        }
        
        //Resetting speed when it is under default crouch speed
        if (isSliding && currentSpeed < walkSpeed * crouchSpeedMultiplier)
        {
            ChangeSpeed(crouchSpeedMultiplier);
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov));
            isSliding = false;
            return;
        }

        if (MenuFunctions.HoldToCrouch == 1 && stoppedCrouching)
        {
            isSliding = false;

            if (!isGrounded)
                StartCoroutine(StopSlide());
            else
            {
                if (isSprinting)
                {
                    ChangeSpeed(sprintSpeedMultiplier);
                    if (isChangingFov) StopCoroutine(changeFov);
                    changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
                }
                else
                {
                    ChangeSpeed();
                    if (isChangingFov) StopCoroutine(changeFov);
                    changeFov = StartCoroutine(ChangeFov(fov));
                }
            }
        }
    }

    private IEnumerator StopSlide()
    {
        yield return new WaitUntil(() => isGrounded);
        isSliding = false;

        if (isSprinting)
        {
            ChangeSpeed(sprintSpeedMultiplier);
            if (isChangingFov) StopCoroutine(changeFov);
            changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
        }
        else
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
                StartCoroutine(Ledging());
        }
    }

    private IEnumerator Ledging()
    {
        velocity.y = Mathf.Sqrt(ledgeJumpHeight * -2 * gravity);
        currentSpeed /= 2;
        hasLedged = true;

        yield return new WaitUntil(() => isGrounded);

        currentSpeed = isSprinting ? walkSpeed * sprintSpeedMultiplier : walkSpeed;
        hasLedged = false;
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
