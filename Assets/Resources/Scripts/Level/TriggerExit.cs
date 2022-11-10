using System.Collections;
using UnityEngine;

namespace Resources.Scripts.Level
{
    public class TriggerExit : MonoBehaviour
    {
        public float delay = 5f;
    
        public delegate void ExitAction();
        public static event ExitAction OnChunkExited;

        private bool _exited;

        private void OnTriggerExit(Collider other)
        {
            var playerTag = other.GetComponent<PlayerTag>();
            if (playerTag != null)
            {
                if (!_exited)
                {
                    _exited = true;
                    OnChunkExited?.Invoke();
                    StartCoroutine(WaitAndDeactivate());
                }


            }
        }

        IEnumerator WaitAndDeactivate()
        {
            yield return new WaitForSeconds(delay);

            transform.root.gameObject.SetActive(false);

        }



    }
}
