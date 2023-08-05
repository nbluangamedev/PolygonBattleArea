using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;

    private float gravity;
    private float groundSpeed;
    private float stepDown;
    private float jumpHeight;
    private float jumpDamp;
    private float airControl;
    private float pushPower;
    private float animationSmoothTime = 0.15f;

    private Animator animator;
    private CharacterController characterController;
    private ActiveWeapon activeWeapon;
    private WeaponReload weaponReload;
    private CharacterAiming characterAiming;
    private Vector2 input;
    private Vector3 rootMotion;
    private Vector3 velocity;
    private bool isJumping;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    private int isSprintingParameter = Animator.StringToHash("isSprinting");

    private void Start()
    {
        if (DataManager.HasInstance)
        {
            gravity = DataManager.Instance.globalConfig.gravity;
            groundSpeed = DataManager.Instance.globalConfig.groundSpeed;
            stepDown = DataManager.Instance.globalConfig.stepDown;
            airControl = DataManager.Instance.globalConfig.airControl;
            jumpHeight = DataManager.Instance.globalConfig.jumpHeight;
            jumpDamp = DataManager.Instance.globalConfig.jumpDamp;
            pushPower = DataManager.Instance.globalConfig.pushPower;
            animationSmoothTime = DataManager.Instance.globalConfig.animationSmoothTime;
        }

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();
        weaponReload = GetComponent<WeaponReload>();
        characterAiming = GetComponent<CharacterAiming>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        //input.x = Input.GetAxis("Horizontal");
        //input.y = Input.GetAxis("Vertical");

        input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);

        animator.SetFloat("InputX", currentAnimationBlendVector.x);
        animator.SetFloat("InputY", currentAnimationBlendVector.y);

        UpdateIsSprinting();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)   //Air State
        {
            UpdateInAir();
        }
        else           //Ground State
        {
            UpdateOnGround();
        }
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

    private bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = weaponReload.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParameter, isSprinting);
        rigController.SetBool(isSprintingParameter, isSprinting);
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    private Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.deltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        characterController.Move(displacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        characterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!characterController.isGrounded)
        {
            SetInAir(0);
        }
    }

    private void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }
}