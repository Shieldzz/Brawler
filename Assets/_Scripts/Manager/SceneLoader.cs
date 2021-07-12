using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BTA
{

    public class SceneLoader : MonoBehaviour
    {

        private bool m_created = false;
        private Scene m_currScene;

        private string m_sceneToLoad = "";
        public string SceneToLoad { get { string toReturn = m_sceneToLoad; m_sceneToLoad = ""; return toReturn; } }

        private AsyncOperation m_loadingSceneAsynOperation = null;

        private void Awake()
        {
            if (!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
            }

            GameState gState = GameManager.Instance.GetInstanceOf<GameState>();
            //gState.OnStateLaunchGame += LoadScenebyName;
            Debug.Log("Load MainMenu Event");
            gState.OnStateMainMenu += LoadMainMenu;
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void LoadScenebyName(string sceneName)
        {
            //Scene sceneToLoad = SceneManager.get(sceneName);
            //if (!sceneToLoad.IsValid())
            //{
            //    Debug.LogError("Cant find a scene named " + sceneName);
            //    return;
            //}
            LoadSceneWithLoadingScreen(sceneName);
            //SceneManager.LoadSceneAsync(sceneName);

        }

        private void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync("_Scenes/Proto/MainMenu_v2");
            m_loadingSceneAsynOperation = SceneManager.LoadSceneAsync("_Scenes/LoadingScene");
            m_loadingSceneAsynOperation.allowSceneActivation = false;
        }

        //private void LoadGameScene()
        //{
        //    int nextScene = 2;
        //    SceneManager.LoadSceneAsync(nextScene);
        //}

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadSceneWithLoadingScreen(string sceneName)
        {
            m_sceneToLoad = sceneName;

            StartCoroutine(ActiveLoadingScene());
        }

        IEnumerator ActiveLoadingScene()
        {
            while (!m_loadingSceneAsynOperation.isDone)
            {
                if (m_loadingSceneAsynOperation.progress > 0.89f)
                    m_loadingSceneAsynOperation.allowSceneActivation = true;

                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadingScene"));
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return new WaitForEndOfFrame();
        }
    }

} // namespace BTA
