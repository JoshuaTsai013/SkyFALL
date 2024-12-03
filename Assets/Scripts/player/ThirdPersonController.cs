using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputs))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Player")]
    [Space(5)]

    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeedOnGround = 7.0f;
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeedOnAir = 4.0f;
    [Tooltip("How fast the character turns to face movement direction on ground")]
    public float RotationSmoothTimeOnGround = 0.15f;
    [Tooltip("How fast the character turns to face movement direction on Air")]
    public float RotationSmoothTimeOnAir = 0.8f;
    [Tooltip("How fast the character turns to face movement direction on Aim")]
    public float RotationSmoothTimeOnAim = 0.15f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Header("Dash")]
    [Space(5)]

    [Tooltip("Player started Dash")]
    public bool isDash = false;
    [Tooltip("Dash speed of the character in m/s")]
    public float DashSpeed = 12.335f;
    [Tooltip("Time required to pass before being able to Dash again. Set to 0f to instantly Dash again")]
    public float DashTimeout = 0.4f;
    [Tooltip("How long a Dash Performed")]
    public float DashDuration = 0.2f;

    [Header("Jump")]
    [Space(5)]
    public VisualEffect JumpEffect1;
    public VisualEffect JumpEffect2;
    public VisualEffect JumpEffect3;
    public VisualEffect JumpEffect4;


    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("Time required to ready before jump. Set to 0f to instantly jump")]
    public float JumpDelayTimeout = 0.20f;
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0;

    [Header("Player Grounded")]
    [Space(5)]

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Space(5)]

    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    [Range(-50.0f, 50.0f)] public float TopClamp = 10.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [Range(-50.0f, 50.0f)] public float BottomClamp = -10.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private Vector3 _inputDirection;
    private Vector3 _targetDirection;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    [SerializeField]
    private float _rotationSmoothTime = 0.15f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private readonly float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _DashTimeoutDelta;
    private float _DashDurationDelta;
    private float _jumpDelayTimeoutDelta;
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private PlayerInput _playerInput;
    public Animator _animator;
    private CharacterController _controller;

    private PlayerInputs _input;  //Created by me
    private GameObject _mainCamera;
    private ThirdPersonShooterController _thirdPersonShooterController;
    private float currentHeat;
    private float maxHeat;
    private const float _threshold = 0.01f;
    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputs>();

        _playerInput = GetComponent<PlayerInput>();
        _thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();


        _rotationSmoothTime = RotationSmoothTimeOnGround;
        // reset our timeouts on start
        _jumpDelayTimeoutDelta = JumpDelayTimeout;
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
        _DashTimeoutDelta = DashTimeout;
        _DashDurationDelta = DashDuration;
    }

    private void Update()
    {
        Dash();
        Move();
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        JumpAndGravity();
        CheckDirection();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        if (Grounded || isDash)
        {
            MoveSpeed = MoveSpeedOnGround;
        }
        else
        {
            MoveSpeed = MoveSpeedOnAir;
        }
        // update animator if using character
        _animator.SetBool("Grounded", Grounded);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float targetSpeed = MoveSpeed;
        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        _inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        //act according to states
        if (isDash != true && _thirdPersonShooterController.isAiming != true)
        {
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) { targetSpeed = 0.0f; }

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            { _speed = targetSpeed; }

            // normalise input direction

            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    _rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        }
        else if (isDash == true)
        {
            targetSpeed = DashSpeed;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            { _speed = targetSpeed; }

            _targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        }
        else
        {
            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else if (_input.move == Vector2.zero)
            {
                _speed = 0.0f;
            }
            else
            { _speed = targetSpeed; }


            // rotate to face camera rotation
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _mainCamera.transform.eulerAngles.y, ref _rotationVelocity, RotationSmoothTimeOnAim);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            _targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // move the player
        _controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        _animator.SetFloat("Speed", _animationBlend);
        _animator.SetFloat("MotionSpeed", inputMagnitude);
    }
    
    private void CheckDirection()
    {
        float smoothX = Mathf.Lerp(_animator.GetFloat("X"), _inputDirection.x, Time.deltaTime * SpeedChangeRate);
        float smoothZ = Mathf.Lerp(_animator.GetFloat("Z"), _inputDirection.z, Time.deltaTime * SpeedChangeRate);
        _animator.SetFloat("X", smoothX);
        _animator.SetFloat("Z", smoothZ);
    }
    private void Dash()
    {

        if (_input.Dash && (_DashTimeoutDelta <= 0.0f) && (_input.move != Vector2.zero))
        {
            isDash = true;
            _animator.SetBool("Dash", true);
            JumpEffect3.Play();
            JumpEffect4.Play();
            //check angle between player input and character facing
            Vector3 vectorinputXZ = new(_inputDirection.x, 0, _inputDirection.z);
            Quaternion cameraLookRotation = Quaternion.LookRotation(_mainCamera.transform.forward);
            Vector3 result = cameraLookRotation * vectorinputXZ;

            float angle = Vector3.Angle(result, transform.forward);

            // Debug.Log("輸入角度:" + vectorinputXZ);
            // Debug.Log("角色面向角度:" + transform.forward);
            // Debug.Log("相機角度:" + _mainCamera.transform.forward);

            Debug.Log("Player Dashed 夾角是: " + angle + " 度");

            _input.Dash = false;
            _DashTimeoutDelta = DashTimeout;
        }
        else
        {
            _input.Dash = false;
        }

        //Timeout Dash
        if (isDash)
        {
            _DashDurationDelta -= Time.deltaTime;
            if (_DashDurationDelta <= 0.0f)
            {
                _DashDurationDelta = DashDuration;
                _input.Dash = false;
                isDash = false;
                _animator.SetBool("Dash", false);
                JumpEffect3.Stop();
                JumpEffect4.Stop();
            }
        }

        if (_DashTimeoutDelta >= 0.0f)
        {
            _DashTimeoutDelta -= Time.deltaTime;
        }
    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            //reset the Rotation speed on ground
            _rotationSmoothTime = RotationSmoothTimeOnGround;
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            _animator.SetBool("Jump", false);
            _animator.SetBool("FreeFall", false);
            JumpEffect1.Stop();
            JumpEffect2.Stop();

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if ((_input.jump == true) && (_jumpTimeoutDelta <= 0.0f) && (isDash == false))
            {
                _jumpDelayTimeoutDelta -= Time.deltaTime;
                _animator.SetBool("Jump", true);
            }
            if (_jumpDelayTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = 20f;
                _input.jump = false;
                _jumpDelayTimeoutDelta = JumpDelayTimeout;
                _jumpTimeoutDelta = JumpTimeout;
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
                _input.jump = false;
            }
        }
        else
        {
            JumpEffect1.Play();
            JumpEffect2.Play();
            //set the Rotation speed on Air
            _rotationSmoothTime = RotationSmoothTimeOnAir;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                _animator.SetBool("FreeFall", true);
            }
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }
}
