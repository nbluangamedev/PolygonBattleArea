using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;

    private float jumpHeight;
    private float gravity;
    private float stepDown;
    private float airControl;
    private float jumpDamp;
    private float groundSpeed;
    private float pushPower;

    private Animator animator;
    private CharacterController characterController;
    private ActiveWeapon activeWeapon;
    private WeaponReload reloadWeapon;
    private CharacterAiming characterAiming;
    private Vector2 userInput;
    private Vector3 rootMotion;
    private Vector3 velocity;
    private bool isJumping;

    private int isSprintingParam = Animator.StringToHash("isSprinting");

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();
        reloadWeapon = GetComponent<WeaponReload>();
        characterAiming = GetComponent<CharacterAiming>();
        if (DataManager.HasInstance)
        {
            jumpHeight = DataManager.Instance.globalConfig.jumpHeight;
            gravity = DataManager.Instance.globalConfig.gravity;
            stepDown = DataManager.Instance.globalConfig.stepDown;
            airControl = DataManager.Instance.globalConfig.airControl;
            jumpDamp = DataManager.Instance.globalConfig.jumpDamp;
            groundSpeed = DataManager.Instance.globalConfig.groundSpeed;
            pushPower = DataManager.Instance.globalConfig.pushPower;
        }
    }

    void Update()
    {
        userInput.x = Input.GetAxis("Horizontal");
        userInput.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", userInput.x);
        animator.SetFloat("InputY", userInput.y);

        UpdateIsSprinting();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    private bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = reloadWeapon.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
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

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 airDisplacement = velocity * Time.fixedDeltaTime;
        airDisplacement += CalculateAircontrol();
        characterController.Move(airDisplacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    private Vector3 CalculateAircontrol()
    {
        return ((transform.forward * userInput.y) + (transform.right * userInput.x)) * (airControl / 100);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
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
}
