using UnityEngine;

namespace Resources.Scripts
{
    public class PlayerTag : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collided with " + collision.gameObject.name);
        }

    }
}
