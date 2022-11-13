using UnityEngine;

namespace Resources.Scripts.Tags
{
    public class CoinTag : MonoBehaviour
    {
        
        public delegate void CoinTriggerAction();
        public static event CoinTriggerAction OnCoinTriggered;
        private void OnTriggerEnter(Collider other)
        {
            var playerTag = other.GetComponent<PlayerTag>();
            if (!playerTag) return;
            gameObject.SetActive(false);
            OnCoinTriggered?.Invoke();
            Invoke(nameof(EnableCoin),2f);
        }

        private void EnableCoin()
        {
            gameObject.SetActive(true);
        }
    }
}
