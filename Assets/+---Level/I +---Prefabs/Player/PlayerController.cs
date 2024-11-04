using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("References")]
    public PlayerMovementStats MovementStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    public UnityEvent StatusUIEvent;
    public UnityEvent<int> levelUpUIEvent;
    public UnityEvent<int, int> expUpUIEvent;
    public UnityEvent<int> lifeUpdateUIEvent;
    public UnityEvent skillCoolTimeUIEvent;
    public PointTextUI pointTextUI;

    public GameObject laserPrefab;
    public GameObject shootPoint;
    public GameObject attackArea;
    public GameObject chargingFX;

    private Rigidbody2D _rb;

    private Animator _animator; // animation

    // Status UI
    public GameObject statusUI;
    private Scene scene;
    public FadeEffect fadeEffectUI;

    // Life
    private bool isDead;
    public bool _isDead
    {
        get { return isDead; }
        set
        {
            if (isDead != value)
            {
                if (value == true)
                {
                    fadeEffectUI.FadeOut();
                    RespawnPointManager.Instance.Respawn(this);
                }

                isDead = value;
                _isDead = value;
            }
        }
    }
    private bool _isBeingDamaged;
    private bool _isAvoiding;
    private float _avoidanceTimer;
    public int dieCount;

    // Movement
    public float HorizontalVelocity { get; private set; }
    public bool _isFacingRight;

    // Collision Check
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private RaycastHit2D _monsterHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    // Jump vars
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    // Apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // Jump Buffer vars
    private float _jumpBufferTimer;
    private bool _isJumpReleasedDuringBuffer;

    // Jump Coyote Time vars
    private float _coyoteTimer;

    // Dash vars
    private bool _isDashing;
    private bool _isAirDashing;
    private float _dashTimer;
    private float _dashOnGroundTimer;
    private int _numberOfDashesUsed;
    private Vector2 _dashDirection;
    private bool _isDashFastFalling;
    private float _dashFastFallTime;
    private float _dashFastFallReleaseSpeed;
    private float _rotationTimer;

    // Attack vars
    private bool _isAttacking;
    private bool isCharging;
    public bool _isCharging
    {
        get { return isCharging; }
        private set
        {
            if (isCharging != value)
            {
                if (value == true) OnChargingSound();
                else OnLaserSound();

                isCharging = value;
            }
        }
    }
    private bool _isChargeAttacking;
    private Transform attackTransform;
    private LayerMask AttackableLayer;
    private RaycastHit2D[] hits;
    private float _chargeTimer;

    // Sound
    public AudioSource audioSrc;
    public AudioClip moveSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip landSound;
    public AudioClip attackSound;
    public AudioClip chargingSound;
    public AudioClip laserSound;
    public AudioClip damagedSound;
    public float _moveSoundTimer;

    #endregion

    private void Awake()
    {
        // Status UI
        //statusUI = GameObject.Find("---StatusUI---").transform.Find("Frame").gameObject;
        // statusUI.SetActive(false);
        statusUI = GameObject.Find("---StatusUI---").gameObject;
        statusUI.GetComponent<Canvas>().enabled = false;

        // Status
        ResetStatus();

        // Life
        _isDead = false;
        _isBeingDamaged = false;
        _isAvoiding = false;
        _avoidanceTimer = 0f;
        dieCount = 0;

        // Movement
        _isJumping = false;
        _isDashing = false;
        _isAirDashing = false;
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // Attack
        attackArea = GameObject.Find("AttackArea");

        // Sound
        _moveSoundTimer = 0f;
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        StatusUICheck();
        StatusCheck();

        if (_isDead)
        {
            return;
        }
        CountTimers();
        JumpChecks();
        DashCheck();
        AttackCheck();
        LandCheck();
        DieCheck();

        CheatCheck();
    }

    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        Dash();
        Attack();
        ChargeAttack();
        Fall();
        Die();
        Animations();
        Sound();

        if (_isGrounded)
        {
            if (!_isDead)
                Move(MovementStats.GroundAcceleration, MovementStats.GroundDeceleration, InputManager.Movement);
        }
        else
        {
            if (!_isDead)
                Move(MovementStats.AirAcceleration, MovementStats.AirDeceleration, InputManager.Movement);
        }

        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        // Clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MovementStats.MaxFallSpeed, 50f);

        _rb.velocity = new Vector2(HorizontalVelocity, VerticalVelocity);
    }

    #region UI

    #region StatusUI

    private void StatusUICheck()
    {
        if (InputManager.StatusWasPressed)
        {
            StatusUIEvent.Invoke();
        }
    }

    public void StatusUIManage()
    {
        statusUI.GetComponent<Canvas>().enabled = !statusUI.GetComponent<Canvas>().enabled;
        // statusUI.SetActive(!statusUI.activeSelf);
    }

    #endregion

    #endregion

    /*
    -----------------------------------------------------------------------------------------------------------------
    
    -----------------------------------------------------------------------------------------------------------------
    */

    #region Movement

    #region Jump

    private void JumpChecks()
    {
        // When we press the jump button
        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MovementStats.JumpBufferTime;
            _isJumpReleasedDuringBuffer = false;
        }

        // When we release the jump button
        if (InputManager.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _isJumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MovementStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        // Initiate jump with jump buffering and coyote time
        if (InputManager.JumpWasPressed && _jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f) && !_isCharging)
        {
            _numberOfJumpsUsed = 1;
            InitiateJump();

            if (_isJumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        // Double jump
        else if (InputManager.JumpWasPressed && _jumpBufferTimer > 0f && (_isJumping || _isAirDashing || _isDashFastFalling) && _numberOfJumpsUsed < MovementStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            _numberOfJumpsUsed = 2;
            InitiateJump();

            if (_isDashFastFalling)
            {
                _isDashFastFalling = false;
            }
        }

        // Handle air jump AFTER coyote time has lapsed (take off an extra jump so we don't get a bonus jump)
        else if (InputManager.JumpWasPressed && _jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MovementStats.NumberOfJumpsAllowed - 1)
        {
            _numberOfJumpsUsed = 2;
            InitiateJump();
            _isFastFalling = false;
        }

    }

    private void ResetJumpValues()
    {
        _isJumping = false;
        _isFalling = false;
        _isFastFalling = false;
        _fastFallTime = 0f;
        _isPastApexThreshold = false;
        _numberOfJumpsUsed = 0;
    }

    private void InitiateJump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        VerticalVelocity = MovementStats.InitialJumpVelocity;

        // sound
        audioSrc.PlayOneShot(jumpSound);

    }

    private void Jump()
    {
        // Apply gravity while jumping
        if (_isJumping)
        {
            // Check for head bump
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            // Gravity on ascending
            if (VerticalVelocity >= 0f)
            {
                // Apex controls
                _apexPoint = Mathf.InverseLerp(MovementStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > MovementStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < MovementStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }

                // Gravity on ascending but not past apex threshold
                else if (!_isFastFalling)
                {
                    VerticalVelocity += MovementStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }

            }

            // Gravity on descending
            else if (!_isFastFalling)
            {
                VerticalVelocity += MovementStats.Gravity * MovementStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        // Jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= MovementStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MovementStats.Gravity * MovementStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < MovementStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MovementStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Move

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (!_isDashing)
        {

            if (moveInput != Vector2.zero)
            {

                TurnCheck(moveInput);

                float targetVelocity = 0f;
                if (InputManager.RunIsHeld)
                {
                    targetVelocity = moveInput.x * MovementStats.MaxRunSpeed;
                }
                else
                {
                    targetVelocity = moveInput.x * MovementStats.MaxWalkSpeed;
                }

                HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }

            else if (moveInput == Vector2.zero)
            {
                HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
            }
        }

    }

    #endregion

    #region Land/Fall

    private void LandCheck()
    {
        // Landed
        if ((_isJumping || _isFalling || _isDashFastFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            // sound
            audioSrc.PlayOneShot(landSound);

            ResetJumpValues();
            ResetDashes();

            VerticalVelocity = Physics2D.gravity.y;

            if (_isDashFastFalling && _isGrounded)
            {
                ResetDashValues();
                return;
            }

            ResetDashValues();
        }
    }

    private void Fall()
    {
        // Normal gravity while falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += MovementStats.Gravity * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Dash

    private void DashCheck()
    {
        if (InputManager.DashWasPressed)
        {
            // ground dash
            if (_isGrounded && _dashOnGroundTimer <= 0f && !_isDashing)
            {
                InitiateDash();
            }

            // air dash
            else if (!_isGrounded && _numberOfDashesUsed < MovementStats.NumberOfDashes && !_isDashing)
            {
                _isAirDashing = true;
                InitiateDash();
            }
        }
    }

    private void InitiateDash()
    {
        _dashDirection = InputManager.Movement;

        Vector2 closestDirection = Vector2.zero;
        float minDistance = Vector2.Distance(_dashDirection, MovementStats.DashDirections[0]);

        for (int i = 0; i < MovementStats.DashDirections.Length; i++)
        {
            // skip if we hit it bang on
            if (_dashDirection == MovementStats.DashDirections[i])
            {
                closestDirection = _dashDirection;
                break;
            }

            float distance = Vector2.Distance(_dashDirection, MovementStats.DashDirections[i]);

            // check if this is a diagonal direction and apply bias
            bool isDiagonal = (Mathf.Abs(MovementStats.DashDirections[i].x) > 0 && Mathf.Abs(MovementStats.DashDirections[i].y) > 0);

            if (isDiagonal)
            {
                distance -= MovementStats.DashDiagonallyBias;
            }
            else if (distance < minDistance)
            {
                minDistance = distance;
                closestDirection = MovementStats.DashDirections[i];
            }

        }

        // handle dash direction with NO input
        if (closestDirection == Vector2.zero)
        {
            /* if (_isFacingRight)
            {
                closestDirection = Vector2.right;
            }
            else
            {
                closestDirection = Vector2.left;
            } */
            _isDashing = false;
        }
        else
        {
            _dashDirection = closestDirection;
            _numberOfDashesUsed++;
            _isDashing = true;
            _dashTimer = 0f;
            _dashOnGroundTimer = MovementStats.TimeBtwDashesOnGround;

            _rotationTimer = 0f;
        }

        // sound
        audioSrc.PlayOneShot(dashSound);

        // ResetJumpValues();
    }

    private void Dash()
    {
        if (_isDashing || _isAirDashing)
        {
            // stop the dash after the timer
            _dashTimer += Time.fixedDeltaTime;

            if (_dashTimer >= MovementStats.DashTime)
            {
                if (_isGrounded)
                {
                    ResetDashes();
                }

                _isAirDashing = false;
                _isDashing = false;

                if (!_isJumping)
                {
                    _dashFastFallTime = 0f;
                    _dashFastFallReleaseSpeed = VerticalVelocity;

                    if (!_isGrounded)
                    {
                        _isDashFastFalling = true;
                    }
                }

                return;
            }

            HorizontalVelocity = MovementStats.DashSpeed * _dashDirection.x;

            if (_dashDirection.y != 0f || _isAirDashing)
            {
                // 뒤 상수는 대각선 대시 방향 보정
                VerticalVelocity = MovementStats.DashSpeed * _dashDirection.y;
            }

        }

        // handle dash cut time
        else if (_isDashFastFalling)
        {
            if (VerticalVelocity > 0f)
                if (_dashFastFallTime < MovementStats.DashTimeForUpwardsCancel)
                {
                    // 땅에 있을 때 대시하면 높게 안 올라가기 때문에 주석처리
                    //VerticalVelocity = Mathf.Lerp(_dashFastFallReleaseSpeed, 0f, (_dashFastFallTime / MovementStats.DashTimeForUpwardsCancel));
                    VerticalVelocity += MovementStats.Gravity * MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
                else if (_dashFastFallTime >= MovementStats.DashTimeForUpwardsCancel)
                {
                    VerticalVelocity += MovementStats.Gravity * MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }

            _dashFastFallTime += Time.fixedDeltaTime;
        }

        else
        {
            VerticalVelocity += MovementStats.Gravity * MovementStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
        }
    }

    private void ResetDashValues()
    {
        _isDashFastFalling = false;
        _dashOnGroundTimer = -0.01f;
        _dashTimer = 0f;
    }

    private void ResetDashes()
    {
        _numberOfDashesUsed = 0;
    }

    #endregion

    private void TurnCheck(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !_isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && _isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 localeScale = transform.localScale;
        localeScale.x *= -1f;
        transform.localScale = localeScale;
    }

    #endregion

    #region Attack

    private void AttackCheck()
    {
        if (InputManager.AttackIsHolding)
        {
            if (GameManager.instance.coolTimeRatio > 0.0f)
                _chargeTimer = 0f;
            else
                _chargeTimer += Time.fixedDeltaTime;
        }
        if (InputManager.AttackWasPressed)
        {
            InitiateAttack();
        }
        if (InputManager.AttackWasReleased)
        {
            if (_isCharging)
            {
                InitiateChargeAttack();
            }
        }
    }

    private void InitiateAttack()
    {
        _isAttacking = true;
        _chargeTimer = 0f;

        // sound
        audioSrc.PlayOneShot(attackSound);
    }

    private void InitiateChargeAttack()
    {
        _isAttacking = false;
        _chargeTimer = 0f;
        _isCharging = false;
        _isChargeAttacking = true;
    }


    private void Attack()
    {
        if (_isAttacking)
        {
            attackArea.SetActive(true);
        }
        else if (!_isAttacking)
        {
            attackArea.SetActive(false);
        }

        if (_chargeTimer >= MovementStats.ChargeTime && GameManager.instance.coolTimeRatio <= 0.0f)
        {
            _isCharging = true;
        }
    }

    private void ChargeAttack()
    {
        if (_isCharging && _isGrounded)
        {
            HorizontalVelocity = 0f;
        }

        if (_isChargeAttacking)
        {
            // Shoot Laser
            ShootLaser();
            _isChargeAttacking = false;

            StartCoroutine(GameManager.instance.CalculateCoolTime(MovementStats.LaserCoolTime));
            skillCoolTimeUIEvent.Invoke();
        }
    }

    private void AttackFinished()
    {
        ResetAttackValues();
    }

    private void ResetAttackValues()
    {
        _isAttacking = false;
        _isCharging = false;
        _isChargeAttacking = false;
        _chargeTimer = 0f;

    }

    #endregion


    #region Shoot

    public void ShootLaser()
    {
        GameObject clone = Instantiate(laserPrefab);

        clone.transform.localScale = new Vector3(6f, 6f, 10f);
        clone.transform.position = shootPoint.transform.position;
        clone.transform.rotation = shootPoint.transform.rotation;
    }

    #endregion



    #region Life

    public IEnumerator Damaged()
    {
        if (!_isBeingDamaged)
        {
            if (!_isAvoiding)
            {
                // sound
                if (!_isDead)
                    audioSrc.PlayOneShot(damagedSound);

                _isBeingDamaged = true;
                MovementStats.Life -= 1;
                lifeUpdateUIEvent.Invoke(MovementStats.Life);
                StartCoroutine(ChangeRed());
                yield return new WaitForSeconds(1f);
                _isBeingDamaged = false;
            }
        }
    }

    private IEnumerator ChangeRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void DieCheck()
    {
        if (MovementStats.Life <= 0 || transform.position.y < -20f)
        {
            _isDead = true;
        }
        else if (transform.position.y < -16f)
        {
            scene = SceneManager.GetActiveScene();
            if (scene.name == "BossScene" && transform.position.y < -20f)
                _isDead = true;
            else if (scene.name != "BossScene")
                _isDead = true;
        }
        else
        {
            _isDead = false;
        }
    }

    private void Die()
    {
        if (_isDead)
        {
            _isDashing = false;
            _isAirDashing = false;
            _isJumping = false;
            _isAttacking = false;
            _isCharging = false;
            _isChargeAttacking = false;
            _isBeingDamaged = false;
            HorizontalVelocity = 0f;
        }
    }

    public void Revive()
    {
        _animator.SetTrigger("isRevived");
        _isDead = false;
        MovementStats.Life = MovementStats.MaxLife;
        lifeUpdateUIEvent.Invoke(MovementStats.Life);
        dieCount++;
    }

    #endregion

    #region Status (Level Up, EXP, Strength, ...)

    public void ExpUp(int exp)
    {
        MovementStats.Exp += exp;
        expUpUIEvent.Invoke(100, MovementStats.Exp);    // 100은 임의로 설정함, 수정 바람
    }

    private void StatusCheck()
    {
        // Life
        // Todo: Life Up

        // Level Up
        if ((int)(MovementStats.Exp / 10) > MovementStats.Level)
        {
            if (MovementStats.Level >= MovementStats.MaxLevel)
            {
                return;
            }
            MovementStats.Level = (int)(MovementStats.Exp / 10);
            MovementStats.Life = MovementStats.MaxLife;
            lifeUpdateUIEvent.Invoke(MovementStats.Life);
            levelUpUIEvent.Invoke(MovementStats.Level);
            pointTextUI.onChanged.Invoke();
        }

        // Strength
        MovementStats.AttackDamage = MovementStats.Strength * 10f + 30f;
        MovementStats.LaserDamage = MovementStats.Strength * 10f + 30f;

        // Dodge
        // Todo: Dodge

        // SkillCoolTime
        float tempValue = 5f - (MovementStats.SkillCoolTime * 0.5f);
        if (tempValue < 0.5f)
            tempValue = 0.5f;
        MovementStats.LaserCoolTime = tempValue;
    }

    private void ResetStatus()
    {
        MovementStats.MaxLife = 5;
        MovementStats.Level = 1;
        MovementStats.Exp = 10;
        MovementStats.Life = MovementStats.MaxLife;
        lifeUpdateUIEvent.Invoke(MovementStats.Life);
        MovementStats.Strength = 1;
        MovementStats.Dodge = 1;
        MovementStats.SkillCoolTime = 1;
    }

    #endregion



    #region Collision Check

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MovementStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MovementStats.GroundDetectionRayLength, MovementStats.GroundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_bodyColl.bounds.size.x * MovementStats.HeadWidth, MovementStats.HeadDetectionRayLength);
        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MovementStats.HeadDetectionRayLength, MovementStats.GroundLayer);

        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else
        {
            _bumpedHead = false;
        }
    }

    private void PlayerCollidesWithMonster()
    {
        // BodyColl Collides with Monster Layer
        if (_isDead) return;

        _monsterHit = Physics2D.BoxCast(_bodyColl.bounds.center, _bodyColl.bounds.size, 0f, Vector2.zero, 0f, MovementStats.MonsterLayer);
        if (_monsterHit.collider != null)
        {
            StartCoroutine(Damaged());
        }

    }

    private void AvoidCheck()
    {
        if (_avoidanceTimer < 5f)
        {
            _avoidanceTimer += Time.fixedDeltaTime;
        }
        if (_isDashing || _isAirDashing)
        {
            _isAvoiding = true;
            _avoidanceTimer = 0f;
        }
        if (_avoidanceTimer > MovementStats.AvoidanceTime)
        {
            _isAvoiding = false;
        }
    }

    private void CollisionCheck()
    {
        IsGrounded();
        BumpedHead();
        PlayerCollidesWithMonster();
        AvoidCheck();
    }


    #endregion

    #region Timers

    private void CountTimers()
    {

        // jump buffer
        _jumpBufferTimer += Time.deltaTime;

        // jump coyote time
        if (!_isGrounded)
        {
            _coyoteTimer += Time.deltaTime;
        }
        else
        {
            _coyoteTimer = MovementStats.JumpCoyoteTime;
        }

        // dash timer
        if (_isGrounded)
        {
            _dashOnGroundTimer -= Time.deltaTime;
        }
    }

    #endregion

    #region Animations

    private void Animations()
    {
        if (_isDead)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Player Die"))
            {
                _animator.SetBool("isCharging", false);
                _animator.SetBool("isAttack1", false);
                _animator.SetFloat("HorizontalVelocity", 0f);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isDashing", false);
                _animator.SetTrigger("isDead");
            }
        }

        else
        {
            _animator.ResetTrigger("isDead");
            {
                // attack animation
                _animator.SetBool("isCharging", _isCharging);
                _animator.SetBool("isAttack1", _isAttacking);
                _animator.SetFloat("HorizontalVelocity", Mathf.Abs(HorizontalVelocity));
                _animator.SetBool("isJumping", _isJumping);
                _animator.SetBool("isDashing", _isDashing || _isAirDashing);
            }
            {
                _rotationTimer += Time.fixedDeltaTime;
                if ((_isDashing || _isAirDashing) && !_isGrounded)
                {
                    // 대시 방향으로 캐릭터 rotate
                    if (_isFacingRight)
                        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_dashDirection.y, _dashDirection.x) * Mathf.Rad2Deg);
                    else
                        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_dashDirection.y, _dashDirection.x) * Mathf.Rad2Deg + 180);
                }
                else if (_rotationTimer > 0.3f)
                {
                    _rotationTimer = 0f;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            {
                if (_isCharging)
                {
                    chargingFX.SetActive(true);
                }
                else
                {
                    chargingFX.SetActive(false);
                }
            }
        }
    }

    #endregion

    #region Sound



    private void Sound()
    {
        // walking
        if (_isGrounded && HorizontalVelocity != 0 && (!_isDashing || !_isAirDashing))
        {
            if (_moveSoundTimer > MovementStats.MoveSoundGap || _moveSoundTimer == 0f)
            {
                if (InputManager.Movement.x != 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                {
                    audioSrc.PlayOneShot(moveSound);
                    _moveSoundTimer = 0f;
                }
            }
            _moveSoundTimer += Time.fixedDeltaTime;
        }
    }

    private void OnChargingSound()
    {
        audioSrc.PlayOneShot(chargingSound);
    }

    private void OnLaserSound()
    {
        audioSrc.PlayOneShot(laserSound);
    }

    #endregion

    #region Cheat

    private void CheatCheck()
    {
        if (InputManager.CheatWasPressed)
        {
            // Boss room entrance
            transform.position = new Vector3(378f, 2f, 0f);
        }
    }

    #endregion





}