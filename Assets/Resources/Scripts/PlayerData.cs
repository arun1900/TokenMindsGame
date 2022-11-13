using UnityEngine;

namespace Resources.Scripts
{
    [CreateAssetMenu(fileName = "PlayerData",menuName = "Assets/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public float forwardSpeed = 0f;
        public float rotationSpeed = 0f;
        public float horizontalSpeed = 0f;
        public int playerCoins;
        public int playerHealth;
        public float radius;
        public float animationDuration = 0f;
    }
}
