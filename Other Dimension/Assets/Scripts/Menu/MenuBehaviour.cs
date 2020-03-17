using System;
using Terrain;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuBehaviour : MonoBehaviour
    {
        [SerializeField] private ObjectCreation _objectCreator;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _pauseMenu;
        public bool GameActive { get; set; }

        private void Awake()
        {
            _pauseMenu.SetActive(false);
            _mainMenu.SetActive(true);
            Time.timeScale = 1f;
            _mainCamera.farClipPlane = 10000;
        }

        public void BeginGame()
        {
            //Cursor.visible = false;
            _mainMenu.SetActive(false);
            GameActive = true;
            _mainCamera.farClipPlane = 4000;
            if (_objectCreator) StartCoroutine(_objectCreator.CreateObjects());
        }

        public void LoadAiDemo()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public void LoadMainGame()
        {
            SceneManager.LoadSceneAsync(0);
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            _pauseMenu.SetActive(false);
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(0);
        }

        public void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}
