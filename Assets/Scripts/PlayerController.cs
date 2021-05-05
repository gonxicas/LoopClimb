using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpForceX = 1;
    [SerializeField] private float jumpForceY = 1;
    [SerializeField] private float minJumpForceY = 1;
    [SerializeField] private float maxJumpForceY = 1;
    [SerializeField] private float maxJumpTime = 1;
    [SerializeField] private float minJumpTime = 0.3f;
    [SerializeField] private float minJumpAngle = 40;
    [SerializeField] private float maxJumpAngle = 60;
    private Animator _animator;
    private Rigidbody2D _myRigidbody2D;
    private Vector2 _lookDirection;
    private float _moveX;
    private float _timer;
    private float _minJumpAngle;
    private float _maxJumpAngle;
    private bool _jump;
    private bool _canJump;
    private bool _walking;
    private bool _crouching;
    private bool _touchingGround;
    private bool _canDoActions;
    private bool _canMove;
    private bool _cinematic;
    private KeyCode _jumpButton;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Crouch1 = Animator.StringToHash("Crouch");
    private static readonly int Fall = Animator.StringToHash("Fall");

    private void Awake()
    {
        _myRigidbody2D = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        _jumpButton = KeyCode.Space;
        _timer = 0;
        _minJumpAngle = minJumpAngle * Mathf.PI / 180;
        _maxJumpAngle = maxJumpAngle * Mathf.PI / 180;
        SetBool(out _jump, false);
        SetBool(out _touchingGround, false);
        SetBool(out _canMove, false);
        SetBool(out _walking, false);
        SetBool(out _crouching, false);
        SetBool(out _canDoActions, false);
        SetJumpAnim();
        SetBool(out _cinematic, false);
    }

    private void Update()
    {
        if (_cinematic) return;
        GetInputs();
        TryCrouch();
        SetWalkAnim();
        LookDirection();
        ClampVelocity();
    }

    private void FixedUpdate()
    {
        if (_cinematic) return;
        Move();
    }

    private void TryCrouch()
    {
        if (!_canDoActions||!_touchingGround) return;

        if (_jump)
        {
            SetBool(out _crouching, true);
            SetCrouchAnim();
            ResetTimer();
        }

        Crouch();
    }

    private void Crouch()
    {
        if (!_crouching) return;

        AddTime();
        if (Input.GetKeyUp(_jumpButton))
        {
            // if (GetTime() < minJumpTime)
            // {
            //     Jump(minJumpTime);
            //     SetBool(out _crouching, false);
            //     SetCrouchAnim();
            // }
            // else
            // {
            //     Jump(GetTime());
            //     SetBool(out _crouching, false);
            //     SetCrouchAnim();
            // }
            Jump(GetTime());
            SetBool(out _crouching, false);
            SetCrouchAnim();
        }

        if (GetTime() >= maxJumpTime)
        {
            Jump(GetTime());
            SetBool(out _crouching, false);
            SetCrouchAnim();
        }
    }

    private void Jump(float time)
    {
        
        _canDoActions = false;
        SoundManagaer.instance.PlayJump();
        SetJumpAnim();
        var jumpTime = time / maxJumpTime;
        var angle = Mathf.Lerp(_minJumpAngle, _maxJumpAngle, jumpTime);
        
        // var finalForceX = jumpForceX * jumpTime;
        var finalForceX = jumpForceX ;
        var finalForceY = jumpForceY * jumpTime;
        var finalForce = new Vector2(finalForceX, finalForceY);
        if (_moveX != 0)
        {
            var force = new Vector2(_moveX, Mathf.Sin(angle)) * finalForce;
            force.y = Mathf.Clamp(force.y, minJumpForceY, Mathf.Sin(_maxJumpAngle) * jumpForceY);
            
            _myRigidbody2D.AddForce(force);
        }
        else
        {
            var force = Vector2.up * finalForce;
            force.y = Mathf.Clamp(force.y, minJumpForceY,  Mathf.Sin(_maxJumpAngle) * jumpForceY);
            _myRigidbody2D.AddForce(force);
        }
    }

    private void ClampVelocity()
    {
        _myRigidbody2D.velocity = new Vector2(Mathf.Clamp(_myRigidbody2D.velocity.x,-5, 5f),
            Mathf.Clamp(_myRigidbody2D.velocity.y,-20f, 15f));
    }

    private void GetInputs()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _jump = Input.GetKeyDown(_jumpButton);
        if (Input.GetKeyDown(KeyCode.Z)) _canDoActions = true; SetJumpAnim();
        SetBool(out _canMove, !_crouching||!_canDoActions);
    }


    private void Move()
    {
        if (!_canMove || _moveX == 0 || !_canDoActions||!_touchingGround) return;

        var direction = _myRigidbody2D.position + Vector2.right * (_moveX * speed * Time.deltaTime);
        _myRigidbody2D.MovePosition(direction);
    }

    private void LookDirection()
    {
        if (!_canMove || _moveX == 0||!_touchingGround)
        {
            SetBool(out _walking, false);
            return;
        }

        transform.forward = new Vector3(0, 0, _moveX);
        SetBool(out _walking, true);
    }


    private void SetBool(out bool boolName, bool value) => boolName = value;

    private void ResetTimer() => _timer = 0;
    private float GetTime() => _timer;
    private void AddTime() => _timer += Time.deltaTime;
    private void SetWalkAnim() => _animator.SetBool(Walk, _walking);
    private void SetJumpAnim() => _animator.SetBool(Jump1, !_canDoActions);
    private void SetCrouchAnim() => _animator.SetBool(Crouch1, _crouching);
    public void SetFallAnim(bool value) => _animator.SetBool(Fall, value);
    public void SetTouchingGround(bool value) => _touchingGround = value;
    public void ResetVelocity() => _myRigidbody2D.velocity = Vector2.zero;
    public void SetCanDoActions(bool value) => _canDoActions = value;

    public void SetCinematic(bool value) => _cinematic = value;
    public bool GetCrouching() => _crouching;
}
