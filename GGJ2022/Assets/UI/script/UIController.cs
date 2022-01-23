using UnityEditor;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIController
{
    public class UIController : MonoBehaviour
    {
        public GameObject btn_start;
        public GameObject btn_tujian;
        public GameObject btn_developer;
        public GameObject btn_exit;
        public GameObject btn_startGame;
        public GameObject btn_closeStartPanel;
        public GameObject btn_closeTuijianPanel;
        public GameObject btn_closeDeveloperPanel;
        public GameObject panel_start;
        public GameObject panel_tujian;
        public GameObject panel_developer;



        private void Start()
        {
            btn_start.GetComponent<Button>().onClick.AddListener(startGame);
            btn_tujian.GetComponent<Button>().onClick.AddListener(openTujianPanel);
            btn_developer.GetComponent<Button>().onClick.AddListener(openDeveloperPanel);
            btn_closeStartPanel.GetComponent<Button>().onClick.AddListener(closeStartPanel);
            btn_closeTuijianPanel.GetComponent<Button>().onClick.AddListener(closeTujianPanel);
            btn_closeDeveloperPanel.GetComponent<Button>().onClick.AddListener(closeDeveloperPanel);
            btn_exit.GetComponent<Button>().onClick.AddListener(ExitGame);
            btn_startGame.GetComponent<Button>().onClick.AddListener(startGame);


        }

        private void startGame()
        {
            SceneManager.LoadScene("MainGame");
        }
        private void openStartPanel()
        {
            panel_start.SetActive(true);
        }
        private void closeStartPanel()
        {
            panel_start.SetActive(false);
        }
        private void openTujianPanel()
        {
            panel_tujian.SetActive(true);
        }
        private void closeTujianPanel()
        {
            panel_tujian.SetActive(false);
        }
        private void openDeveloperPanel()
        {
            panel_developer.SetActive(true);
        }
        private void closeDeveloperPanel()
        {
            panel_developer.SetActive(false);
        }
        private void ExitGame()
        {
            //预处理
#if UNITY_EDITOR    //在编辑器模式下
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }


    }
}