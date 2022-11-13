using System;
using UnityEngine;

namespace Resources.Scripts.Tags
{
    public class ObstacleTag : MonoBehaviour
    {

        public static event Action<int> OnObstacleHit;

        [SerializeField] private int damage;

        private void OnTriggerExit(Collider other)
        {
            var playerTag = other.GetComponent<PlayerTag>();
            if (playerTag)
            {
                MakeDamageToPlayer();
            }
        }
        
        private void MakeDamageToPlayer()
        {
           OnObstacleHit?.Invoke(damage);
        }
    }
}