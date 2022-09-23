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
    private float _currentSpeed;
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
    public Vector3 rayOffset = new(0, 1, 0);
    public Transform gunContainer;

    private bool _isGrounded;
    private bool _isJumping;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool isSliding;
    private bool _isCurrentlySprinting, _justSprinted, _stoppedSprinting;
    private bool _isCurrentlyCrouching, _justCrouched, _stoppedCrouching;
    private bool _hasJumped;
    private bool _hasLedged;
    private float _x, _z;

    private Vector3 _velocity;
    private Vector3 _move;
    private readonly Vector3 _walkScale = new(1, 1.5f, 1);
    private readonly Vector3 _crouchScale = new(1, 0.5f, 1);
    private Vector3 _walkTpPosition;
    private Vector3 _crouchTpPosition;

    private bool _isChangingFov;
    private bool _isChangingScale;
    private Coroutine _changeFov;
    private Coroutine _changeScale;

    private IWeaponData _gunWeaponData;

    private void Start()
    {
        transform.localScale = _walkScale;
        _currentSpeed = walkSpeed;
        camera.fieldOfView = fov;

        _gunWeaponData = Shooting.GetData();
        Shooting.WeaponSwitchInput += () => _gunWeaponData = Shooting.GetData();
        Shooting.SemiShootInput += StopSprint;
        Shooting.AutoShootInput += StopSprint;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Getting the axis inputs
        _x = Input.GetAxis("Horizontal");
        _z = Input.GetAxis("Vertical");

        //Moving
        var playerTransform = transform;
        _move = playerTransform.right * _x + playerTransform.forward * _z;
        playerController.Move((_gunWeaponData.Weight / 100) * _currentSpeed * Time.deltaTime * _move);

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
        isWalking = (_z > 0.5 || _z < -0.5 || _x > 0.5 || _x < -0.5) && !isSprinting;

        _isJumping = Input.GetButtonDown("Jump");

        _isCurrentlySprinting = Input.GetButton("Sprint");
        _justSprinted = Input.GetButtonDown("Sprint");
        _stoppedSprinting = Input.GetButtonUp("Sprint");

        _isCurrentlyCrouching = Input.GetButton("Crouch");
        _justCrouched = Input.GetButtonDown("Crouch");
        _stoppedCrouching = Input.GetButtonUp("Crouch");
    }

    private void Gravity()
    {
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        playerController.Move(_velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (_isJumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            _hasJumped = true;
        }

        if (_isGrounded && _hasJumped && _velocity.y < 0) _hasJumped = false;

        if (_hasJumped && isCrouching) StopCrouch();
        if (_hasJumped && isSliding) isSliding = false;
    }

    private void Sprint()
    {
        if (_isGrounded && !_isCurrentlyCrouching && _z > 0)
        {
            switch (MenuFunctions.HoldToSprint)
            {
                case 0 when !isSprinting && _justSprinted:
                    StartSprint();
                    return;
                
                case 0 when isSprinting && _justSprinted:
                    StopSprint();
                    return;
                
                case 1 when _justSprinted:
                    StartSprint();
                    return;
                
                case 1 when _stoppedSprinting:
                    StopSprint();
                    return;
            }
        }

        //Prevents you to sprint while going backwards
        if (isSprinting && !_isCurrentlyCrouching && _z <= 0 && _isGrounded)
            StopSprint();
    }

    private void StartSprint()
    {
        isSprinting = true;

        isWalking = false;
        if (isCrouching) StopCrouch();
        ChangeSpeed(sprintSpeedMultiplier);
        if (_isChangingFov) StopCoroutine(_changeFov);
        _changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
    }

    private void StopSprint()
    {
        isSprinting = false;

        isWalking = true;
        ChangeSpeed();
        if (_isChangingFov) StopCoroutine(_changeFov);
        _changeFov = StartCoroutine(ChangeFov(fov));
    }

    private void Crouch()
    {
        switch (MenuFunctions.HoldToCrouch)
        {
            case 0 when !isCrouching && _justCrouched:
                StartCrouch();
                return;
            
            case 0 when isCrouching && _justCrouched:
                StopCrouch();
                return;
            
            case 1 when _justCrouched:
                StartCrouch();
                return;
            
            case 1 when _stoppedCrouching:
                StopCrouch();
                return;
        }
    }
    
    private void StartCrouch()
    {
        isCrouching = true;

        //Setting the crouch speed
        if (!isSprinting && !isSliding)
            ChangeSpeed(crouchSpeedMultiplier);

        //Changing the size of the player and teleporting it
        var position = transform.position;
        _crouchTpPosition = new Vector3(
            position.x + _currentSpeed * _move.x / 10,
            position.y - (_walkScale.y - _crouchScale.y),
            position.z + _currentSpeed * _move.z / 10
            );
        if (_isChangingScale) StopCoroutine(_changeScale);
        _changeScale = StartCoroutine(ScalePlayer(_crouchScale, _crouchTpPosition));
    }

    private void StopCrouch()
    {
        isCrouching = false;
        isSliding = false;

        //Setting the speed and changing the fov
        if (_isGrounded)
        {
            if (isSprinting)
            {
                isSprinting = true;
                ChangeSpeed(sprintSpeedMultiplier);
                if (_isChangingFov) StopCoroutine(_changeFov);
                _changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
            }
            else
            {
                ChangeSpeed();
                if (_isChangingFov) StopCoroutine(_changeFov);
                _changeFov = StartCoroutine(ChangeFov(fov));
            }
        }

        //Changing the size of the player and teleporting it
        var position = transform.position;
        _walkTpPosition = new Vector3(
            position.x + _currentSpeed * _move.x / 10,
            position.y + (_walkScale.y - _crouchScale.y),
            position.z + _currentSpeed * _move.z / 10
            );
        if (_isChangingScale) StopCoroutine(_changeScale);
        _changeScale = StartCoroutine(ScalePlayer(_walkScale, _walkTpPosition));
    }

    private void Slide()
    {
        //Changing speed and fov
        if (_isGrounded && isCrouching && _justCrouched && isSprinting)
        {
            isSliding = true;
            ChangeSpeed(slideForce);
            if (_isChangingFov) StopCoroutine(_changeFov);
            _changeFov = StartCoroutine(ChangeFov(fov * slideFovMultiplier));
        }

        //Slowing down while sliding
        if (isSliding)
        {
            if (_isGrounded)
                _currentSpeed -= _currentSpeed * slideCounterForce;
            else
                _currentSpeed -= _currentSpeed * slideAirCounterForce;
        }
        
        //Resetting speed when it is under default crouch speed
        if (isSliding && _currentSpeed < walkSpeed * crouchSpeedMultiplier)
        {
            ChangeSpeed(crouchSpeedMultiplier);
            if (_isChangingFov) StopCoroutine(_changeFov);
            _changeFov = StartCoroutine(ChangeFov(fov));
            isSliding = false;
            return;
        }

        if (MenuFunctions.HoldToCrouch != 1 || !_stoppedCrouching) return;
        isSliding = false;

        if (!_isGrounded)
            StartCoroutine(StopSlide());
        else
        {
            if (isSprinting)
            {
                ChangeSpeed(sprintSpeedMultiplier);
                if (_isChangingFov) StopCoroutine(_changeFov);
                _changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
            }
            else
            {
                ChangeSpeed();
                if (_isChangingFov) StopCoroutine(_changeFov);
                _changeFov = StartCoroutine(ChangeFov(fov));
            }
        }
    }

    private IEnumerator StopSlide()
    {
        yield return new WaitUntil(() => _isGrounded);
        isSliding = false;

        if (isSprinting)
        {
            ChangeSpeed(sprintSpeedMultiplier);
            if (_isChangingFov) StopCoroutine(_changeFov);
            _changeFov = StartCoroutine(ChangeFov(fov * sprintFovMultiplier));
        }
        else
        {
            ChangeSpeed();
            if (_isChangingFov) StopCoroutine(_changeFov);
            _changeFov = StartCoroutine(ChangeFov(fov));
        }
    }

    private void LedgeGrab()
    {
        //Checks for collisions and makes the player jump
        if (!_hasJumped) return;
        //Creates the raycast
        var playerTransform = transform;
        bool ray = Physics.Raycast(playerTransform.position + rayOffset + playerTransform.forward * rayDistance, Vector3.down, rayLength, groundMask);

        if (!_hasLedged && ray)
            StartCoroutine(Ledging());
    }

    private IEnumerator Ledging()
    {
        _velocity.y = Mathf.Sqrt(ledgeJumpHeight * -2 * gravity);
        _currentSpeed /= 2;
        _hasLedged = true;

        yield return new WaitUntil(() => _isGrounded);

        _currentSpeed = isSprinting ? walkSpeed * sprintSpeedMultiplier : walkSpeed;
        _hasLedged = false;
    }

    private IEnumerator ScalePlayer(Vector3 newScale, Vector3 newPosition)
    {
        _isChangingScale = true;

        for (float i = 0; i < 1; i += Time.deltaTime / scaleLerpTime)
        {
            transform.localScale = Vector3.Lerp(_walkScale, newScale, i);
            if (_isGrounded)
                transform.position = Vector3.Lerp(transform.position, newPosition, i);

            yield return null;
        }

        _isChangingScale = false;
    }

    private IEnumerator ChangeFov(float newFov)
    {
        _isChangingFov = true;
        float elapsed = 0;

        while (elapsed < cameraLerpTime)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newFov, elapsed / cameraLerpTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camera.fieldOfView = newFov;

        _isChangingFov = false;
    }

    private void ChangeSpeed(float multiplier = 1)
    {
        _currentSpeed = walkSpeed * multiplier;
    }
}
