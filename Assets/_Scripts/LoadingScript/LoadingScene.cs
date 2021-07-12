using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BTA
{

    public class LoadingScene : MonoBehaviour
    {

        private string m_sceneToLoad;
        private Scene m_currentScene;
        private bool m_isSceneIsLoaded = false;
        private bool m_unloadLoadingScene = false;

        [SerializeField] private GameObject m_loadingTemplate = null;
        [SerializeField] private Slider m_truckSlider = null;
        [SerializeField] private float m_sliderDuration = 3f;

        private bool m_isLoad = false;
        private float m_sliderProgress = 0f;
        private float m_loadingProgress = 0f;

        private SceneTransition m_sceneTransition = null;

        void Start()
        {
            m_sceneToLoad = GameManager.Instance.GetInstanceOf<SceneLoader>().SceneToLoad;
            m_currentScene = SceneManager.GetActiveScene();
            m_sceneTransition = GetComponent<SceneTransition>();
            StartLoading();
        }

        void Update()
        {
            if (!m_isLoad)
            {
                m_sliderProgress += Time.deltaTime / m_sliderDuration;
                m_sliderProgress = Mathf.Clamp(m_sliderProgress, 0f, m_loadingProgress);
                m_truckSlider.value = Mathf.Lerp(0f, 1f, m_sliderProgress);
            }
        }

        private void StartLoading()
        {
            m_isSceneIsLoaded = false;
            m_unloadLoadingScene = false;

            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            AsyncOperation loading = SceneManager.LoadSceneAsync(m_sceneToLoad, LoadSceneMode.Additive);
            loading.allowSceneActivation = false;

            while(!loading.isDone)
            {
                m_loadingProgress = loading.progress;

                if(m_sliderProgress > 0.89f)
                {
                    m_sceneTransition.PlayEndTransition();
                    while (!m_sceneTransition.m_isTransitionEnd)
                        yield return null;

                    loading.allowSceneActivation = true;
                }

                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_sceneToLoad));

            SceneManager.UnloadSceneAsync(m_currentScene);

            m_isLoad = true;
            m_isSceneIsLoaded = true;

            yield return null;
        }
    }

}