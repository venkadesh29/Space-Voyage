using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander Instance { get; private set; }

    public Rigidbody2D rigidbody2D = new Rigidbody2D();
    [SerializeField] float thrust = 700f;
    [SerializeField] float turnSpeed = 100f;
    [SerializeField] float softMagnitude = 4f;
    [SerializeField] float minDotVector = 0.9f;
    [SerializeField] float magnitude;

    [SerializeField] Transform savePoint;

    [SerializeField] private float fuelAmount;
    [SerializeField] private float fuelAmountMax = 10f;
    [SerializeField] private float _fuelConsumption = 1f;
    [SerializeField] private float _fuelRefill = 10f;

    [SerializeField] private const float GRAVITY_NORMAL = 0.7F;
    public const float _GRAVITY = 0F;

    [SerializeField] private bool mouseLockMode = false;
    [SerializeField] private bool fuel = false;

    public event EventHandler on_up_Force;
    public event EventHandler on_Before_Force;
    public event EventHandler<OnStateChangedEventArgs> on_State_Changed;
    public enum LanderState
    {
        WaitingToStart,
        Started,
        GameOver
    }

    public class OnStateChangedEventArgs : EventArgs
    {
        public LanderState landerState;
    }

    public event EventHandler<OnLandingEventArgs> on_Landing;

    public class OnLandingEventArgs : EventArgs
    {
        public LandingType landingType;
    }
    public enum LandingType
    {
        Safe,
        Hard,
        TooSteep,
    }

    private LanderState landerState;

    public Transform _savePoint { get => savePoint; set => savePoint = value; }
    private void Awake()
    {
        Instance = this;

        fuelAmount = fuelAmountMax;

        GetComponenets();
    }
    private void Start()
    {
        MouseLockMode();
        rigidbody2D.gravityScale = _GRAVITY;
        landerState = LanderState.WaitingToStart;
        LandingType landingType = LandingType.Safe;
        on_State_Changed?.Invoke(this, new OnStateChangedEventArgs { landerState = landerState });
    }
    private void MouseLockMode()
    {
        Cursor.lockState = mouseLockMode ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        on_Before_Force?.Invoke(this, EventArgs.Empty);

        switch (landerState)
        {
            case LanderState.WaitingToStart:
                {
                    if (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed ||
                        Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                    {
                        rigidbody2D.gravityScale = GRAVITY_NORMAL;
                        OnStateChange(LanderState.Started);
                    }
                }
                break;
            case LanderState.Started:
                {
                    if (fuelAmount > 0)
                        Movement();
                }
                break;
            case LanderState.GameOver:
                this.rigidbody2D.gravityScale = _GRAVITY;
                break;
        }
    }

    private void GetComponenets()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Movement()
    {
        if (fuelAmount <= 0) return;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            rigidbody2D.AddForce(thrust * transform.up * Time.deltaTime);
            on_up_Force?.Invoke(this, EventArgs.Empty);
            FuelConsumption();
        }

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            rigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            rigidbody2D.AddTorque(-turnSpeed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Surronding surronding))
        {
            if (collision.relativeVelocity.magnitude > softMagnitude)
            {
                magnitude = collision.relativeVelocity.magnitude;
                OnStateChange(LanderState.GameOver);
                on_Landing?.Invoke(this, new OnLandingEventArgs { landingType = LandingType.Hard });
                Debug.Log("Hard landing!");

            }

            float dotVector = Vector2.Dot(Vector2.up, transform.up);

            if (dotVector < minDotVector)
            {
                OnStateChange(LanderState.GameOver);
                on_Landing?.Invoke(this, new OnLandingEventArgs { landingType = LandingType.TooSteep });
                Debug.Log("Too Steep");
            }
        }
    }

    public void OnStateChange(LanderState landerState)
    {
        this.landerState = landerState;
        on_State_Changed?.Invoke(this, new OnStateChangedEventArgs { landerState = landerState });
    }

    private void FuelConsumption()
    {
        if (fuel)
            fuelAmount -= _fuelConsumption * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickUp fuelPickUp))
        {
            fuelAmount += _fuelRefill;
            fuelPickUp.DestroyFuel();

            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;

            }
        }
    }

    public float FuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    public void ResetLander(Transform savePoint_Position)
    {
        savePoint = savePoint_Position;
    }
}
