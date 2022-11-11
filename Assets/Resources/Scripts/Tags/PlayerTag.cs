using System;
using UnityEngine;

namespace Resources.Scripts
{
    public class PlayerTag : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collided with " + collision.gameObject.name);
        }

        private void OnTriggerExit(Collider other)
        {
            var obstacle = other.GetComponent<ObstacleTag>();
            if (obstacle)
            {
                TakeDamage();
            }
        
        }

        private void TakeDamage()
        {
            Debug.Log("Taking Damage");
        }
    }
}
