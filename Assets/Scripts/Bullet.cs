using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public GameObject BreakingEffect;
    public float BulletSpeed;

    private float _lifeTime = 10f;

    public void Fire (Vector3 direction)
    {
        Rigidbody.velocity = direction * BulletSpeed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        TriggerCollision(collision);
        CreateBreakingEffect();
        Destroy(gameObject);
    }
    
    protected void CreateBreakingEffect ()
    {
        GameObject breakingEffect = BreakingEffect;
        breakingEffect = Instantiate(breakingEffect);
        breakingEffect.transform.position = transform.position;
    }

    protected abstract void TriggerCollision(Collider collision);

    public void FixedUpdate()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime < 0 || transform.position.y < -50f)
        {
            CreateBreakingEffect();
            Destroy(gameObject);
        }
    }
}