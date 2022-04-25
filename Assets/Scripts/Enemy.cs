using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Main: ")]
    public Rigidbody Rigidbody;
    public MeshRenderer MeshRenderer;
    public GameObject ShootEffect;
    public Material Material;

    [Header("Gun: ")]
    public Bullet BulletPrefab;
    public GameObject FireEffect;
    public GameObject Gun;
    public Transform FirePoint;

    [Header("Logic: ")]
    public float MotionSpeed = 5f;
    public float ChangeDirectionCooldown = 2f;
    private float _changeDirectionDelay;
    private bool _isMovingRight = false;

    public float AttackCooldown;
    private float _attackCooldown;

    private bool _isTrackingPlayer;
    private bool _isDead;

    public void Start()
    {
        MeshRenderer.material = new Material(Material);
    }

    public void FixedUpdate()
    {
        if (_isDead == true)
            return;

        if(MotionSpeed != 0)
        {
            if(_isMovingRight)
            {
                Rigidbody.AddForce(new Vector3(MotionSpeed, 0, 0) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
            else
            {
                Rigidbody.AddForce(new Vector3(-MotionSpeed, 0, 0) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }

            _changeDirectionDelay -= Time.fixedDeltaTime;
            if(_changeDirectionDelay <= 0)
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
        Vector3 targetDirection = (Player.Instance.transform.position - FirePoint.transform.position).normalized;

        if (Physics.Raycast(FirePoint.transform.position, targetDirection, out RaycastHit hit))
        {
            if (hit.transform.tag.Equals("Player"))
            {
                _isTrackingPlayer = true;
            }
        }

        _attackCooldown -= Time.deltaTime;
        if (_isTrackingPlayer)
        {
            Vector3 targetPosition = Player.Instance.transform.position;
            targetPosition.y = transform.position.y;

            transform.LookAt(targetPosition);

            if (_attackCooldown <= 0)
            {
                _attackCooldown = AttackCooldown;
                FireGun();
            }
        }
    }

    public void Die ()
    {
        if (_isDead)
            return;

        GameRules.Log("Agent dying");

        Destroy(Gun.gameObject);
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        Rigidbody.freezeRotation = false;
        Rigidbody.AddRelativeTorque(-transform.forward * 100f, ForceMode.Impulse);

        _isDead = true;
        Invoke(nameof(CreateDeathEffect), 1.9f);
        Destroy(gameObject, 2f);
    }

    private void CreateDeathEffect ()
    {
        GameObject shootEffect = ShootEffect;
        shootEffect = Instantiate(shootEffect);
        shootEffect.transform.position = transform.position;
    }

    private void FireGun()
    {
        GameObject fireEffect = FireEffect;
        fireEffect = Instantiate(fireEffect, FirePoint);
        fireEffect.transform.localPosition = Vector3.zero;
        fireEffect.transform.forward = FirePoint.forward;
        
        Vector3 direction = FirePoint.forward;

        Bullet bullet = Instantiate(BulletPrefab);
        bullet.transform.position = FirePoint.transform.position;
        bullet.Fire(FirePoint.forward);
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (contact.otherCollider.transform.tag.Equals("Obstacle"))
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection ()
    {
        _changeDirectionDelay = ChangeDirectionCooldown;
        _isMovingRight = !_isMovingRight;
    }

    public void SelectEnemy()
    {
        MeshRenderer.material.color = Color.yellow;
    }

    public void DeselectEnemy()
    {
        MeshRenderer.material.color = Color.red;
    }
}