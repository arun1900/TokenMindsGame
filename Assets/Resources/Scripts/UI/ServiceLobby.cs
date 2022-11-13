using System;
using Resources.Scripts.Tags;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Resources.Scripts.UI
{
    public class ServiceLobby : MonoSingletonGeneric<ServiceLobby>
            {
                public delegate void ResetPlayerStatsAction();
                public static event ResetPlayerStatsAction OnResetTriggered;
                
                public WindowTag[] windows;
                
                public GameObject[] hearts;

                public Text levelInput;
                // Start is called before the first frame update
                void Start()
                {
                    UpdateLevelUI();
                }

                // Update is called once per frame
                void Update()
                {
                
                }

                private void UpdateLevelUI()
                {
                    levelInput.text = (PlayerPrefs.GetInt("level") + 1).ToString();
                }
                

                public void AppearWindow(WindowType type)
                {
                    OnResetTriggered?.Invoke();
                    foreach (var item in windows)
                    {
                        if (type == item.windowType)
                        {
                            item.gameObject.SetActive(true);
                        }
                    }
                }
                
                public void CloseWindow(WindowType type)
                {
                    foreach (var item in windows)
                    {
                        if (type == item.windowType)
                        {
                            item.gameObject.SetActive(false);
                        }
                    }
                }
                public void QuitApplication()
                {
        #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
        #else
				        Application.Quit();
        #endif
                }
                
          
              
                public void LoadNextLevel()
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                public void UpdateHealthUI(int heartIndex)
                {
                    hearts[heartIndex].SetActive(false);
                }
            }

    public enum WindowType
    {
        GameOver,
        NextLevel,
        TapToPlay
        
    }
}
