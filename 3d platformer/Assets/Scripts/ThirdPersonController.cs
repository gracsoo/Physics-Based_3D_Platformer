using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] new private Camera camera;
    [SerializeField] private Rigidbody character;
    [SerializeField] private Animator animator;

    [Header("Animator Parameters")]
    [SerializeField] private string speedAnimParam = "Speed";
    [SerializeField] private string jumpAnimParam = "Jump";
    [SerializeField] private string groundedAnimParam = "Grounded";
    [SerializeField] private string freeFallAnimParam = "FreeFall";

    [Header("Input Names")]
    [SerializeField] private string moveXInputName = "Horizontal";
    [SerializeField] private string moveZInputName = "Vertical";
    [SerializeField] private string orbitYInputName = "Mouse X";
    [SerializeField] private string jumpInputName = "Jump";
    [SerializeField] private string runInputName = "Fire1";

    private Vector3 inputMove = Vector3.zero;
    private Vector3 smoothInputMove = Vector3.zero;
    private float inputOrbit = 0f;
    private float smoothInputOrbit = 0f;
    private bool inputJump = false;
    private bool inputRun = false;

    [Header("Movement and Orbit")]
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float inAirSpeedFactor = 0.5f;
    [SerializeField] private float lookAngularSpeed = 30f;
    [SerializeField] private float inputOrbitScale = 10f;
    [SerializeField] private float followSpeed = 4f;
    [SerializeField] private float turnAngularSpeed = 6f;
    [SerializeField] private float jumpImpulse = 10f;
    [SerializeField] private float movementLerpSpeed = 17.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckCenter = Vector3.down * 0.8f;
    [SerializeField] private float groundCheckRadius = 0.175f;

    private Vector3 cameraOffset;
    private bool jumpedThisFrame;
    private bool isGrounded;
    private bool isFreeFalling;
    private float speed;

    private void Awake()
    {
        cameraOffset = camera.transform.InverseTransformVector(character.position - camera.transform.position);
    }

    private void Update()
    {
        ReadInputs();
        UpdateAnimatorParameters();
        isGrounded = CheckGround();
        isFreeFalling = CheckFreeFalling();
    }

    private void ReadInputs()
    {
        inputMove.x = Input.GetAxis(moveXInputName);
        inputMove.z = Input.GetAxis(moveZInputName);
        inputMove = Vector3.ClampMagnitude(inputMove, 1);
        smoothInputMove = Vector3.Lerp(smoothInputMove, inputMove, Time.deltaTime * runSpeed);

        inputOrbit = Input.GetAxis(orbitYInputName) * inputOrbitScale;
        smoothInputOrbit = Mathf.Lerp(smoothInputOrbit, inputOrbit, Time.deltaTime * lookAngularSpeed);

        inputJump = Input.GetButtonDown(jumpInputName);

        inputRun = Input.GetButton(runInputName);
    }

    public void UpdateAnimatorParameters()
    {
        animator.SetFloat(speedAnimParam, speed);
        animator.SetBool(jumpAnimParam, jumpedThisFrame);
        animator.SetBool(groundedAnimParam, isGrounded);
        animator.SetBool(freeFallAnimParam, isFreeFalling);
    }

    private bool CheckGround()
    {
        return Physics.CheckSphere(character.position + groundCheckCenter, groundCheckRadius, groundLayer);
    }

    private bool CheckFreeFalling()
    {
        return Mathf.Abs(character.velocity.y) > runSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(character.position + groundCheckCenter, groundCheckRadius);
    }

    private void FixedUpdate()
    {
        Jump();
        Orbit(Time.fixedDeltaTime);
        Move(Time.fixedDeltaTime);
    }

    private void Jump()
    {
        // fix: if the player presses the jump key many times too quickly, this will trigger many jumps
        if(jumpedThisFrame = inputJump && isGrounded)
        {
            Vector3 vel = character.velocity;
            vel.y = 0;
            character.velocity = vel;
            character.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
        }
    }

    private void Orbit(float delta)
    {
        Vector3 position = character.position - camera.transform.TransformVector(cameraOffset);
        camera.transform.position = Vector3.Lerp(camera.transform.position, position, delta * followSpeed);
        camera.transform.RotateAround(character.position, Vector3.up, smoothInputOrbit);
    }

    private void Move(float delta)
    {
        float targetSpeed = smoothInputMove.magnitude * (inputRun ? runSpeed : walkSpeed);
        speed = Mathf.Lerp(speed, targetSpeed, delta * movementLerpSpeed);
        Debug.Log(speed);

        if(jumpedThisFrame)
        {
            Vector3 velocity = character.transform.forward * speed * inAirSpeedFactor;
            character.velocity += velocity;
            return;
        }

        if(inputMove == Vector3.zero)
            return;
        
        if(!isGrounded)
            delta *= inAirSpeedFactor;

        Vector3 direction = camera.transform.TransformVector(smoothInputMove);
        direction.y = 0;
        direction.Normalize();
        character.transform.forward = Vector3.Slerp(character.transform.forward, direction, turnAngularSpeed * delta);

        Vector3 position = character.position + character.transform.forward * speed * delta;
        character.MovePosition(position);
    }
}
