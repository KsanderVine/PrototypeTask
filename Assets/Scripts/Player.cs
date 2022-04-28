using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTask.Enemies;
using DevTask.Bullets;

namespace DevTask
{
    public class Player : MonoBehaviour
    {
        [Header("Physics: ")]
        public Rigidbody Rigidbody;
        public float PlayerMotionSpeed;

        [Header("Gun & Bullet: ")]
        public Camera PlayerCamera;
        public Transform CameraTransform;
        public Transform FirePoint;

        private ObjectsPool _bulletsPool;
        private ObjectsPool _gunFirePool;

        [SerializeField]
        private AttackType _attackType;
        private float _holdAttackDelay;
        private float _holdAttackCooldown = 0.35f;

        private float _sensitivityX = 1000f;
        private float _sensitivityY = 1000f;

        private float _cameraRotationX = 0f;
        private float _playerRotationY = 0f;

        private GameRules _gameRules;

        public enum AttackType
        {
            MouseHold,
            MouseClick
        }

        public void Awake()
        {
            _gameRules = FindObjectOfType<GameRules>();
            _bulletsPool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@PlayerBulletsPool");
            _gunFirePool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@GunFirePool");
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

            if (Input.GetKey(KeyCode.W))
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
                _gameRules.GetLevelResultsAndReload(false);
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
            GameRules.GameLog.Log("Player shooting");

            GameObject fireEffect = _gunFirePool.Create().GameObject;
            fireEffect.transform.position = FirePoint.transform.position;
            fireEffect.transform.forward = FirePoint.forward;

            float directionX = _cameraRotationX;
            float directionY = _playerRotationY;
            Vector3 direction = new Vector3(directionX, directionY, 0);

            Bullet bullet = (Bullet)_bulletsPool.Create<Bullet>();
            bullet.transform.position = FirePoint.transform.position;
            bullet.Fire(FirePoint.forward);
        }

        public void PushVelocityImpulse(Vector3 velocity)
        {
            GameRules.GameLog.Log("Player thrown back");
            Rigidbody.AddForce(velocity, ForceMode.Impulse);
        }
    }
}