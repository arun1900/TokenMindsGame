using System.Collections;
using Resources.Scripts.Tags;
using Resources.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("---------Movement Modifiers--------")]
        public Transform player;

        [SerializeField] private PlayerData playerData;
        [SerializeField] private Text playerCoinsField;
        public int playerCoins;
        public float animDuration;
        public float xMin = 0.15f, xMax = 0.85f;
        public Transform target;

        private Touch _touch;
        private Vector3 _targetPos;
        private Vector3 _touchOrigin = -Vector3.one;
        private float _horizontal = 0; //Used to store the horizontal move direction.
        private Vector3 _touchEnd;
        private Camera _mainCam;    
        private float _radius;

    
        private bool _isLevelStarted;
        private bool _isLevelEnded;
        private int _currentLevel = 1;

        [field: Header("Settings")] 
        private float HorizontalSpeed { get; set; }
        public float ForwardSpeed { get; set; }
        private float RotationSpeed { get; set; }
    
        private void Awake()
        {
            InitializeData();
            _currentLevel = PlayerPrefs.GetInt("level");

        }

        private void OnEnable()
        {
            CoinTag.OnCoinTriggered += IncreasePlayerCoins;
            ObstacleTag.OnObstacleHit += DecreaseHealth;
            ServiceLobby.OnResetTriggered += ResetPlayerStats;
        }

        private void InitializeData()
        {
            this.ForwardSpeed = playerData.forwardSpeed;
            this.RotationSpeed = playerData.rotationSpeed;
            this.playerCoins = playerData.playerCoins;
            this._radius = playerData.radius;
            this.HorizontalSpeed = playerData.horizontalSpeed;
            this.animDuration = playerData.animationDuration;
        }

        private void Start()
        {
            if (target == null)
            {
                target = this.transform;
            }

            _mainCam = Camera.main;
            _targetPos = target.position;
            // GameManager.Instance.OnLevelEnd += SetLevelEnd;
            // GameManager.Instance.OnLevelStart += SetLevelStart;
        }
        private void Update()
        {
            if ( !_isLevelStarted && Input.touchCount > 0)
            {
                _isLevelStarted = true;
                ServiceLobby.Instance.CloseWindow(WindowType.TapToPlay);
            }
            
            if (!_isLevelStarted) return;
            if (Input.touchCount == 1)
            {
                _touch = Input.GetTouch(0);
                if (_touch.phase == TouchPhase.Began)
                {
                    _touchOrigin = _touch.deltaPosition;
                }

                else if (_touch.phase == TouchPhase.Moved)
                {
                    _touchEnd = _touch.deltaPosition;

                    var position = target.position;
                    var movePos = new Vector3(
                        position.x + _touch.deltaPosition.x * HorizontalSpeed * 1 * Time.deltaTime,
                        position.y,
                        position.z);

                    float distanceX = movePos.x - _targetPos.x;

                    if (distanceX < _radius)
                    {
                        target.position = movePos;
                    }

                    ClampXPosition();
                    CalculateInputs();
                }
                else if (_touch.phase == TouchPhase.Ended && _touchOrigin.x >= 0)
                {
                    _touchEnd = _touch.deltaPosition;
                    CalculateInputs();
                }
                else
                {
                    _touchOrigin = Vector3.zero;
                    _touchEnd = Vector3.zero;
                }
            }
            else
            {
                _horizontal = 0;
            }
            // player.DOLocalRotate(
            //     Mathf.Abs(_horizontal) < 1 ? Vector3.zero : new Vector3(0, _horizontal * RotationSpeed, 0), .3f);
          
            target.Translate(transform.forward * ForwardSpeed * Time.deltaTime);
        }

        private void ClampXPosition()
        {
            var pos = _mainCam.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.x = Mathf.Clamp(pos.x, xMin, xMax);
            target.position = _mainCam.ViewportToWorldPoint(pos);
        }

        private void IncreasePlayerCoins()
        {
            StartCoroutine(StartIncreasing(2));
        }

        private IEnumerator StartIncreasing(int coins)
        {
            var wait = new WaitForSeconds(0.2f);
            for (var i = coins; i > 0; i--)
            {
                playerData.playerCoins += 1;
                playerCoinsField.text = playerData.playerCoins.ToString();
                if (playerData.playerCoins >= 100)
                {
                    ServiceLobby.Instance.AppearWindow(WindowType.NextLevel);
                    _currentLevel++;
                    PlayerPrefs.SetInt("level",_currentLevel);

                    this.enabled = false;
                    // playerData.playerCoins = 0;
                    // playerData.playerHealth = 3;
                    
                }
                yield return wait;
                
            }
            yield return null;
        }

        private void CalculateInputs()
        {
            var x = _touchEnd.x - _touchOrigin.x;

            //Calculate the difference between the beginning and end of the touch on the y axis.
            var y = _touchEnd.y - _touchOrigin.y;

            //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
            _touchOrigin.x = -1;
            // Debug.Log(x);
            //Check if the difference along the x axis is greater than the difference along the y axis.
            if (Mathf.Abs(x) > 10f)
            {
                //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                _horizontal = x > 0 ? 1 : -1;
            }
            else
            {
                //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                // _vertical = y > 0 ? 1 : -1;
                _horizontal = 0;
                // _touchEnd = Vector3.zero;
                // _touchOrigin = Vector3.zero;
                // _horizontalInput = 0f;
            }
        }

      
#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            playerData.playerCoins = 0;
            playerData.playerHealth = 3;
        }
        
#endif
        private void ResetPlayerStats()
        {
            playerData.playerCoins = 0;
            playerData.playerHealth = 3;
        }
        
        private void OnDisable()
        {
            CoinTag.OnCoinTriggered -= IncreasePlayerCoins;
            ObstacleTag.OnObstacleHit -= DecreaseHealth;
            ServiceLobby.OnResetTriggered -= ResetPlayerStats;
            // GameManager.Instance.OnLevelEnd -= SetLevelEnd;
            // GameManager.Instance.OnLevelStart -= SetLevelStart;
        }




        private void SetLevelEnd(bool status) => _isLevelEnded = status;
        private void SetLevelStart(bool status) => _isLevelStarted = status;

        private void DecreaseHealth(int dmg)
        {
            playerData.playerHealth -= dmg;
            ServiceLobby.Instance.UpdateHealthUI(playerData.playerHealth);
            if (playerData.playerHealth <= 0)
            {
                ServiceLobby.Instance.AppearWindow(WindowType.GameOver);
                this.enabled = false;
            }
        }
    }
}