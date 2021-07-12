using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace BTA
{

    public class CameraManager : MonoBehaviour
    {
        GameManager m_gameMgr;

        [SerializeField] CinemachineVirtualCamera m_virtualCamera;
        [SerializeField] GameObject m_cacTarget;
        [SerializeField] GameObject m_distTarget;

        CinemachineTargetGroup m_targetGroup;
        CinemachineBasicMultiChannelPerlin m_shakeComponent;

        CinemachineVirtualCamera m_tempVirtualCamera;

        void Start()
        {
            m_gameMgr = GameManager.Instance;

            m_targetGroup = m_virtualCamera.Follow.GetComponent<CinemachineTargetGroup>();
            if (!m_targetGroup)
                Debug.LogError(this + " cannot find target group component " + m_virtualCamera.Follow);

            m_shakeComponent = m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (!m_shakeComponent)
                Debug.LogError(this + " cannot find his shake component");

            LevelManager.Instance.GetInstanceOf<PlayerManager>().OnSwitchPlayer.AddListener(SwitchSoloCamera);
            Init();
        }

        private void Init()
        {
            EnableTargetAtIndex(m_cacTarget, 0);
            if (m_gameMgr.GetGameMode() == GameMode.Duo)
                EnableTargetAtIndex(m_distTarget, 1);
            else
                DisableTargetAtIndex(1);
        }

        public void SwitchSoloCamera(GameObject playerEnabled)
        {
            if (playerEnabled.GetComponent<PlayerCacFighting>())

                EnableTargetAtIndex(m_cacTarget, 0);

            else if (playerEnabled.GetComponent<PlayerDistFighting>())

                EnableTargetAtIndex(m_distTarget, 0);
        }

        private void EnableTargetAtIndex(GameObject newTarget, int idx)
        {
            m_targetGroup.m_Targets[idx].target = newTarget.transform;
            m_targetGroup.m_Targets[idx].weight = 1;
        }

        private void DisableTargetAtIndex(int idx)
        {
            m_targetGroup.m_Targets[idx].target = null;
            m_targetGroup.m_Targets[idx].weight = 0;
        }

        #region SwitchCamera

        public void EnableTemporaryCamera(CinemachineVirtualCamera tempCam)
        {
            m_tempVirtualCamera = tempCam;

            m_targetGroup = tempCam.Follow.GetComponent<CinemachineTargetGroup>();
            if (!m_targetGroup)
                Debug.LogError(this + " cannot find target group component " + m_virtualCamera.Follow);

            m_shakeComponent = tempCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (!m_shakeComponent)
                Debug.LogError(this + " cannot find his shake component");

            Init();
        }

        public void ResetCamera()
        {
            m_tempVirtualCamera = null;
            m_targetGroup = m_virtualCamera.Follow.GetComponent<CinemachineTargetGroup>();
            m_shakeComponent = m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Init();
        }
        #endregion

        #region Shake

        public void SetShakeCamera(float amplitude, float frequency)
        {
            m_shakeComponent.m_AmplitudeGain = amplitude;
            m_shakeComponent.m_FrequencyGain = frequency;
        }

        public void ResetShakeCamera()
        {
            m_shakeComponent.m_AmplitudeGain = 0f;
            m_shakeComponent.m_FrequencyGain = 0f;
        }

        public void ShakeCameraTemporary(float amplitude, float frequency, float duration)
        {
            StopCoroutine("Shake");

            StartCoroutine(Shake(amplitude, frequency, duration));
        }

        IEnumerator Shake(float amplitude, float frequency, float duration)
        {
            m_shakeComponent.m_AmplitudeGain = amplitude;
            m_shakeComponent.m_FrequencyGain = frequency;
            yield return new WaitForSeconds(duration);
            m_shakeComponent.m_AmplitudeGain = 0f;
            m_shakeComponent.m_FrequencyGain = 0f;
        }

        #endregion
    }
}
