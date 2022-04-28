using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTask.Bullets;
using DevTask.Selection;
using DevTask.Enemies.EnemyAI;

namespace DevTask.Enemies
{
    public class Enemy : MonoBehaviour, ISelectableRenderer
    {
        [Header("Main: ")]
        public Rigidbody Rigidbody;
        public MeshRenderer MeshRenderer;
        public Material Material;

        [Header("Logic: ")]
        private IAttack _attack;
        private IMotion _motion;
        
        private Player _player;
        private bool _isDead;
        private GameRules _gameRules;
        private ObjectsPool _deathPool;

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

        public void Awake()
        {
            _gameRules = FindObjectOfType<GameRules>();
            _player = FindObjectOfType<Player>(true);
            
            _deathPool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@EnemiesDeathPool");

            _attack = GetComponent<IAttack>();
            _motion = GetComponent<IMotion>();
        }

        public void Start()
        {
            _gameRules.ChangeEnemiesCount(1);
        }

        public void FixedUpdate()
        {
            if (_isDead == true)
                return;

            if (_motion != null)
                _motion.Move(Rigidbody, Time.fixedDeltaTime);
        }

        public void Update()
        {
            if (_isDead == true)
                return;

            if (_attack == null)
                return;

            if(_attack.IsTrackable(_player.transform))
            {
                _attack.Attack(_player.transform, Time.deltaTime);
            }
        }

        public void Die()
        {
            if (_isDead)
                return;

            _gameRules.ChangeEnemiesCount(-1);
            GameRules.GameLog.Log("Agent dying");

            if (_motion != null)
                _motion.OnDeath();

            if (_attack != null)
                _attack.OnDeath();

            gameObject.layer = LayerMask.NameToLayer("Ghost");
            Rigidbody.freezeRotation = false;
            Rigidbody.AddRelativeTorque(-transform.forward * 100f, ForceMode.Impulse);

            _isDead = true;
            StartCoroutine(PlayDeathAnimationAndDestroy(2f));
        }

        private IEnumerator PlayDeathAnimationAndDestroy(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            GameObject deathEffect = _deathPool.Create<IPoolable>().GameObject;
            deathEffect.transform.position = transform.position;

            Destroy(gameObject);
        }
    }
}