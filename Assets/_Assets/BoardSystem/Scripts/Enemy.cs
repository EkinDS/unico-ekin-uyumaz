using UnityEngine;

namespace _Assets.BoardSystem
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private float speed;

        private Board board;
        private bool isAdvancing;

    
        public void Initialize(Board newBoard)
        {
            board = newBoard;
        }


        public void StartAdvancing()
        {
            isAdvancing = true;

        }


        private void StopAdvancing()
        {
            isAdvancing = false;
        }
    
    
        public void GetDamaged(float damage)
        {
            health -= damage;

            if (health <= 0F)
            {
                Die();
            }
        }


        public float GetSpeed()
        {
            return speed;
        }
    
    
        private void Update()
        {
            if (isAdvancing)
            {
                Advance();
            }
        }

    
        private void Advance()
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            if (transform.position.z <= 0F)
            {
                DefeatPlayer();
            }
        }


        private void DefeatPlayer()
        {
            BoardEventsHandler.RaisePlayerFailed();
        }

    
        private void Die()
        {
            DestroyImmediate(gameObject);

            BoardEventsHandler.RaiseEnemyDied(this);
        }
    
        
        private void OnEnable()
        {
            BoardEventsHandler.PlayerFailed += StopAdvancing;
        }


        private void OnDisable()
        {
            BoardEventsHandler.PlayerFailed -= StopAdvancing;
        }
    }
}
