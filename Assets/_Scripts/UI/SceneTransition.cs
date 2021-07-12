using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private Animator m_transitionAnim;
        private string m_sceneName;
        private SceneLoader m_sceneLoader;
        public bool m_isTransitionEnd = false;

        // Use this for initialization
        void Start()
        {
            m_sceneLoader = GameManager.Instance.GetInstanceOf<SceneLoader>();
            StopAllCoroutines();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TransitToScene(string sceneName)
        {
            m_sceneName = sceneName;
            StartCoroutine(TransitionToScene());
        }

        IEnumerator TransitionToScene()
        {
            m_transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1.5f);
            m_sceneLoader.LoadSceneWithLoadingScreen(m_sceneName);
        }

        public void PlayEndTransition()
        {
            StartCoroutine(EndTransition());
        }

        IEnumerator EndTransition()
        {
            m_transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1.5f);
            m_isTransitionEnd = true;
        }
    }

}