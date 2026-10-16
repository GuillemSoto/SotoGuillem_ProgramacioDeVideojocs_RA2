using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CanonRotation : MonoBehaviour, InputSystem_Actions.ICannonActions
{
    //public InputAction inputAction;
    public InputSystem_Actions inputActions;
    //public Vector3 _maxRotation;
    //public Vector3 _minRotation;
    public float maxRotation;
    public float minRotation;
    private float _offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float _initialScaleX;
    private bool isRaising = false;
    [SerializeField] private float _multiplier = 10f;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        //inputAction = new InputAction();
        _initialScaleX = PotencyBar.transform.localScale.x;
        inputActions.Cannon.SetCallbacks(this);
        
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2 direction = (Vector2)mousePos - (Vector2)transform.position;
        var ang = Mathf.Clamp(Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg + _offset, minRotation, maxRotation);
        transform.rotation = Quaternion.Euler(0, 0, ang);
    }
    void Update()
    {
        
        if (isRaising && ProjectileSpeed < MaxSpeed)
        {
            ProjectileSpeed = Time.deltaTime * _multiplier + ProjectileSpeed; //acotar entre dos valors (mirar variables)
            CalculateBarScale();
        }
        
        CalculateBarScale();

    }
    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, _initialScaleX, ProjectileSpeed / MaxSpeed), transform.localScale.y,transform.localScale.z);
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRaising = true;
        }
        if (context.canceled)
        {
            var projectile = Instantiate(Bullet, ShootPoint.transform.position, transform.rotation); //canviar la posició on s'instancia
            projectile.GetComponent<Rigidbody2D>().linearVelocity = transform.up * ProjectileSpeed;
            ProjectileSpeed = 0f;
            isRaising = false;
        }
    }
}
