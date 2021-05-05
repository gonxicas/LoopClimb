using LoopClimb.Others;
using UnityEngine;

namespace LoopClimb.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private float jumpForceX = 1;
        [SerializeField] private float jumpForceY = 1;
        [SerializeField] private float minJumpForceY = 1;
        [SerializeField] private float maxJumpTime = 1;
        [SerializeField] private float minJumpAngle = 40;
        [SerializeField] private float maxJumpAngle = 60;
        private Animator _animator;
        private Rigidbody2D _myRigidbody2D;
        private float _moveInput;
        private float _timer;
        private float _minJumpAngle;
        private float _maxJumpAngle;
        private bool _jumpInput;
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

        #region UNITY_EVENT_FUNCTIONS
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
            SetBool(out _jumpInput, false);
            SetBool(out _touchingGround, false);
            SetBool(out _canMove, false);
            SetBool(out _walking, false);
            SetBool(out _crouching, false);
            SetBool(out _canDoActions, false);
            SetBool(out _cinematic, false);
            SetJumpAnim();
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
        #endregion
        
        #region JUMP

        

        
        private void TryCrouch()
        {
            if (!_canDoActions || !_touchingGround) return;
            //If the jump button is pressed start crouching.
            if (_jumpInput)
            {
                SetBool(out _crouching, true);
                SetCrouchAnim();
                ResetTimer();
            }
            Crouch();
        }
        //It makes the player to crouch and when the jump button is released or
        //when a certain amount of time has passed, it makes the player to jump.
        private void Crouch()
        {
            if (!_crouching) return;

            AddTime();
            if (Input.GetKeyUp(_jumpButton))
            {
                Jump(GetTime());
                SetBool(out _crouching, false);
                SetCrouchAnim();
            }
            //If the player holds the jump button more than a certain amount of time, the player automatically jumps.
            if (GetTime() >= maxJumpTime)
            {
                Jump(GetTime());
                SetBool(out _crouching, false);
                SetCrouchAnim();
            }
        }
        //It is the jump function, which takes into account the time the player has been pressing the jump button.
        private void Jump(float time)
        {
            _canDoActions = false;
            SoundManagaer.Instance.PlayJump();
            SetJumpAnim();
            //Normalize the time the player has been crouching.
            var jumpTime = time / maxJumpTime;
            var angle = Mathf.Lerp(_minJumpAngle, _maxJumpAngle, jumpTime);
            
            var forceX = jumpForceX;
            var forceY = jumpForceY * jumpTime;
            var force = new Vector2(forceX, forceY);
            Vector2 finalForce;
            //If the player is holding the moving input, he will jump in this direction.
            if (_moveInput != 0)
            {
                finalForce = new Vector2(_moveInput, Mathf.Sin(angle)) * force;
                finalForce.y = Mathf.Clamp(finalForce.y, minJumpForceY, Mathf.Sin(_maxJumpAngle) * jumpForceY);
            }
            else
            {
                finalForce = Vector2.up * force;
                finalForce.y = Mathf.Clamp(finalForce.y, minJumpForceY, Mathf.Sin(_maxJumpAngle) * jumpForceY);
            }
            _myRigidbody2D.AddForce(finalForce);
        }
        #endregion
        
        #region MOVEMENT
        
        //This is to prevent the player going to fast.
        private void ClampVelocity()
        {
            _myRigidbody2D.velocity = new Vector2(Mathf.Clamp(_myRigidbody2D.velocity.x, -5, 5f),
                Mathf.Clamp(_myRigidbody2D.velocity.y, -20f, 15f));
        }
        private void Move()
        {
            if (!_canMove || _moveInput == 0 || !_canDoActions || !_touchingGround) return;

            var direction = _myRigidbody2D.position + Vector2.right * (_moveInput * speed * Time.deltaTime);
            _myRigidbody2D.MovePosition(direction);
        }
        //If the player is no longer a cube, this will make the sprite to flip in x.
        private void LookDirection()
        {
            if (!_canMove || _moveInput == 0 || !_touchingGround)
            {
                SetBool(out _walking, false);
                return;
            }

            transform.forward = new Vector3(0, 0, _moveInput);
            SetBool(out _walking, true);
        }
        #endregion
        
        #region OTHERS
        private void GetInputs()
        {
            _moveInput = Input.GetAxisRaw("Horizontal");
            _jumpInput = Input.GetKeyDown(_jumpButton);
            if (Input.GetKeyDown(KeyCode.Z)) _canDoActions = true;
            SetJumpAnim();
            SetBool(out _canMove, !_crouching || !_canDoActions);
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
        #endregion
    }
}