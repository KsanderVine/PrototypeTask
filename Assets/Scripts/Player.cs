using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Physics: ")]
    public Rigidbody Rigidbody;
    public float PlayerMotionSpeed;

    [Header("Gun & Bullet: ")]
    public Transform CameraTransform;
    public Bullet BulletPrefab;
    public GameObject FireEffect;
    public Transform FirePoint;

    [SerializeField]
    private AttackType _attackType;
    private float _holdAttackDelay;
    private float _holdAttackCooldown = 0.35f;

    private float _sensitivityX = 1000f;
    private float _sensitivityY = 1000f;

    private float _cameraRotationX = 0f;
    private float _playerRotationY = 0f;

    private Enemy _selectedEnemy;

    public enum AttackType
    {
        MouseHold,
        MouseClick
    }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        _cameraRotationX = CameraTransform.localRotation.eulerAngles.x;
        _playerRotationY = transform.localRotation.eulerAngles.y;
    }

    public void FixedUpdate()
    {
        if (GameRules.IsRestarting)
            return;

        Vector3 direction = Vector3.zero;

        if(Input.GetKey(KeyCode.W))
        {
            direction.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }

        Rigidbody.AddRelativeForce(direction * PlayerMotionSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    public void Update()
    {
        if (GameRules.IsRestarting)
            return;

        if (transform.position.y < -3f)
        {
            GameRules.RestartLevel(false);
        }

        if (_selectedEnemy != null)
        {
            _selectedEnemy.DeselectEnemy();
            _selectedEnemy = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        Debug.DrawRay(transform.position, ray.direction * 1000f, Color.yellow, 0.1f);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _selectedEnemy = hit.transform.GetComponent<Enemy>();
                _selectedEnemy.SelectEnemy();
            }
        }

        float speed = .1f;
    #if UNITY_EDITOR
        speed = 1f;
    #endif

        float mouseAxisX = Input.GetAxis("Mouse X") * _sensitivityX * speed * Time.deltaTime;
        float mouseAxisY = Input.GetAxis("Mouse Y") * _sensitivityY * speed * Time.deltaTime;

        _cameraRotationX += mouseAxisY * -1;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -60f, 60f);

        Quaternion cameraRotation = Quaternion.Euler(_cameraRotationX, 0, 0.0f);
        CameraTransform.localRotation = cameraRotation;

        _playerRotationY += mouseAxisX;
        Quaternion playerRotation = Quaternion.Euler(0, _playerRotationY, 0.0f);
        transform.localRotation = playerRotation;

        if (_attackType == AttackType.MouseClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireGun();
            }
        }
        else
        {
            _holdAttackDelay -= Time.deltaTime;
            if (_holdAttackDelay < 0)
            {
                if (Input.GetMouseButton(0))
                {
                    _holdAttackDelay = _holdAttackCooldown;
                    FireGun();
                }
            }
        }
    }

    private void FireGun()
    {
        GameRules.Log("Player shooting");

        GameObject fireEffect = FireEffect;
        fireEffect = Instantiate(fireEffect, FirePoint);
        fireEffect.transform.localPosition = Vector3.zero;
        fireEffect.transform.forward = FirePoint.forward;

        float directionX = _cameraRotationX;
        float directionY = _playerRotationY;
        Vector3 direction = new Vector3(directionX, directionY, 0);

        Bullet bullet = Instantiate(BulletPrefab);
        bullet.transform.position = FirePoint.transform.position;
        bullet.Fire(FirePoint.forward);
    }

    public void PushVelocityImpulse (Vector3 velocity)
    {
        GameRules.Log("Player thrown back");
        Rigidbody.AddForce(velocity, ForceMode.Impulse);
    }
}
