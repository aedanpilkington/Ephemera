using UnityEngine;

public class CaterpillarController : MonoBehaviour
{
    private Rigidbody rb;
    private CaterpillarFollow follow;

    #region Camera

    public Camera playerCamera;
    public Vector3 cameraOffset = new Vector3(0f, 2f, -6f);

    public float mouseSensitivity = 1.2f;
    public float maxLookAngle = 25f;

    public float defaultPitch = -10f;
    public float pitchReturnSpeed = 6f;
    public float pitchResetDelay = 0.25f;

    private float yaw;
    private float pitch;
    private float lastMouseYTime;
    private Vector3 defaultCameraOffset;

    #endregion

    #region Movement / Weight

    public float baseMoveSpeed = 8f;

    // how slow you get when REALLY heavy
    public float minMoveSpeed = 2.2f;

    // how many apples until you feel "fully heavy"
    public int applesForMaxWeight = 20;

    // how aggressive the slowdown is (higher = harsher)
    public float weightPenaltyExponent = 2f;

    private float currentMoveSpeed;
    private bool isGrounded;

    #endregion

    #region Charged Jump

    public KeyCode jumpKey = KeyCode.Space;

    public float minJumpForce = 5f;
    public float maxJumpForce = 10f;

    public float maxChargeTime = 1.2f;
    public float minChargePercent = 0.35f;
    public int maxApplesForJump = 3;

    public float baseForwardJumpForce = 6f;

    private float chargeTimer;
    private bool charging;

    #endregion

    #region Crouch / Size Gating

    public bool enableCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float crouchCameraDrop = 0.4f;
    public float crouchBodyDrop = 0.25f;
    public float crouchSpeedMultiplier = 0.5f;

    public int maxApplesToFit = 6;

    private bool isCrouched;

    #endregion

    #region Body Sway

    public bool enableBodySway = true;
    public Transform joint;

    public float swaySpeed = 8f;
    public Vector3 swayAmount = new Vector3(0.15f, 0.05f, 0f);

    private Vector3 defaultJointOffset;
    private float swayTimer;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        follow = GetComponentInChildren<CaterpillarFollow>();

        currentMoveSpeed = baseMoveSpeed;
        pitch = defaultPitch;
        defaultCameraOffset = cameraOffset;

        if (joint != null)
            defaultJointOffset = joint.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleCamera();
        HandleJump();
        HandleCrouch();
        CheckGround();

        if (enableBodySway)
            HandleBodySway();
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        Vector3 move =
            transform.TransformDirection(input.normalized) * currentMoveSpeed;

        Vector3 velocityChange = move - rb.velocity;
        velocityChange.y = 0f;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    #region Camera

    private void HandleCamera()
    {
        if (playerCamera == null) return;

        float mx = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        yaw += mx;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (Mathf.Abs(my) > 0.01f)
        {
            pitch -= my;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
            lastMouseYTime = Time.time;
        }
        else if (Time.time - lastMouseYTime > pitchResetDelay)
        {
            pitch = Mathf.Lerp(pitch, defaultPitch, pitchReturnSpeed * Time.deltaTime);
        }

        Vector3 offset =
            Quaternion.Euler(pitch, 0f, 0f) *
            (transform.rotation * cameraOffset);

        playerCamera.transform.position = transform.position + offset;
        playerCamera.transform.LookAt(transform.position + Vector3.up * 1.2f);
    }

    #endregion

    #region Jump

    private void HandleJump()
    {
        if (!isGrounded) return;

        if (Input.GetKeyDown(jumpKey))
        {
            chargeTimer = 0f;
            charging = true;
        }

        if (Input.GetKey(jumpKey) && charging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeTime);
        }

        if (Input.GetKeyUp(jumpKey) && charging)
        {
            float chargePercent = chargeTimer / maxChargeTime;
            bool chargedEnough = chargePercent >= minChargePercent;

            int applesNeeded =
                Mathf.CeilToInt(chargePercent * maxApplesForJump);

            bool paid = false;

            if (chargedEnough && AppleManager.Instance != null)
                paid = AppleManager.Instance.SpendApples(applesNeeded);

            float jumpForce = paid
                ? Mathf.Lerp(minJumpForce, maxJumpForce, chargePercent)
                : minJumpForce;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // forward jump scaled by weight
            int apples = AppleManager.Instance != null
                ? AppleManager.Instance.appleCount
                : 0;

            float weightFactor = Mathf.Clamp01((float)apples / applesForMaxWeight);
            weightFactor = Mathf.Pow(weightFactor, weightPenaltyExponent);

            float forwardForce = Mathf.Lerp(
                baseForwardJumpForce,
                1.2f,
                weightFactor
            );

            rb.AddForce(transform.forward * forwardForce, ForceMode.Impulse);

            if (paid && follow != null)
                follow.RemoveSegments(applesNeeded);

            RecalculateSpeed();

            charging = false;
            isGrounded = false;
        }
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.down * 0.6f;
        isGrounded = Physics.Raycast(origin, Vector3.down, 0.75f);
    }

    #endregion

    #region Speed / Weight

    public void RecalculateSpeed()
    {
        int apples =
            AppleManager.Instance != null
            ? AppleManager.Instance.appleCount
            : 0;

        float weightFactor = Mathf.Clamp01((float)apples / applesForMaxWeight);
        weightFactor = Mathf.Pow(weightFactor, weightPenaltyExponent);

        currentMoveSpeed = Mathf.Lerp(
            baseMoveSpeed,
            minMoveSpeed,
            weightFactor
        );

        if (isCrouched)
            currentMoveSpeed *= crouchSpeedMultiplier;
    }

    public bool CanFitThroughSmallGap()
    {
        int apples =
            AppleManager.Instance != null
            ? AppleManager.Instance.appleCount
            : 0;

        return apples <= maxApplesToFit;
    }

    #endregion

    #region Crouch

    private void HandleCrouch()
    {
        if (!enableCrouch) return;

        if (Input.GetKeyDown(crouchKey))
            SetCrouch(true);

        if (Input.GetKeyUp(crouchKey))
            SetCrouch(false);
    }

    private void SetCrouch(bool crouch)
    {
        if (crouch == isCrouched) return;

        if (crouch)
        {
            cameraOffset = defaultCameraOffset + Vector3.down * crouchCameraDrop;

            if (joint != null)
                joint.localPosition = defaultJointOffset + Vector3.down * crouchBodyDrop;
        }
        else
        {
            cameraOffset = defaultCameraOffset;

            if (joint != null)
                joint.localPosition = defaultJointOffset;
        }

        isCrouched = crouch;
        RecalculateSpeed();
    }
    public bool IsCrouching()
    {  return isCrouched; }

    #endregion

    #region Body Sway

    private void HandleBodySway()
    {
        if (joint == null) return;

        if (isGrounded)
        {
            swayTimer += Time.deltaTime * swaySpeed;

            joint.localPosition = defaultJointOffset +
                new Vector3(
                    Mathf.Sin(swayTimer) * swayAmount.x,
                    Mathf.Sin(swayTimer) * swayAmount.y,
                    0f
                );
        }
        else
        {
            swayTimer = 0f;
            joint.localPosition = Vector3.Lerp(
                joint.localPosition,
                defaultJointOffset,
                Time.deltaTime * swaySpeed
            );
        }
    }

    #endregion
}
