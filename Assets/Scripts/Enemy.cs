using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTask.Bullets;
using DevTask.Selection;

namespace DevTask.Enemies
{
    public class Enemy : MonoBehaviour, ISelectableRenderer
    {
        [Header("Main: ")]
        public Rigidbody Rigidbody;
        public MeshRenderer MeshRenderer;
        public GameObject ShootEffect;
        public Material Material;

        [Header("Gun: ")]
        public GameObject FireEffect;
        public GameObject Gun;
        public Transform FirePoint;

        private ObjectsPool _bulletsPool;

        [Header("Logic: ")]
        public float MotionSpeed = 5f;
        public float ChangeDirectionCooldown = 2f;
        private float _changeDirectionDelay;
        private bool _isMovingRight = false;

        public float AttackCooldown;
        private float _attackCooldown;

        private Player _player;
        private bool _isTrackingPlayer;
        private bool _isDead;

        //ISelectableRenderer
        public Transform GetTransform() => transform;
        public Renderer GetRenderer() => MeshRenderer;
        public Material GetDefaultMaterial() => Material;

        public bool IsDestroyed { get; private set; }
        private void OnDestroy()
        {
            IsDestroyed = true;
        }
        //End

        public void Start()
        {
            _player = FindObjectOfType<Player>(true);
            _bulletsPool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@EnemiesBulletsPool");
            GameRules.ChangeEnemiesCount(1);
        }

        public void FixedUpdate()
        {
            if (_isDead == true)
                return;

            if (MotionSpeed != 0)
            {
                if (_isMovingRight)
                {
                    Rigidbody.AddForce(new Vector3(MotionSpeed, 0, 0) * Time.fixedDeltaTime, ForceMode.VelocityChange);
                }
                else
                {
                    Rigidbody.AddForce(new Vector3(-MotionSpeed, 0, 0) * Time.fixedDeltaTime, ForceMode.VelocityChange);
                }

                _changeDirectionDelay -= Time.fixedDeltaTime;
                if (_changeDirectionDelay <= 0)
                {
                    ChangeDirection();
                }
            }
        }

        public void Update()
        {
            if (_isDead == true)
                return;

            _isTrackingPlayer = false;
            Vector3 targetDirection = (_player.transform.position - FirePoint.transform.position).normalized;

            if (Physics.Raycast(FirePoint.transform.position, targetDirection, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    _isTrackingPlayer = true;
                }
            }

            _attackCooldown -= Time.deltaTime;
            if (_isTrackingPlayer)
            {
                Vector3 targetPosition = _player.transform.position;
                targetPosition.y = transform.position.y;

                transform.LookAt(targetPosition);

                if (_attackCooldown <= 0)
                {
                    _attackCooldown = AttackCooldown;
                    FireGun();
                }
            }
        }

        public void Die()
        {
            if (_isDead)
                return;

            GameRules.ChangeEnemiesCount(-1);
            GameRules.Log("Agent dying");

            Destroy(Gun.gameObject);
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            Rigidbody.freezeRotation = false;
            Rigidbody.AddRelativeTorque(-transform.forward * 100f, ForceMode.Impulse);

            _isDead = true;
            StartCoroutine(PlayDeathAnimationAndDestroy(2f));
        }

        private IEnumerator PlayDeathAnimationAndDestroy(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            GameObject shootEffect = ShootEffect;
            shootEffect = Instantiate(shootEffect);
            shootEffect.transform.position = transform.position;

            Destroy(gameObject);
        }

        private void FireGun()
        {
            GameObject fireEffect = FireEffect;
            fireEffect = Instantiate(fireEffect, FirePoint);
            fireEffect.transform.localPosition = Vector3.zero;
            fireEffect.transform.forward = FirePoint.forward;

            Vector3 direction = FirePoint.forward;

            Bullet bullet = (Bullet)_bulletsPool.Create<Bullet>();
            bullet.transform.position = FirePoint.transform.position;
            bullet.Fire(FirePoint.forward);
        }

        public void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            if (contact.otherCollider.transform.CompareTag("Obstacle"))
            {
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            _changeDirectionDelay = ChangeDirectionCooldown;
            _isMovingRight = !_isMovingRight;
        }
    }
}