using UnityEngine;

/// <summary>
/// Motion controller for protagonist game objects
/// Handles rigidbody motion by reading axis + button values from an input object
/// </summary>
/// <remarks>
/// TODO: abstract to a base class and inherit
/// </remarks>
[RequireComponent(typeof(Rigidbody2D))]
public class ProtagonistMotion : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private MonoBehaviour _InputBehavior;

    public IInput Input
    {
        get { return (IInput)_InputBehavior; }
        set { _InputBehavior = (MonoBehaviour)value; }
    }

    [SerializeField]
    private Rigidbody2D _Rigidbody;

    public Rigidbody2D Rigidbody
    {
        get { return _Rigidbody; }
        protected set { _Rigidbody = value; }
    }

    [SerializeField]
    private BoxCollider2D _PhysicalSupport;

    /// <summary>
    /// The trigger collider to sense whether the protagonist is
    /// physically supported (i.e. standing on a surface, grounded, etc)
    /// This is used primarily for jump logic
    /// </summary>
    public BoxCollider2D PhysicalSupport
    {
        get { return _PhysicalSupport; }
        protected set { _PhysicalSupport = value; }
    }

    [SerializeField]
    private LayerMask _PhysicalSupportLayerMask;

    public LayerMask PhysicalSupportLayerMask
    {
        get { return _PhysicalSupportLayerMask; }
        protected set { _PhysicalSupportLayerMask = value; }
    }

    /// <summary>
    /// Horizontal "running" speed
    /// </summary>
    [Range(0.1f, 10f)]
    public float HorizontalSpeed = 1f;

    /// <summary>
    /// Jump impulse velocity added to vertical speed
    /// when the protagonist jumps
    /// </summary>
    [Range(1f, 30f)]
    public float JumpStrength = 1f;

    #region Input Logic

    /// <summary>
    /// Logical values for handling input
    /// </summary>
    public struct InputLogic
    {
        public float Horizontal;
        public bool Jump, JumpHold;
    }

    public static InputLogic GetInputLogic(
        float horizontal,
        float vertical, // currently unused
        bool jump,
        bool jump_hold)
    {
        return new InputLogic()
        {
            Horizontal = horizontal,
            Jump = jump,
            JumpHold = jump_hold
        };
    }

    /// <summary>
    /// Gets the input logic by reading from an input object
    /// </summary>
    /// <param name="input">The input object to read from</param>
    /// <returns>Calculated input logic</returns>
    public static InputLogic GetInputLogic(IInput input)
    {
        return GetInputLogic(
            input.GetAxis("Horizontal"),
            input.GetAxis("Vertical"),
            input.GetButtonDown("Jump"),
            input.GetButton("Jump"));
    }

    #endregion Input Logic

    #region Status

    /// <summary>
    /// Values indicating the current status of the protagonist,
    /// such as whether the protagonist is physically suppported, etc.
    /// </summary>
    public struct Status
    {
        public bool IsPhysicallySupported;
    }

    /// <summary>
    /// Gets the current status of the protagonist
    /// </summary>
    /// <returns>Status object</returns>
    public Status GetStatus()
    {
        // Calculate whether the protagonist is currently physically
        // supported using a trigger collider overlap test
        var filter = new ContactFilter2D();
        filter.SetLayerMask(PhysicalSupportLayerMask);
        var results = new Collider2D[1];
        var number_of_physical_suports =
            PhysicalSupport.OverlapCollider(filter, results);

        return new Status()
        {
            // If there is at least one physical support,
            // then the protagonist is physically supported
            IsPhysicallySupported = number_of_physical_suports > 0
        };
    }

    #endregion Status

    /// <summary>
    /// Called once every physics update
    /// </summary>
    private void FixedUpdate()
    {
        if (Input != null)
        {
            var logic = GetInputLogic(Input);
            var status = GetStatus();
            // Get the rigidbody velocity
            var velocity = Rigidbody.velocity;
            velocity.x = HorizontalSpeed * logic.Horizontal;
            // Jumping is allowed if the protagonist is physically supported
            if (logic.Jump && status.IsPhysicallySupported)
                velocity.y += JumpStrength;
            // Set the rigidbody velocity
            Rigidbody.velocity = velocity;
        }
    }

    public void OnBeforeSerialize()
    {
        // Round-trip property validation
        Input = Input;

        // Serialize required components on this GameObject
        if (Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnAfterDeserialize()
    {
        // A vestigial organ. This method is rarely used and
        // the stub is required for ISerializationCallbackReceiver
    }
}