using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BTA
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image m_lifeBar;
        [SerializeField] private Image m_recoverBar;
        [SerializeField] private Image m_corruptedBar;

        [SerializeField] float m_smoothUpdateTime = 1f;

        Coroutine m_lifeCoroutine;
        bool m_lifeUpdate = false;
        Coroutine m_recoveryCoroutine;
        bool m_recoveryUpdate = false;

        public UnityEvent OnLifeBarEmpty;
        public UnityEvent OnLifeBarRecovered;

        public void OnLifeBarChange(AttackTrigger trigger, Damageable damageable)
        {
            if (m_lifeUpdate)
                StopCoroutine(m_lifeCoroutine);

            //float lifeRatio = 0;
            //if (damageable.Life > 0)
            //    lifeRatio = damageable.Life / damageable.MaxLife;
            float lifeRatio = damageable.IsAlive() ? damageable.Life / damageable.MaxLife : 0f;

            m_lifeCoroutine = StartCoroutine(SmoothBarUpdate(m_lifeBar, lifeRatio, m_lifeUpdate));
            if (!damageable.IsAlive())
            {
                OnRecoveryBarUpdate(damageable, 0f);
                OnLifeBarEmpty.Invoke();
            }
        }

        public void OnGainLife(Damageable damageable)
        {
            if (m_lifeUpdate)
                StopCoroutine(m_lifeCoroutine);

            //float lifeRatio = 0;
            //if (damageable.Life > 0)
            //    lifeRatio = damageable.Life / damageable.MaxLife;

            float lifeRatio = damageable.IsAlive() ? damageable.Life / damageable.MaxLife : 0f;

            m_lifeCoroutine = StartCoroutine(SmoothBarUpdate(m_lifeBar, lifeRatio, m_lifeUpdate));

            OnLifeBarRecovered.Invoke();
        }

        public void OnRecoveryBarUpdate(Damageable damageable, float recoveryLife)
        {
            if (m_recoveryUpdate)
                StopCoroutine(m_recoveryCoroutine);

            //float recoverRatio = 0;
            //if (damageable.Life > 0)
            //    recoverRatio = recoveryLife / damageable.MaxLife;

            float recoverRatio = damageable.IsAlive() ? recoveryLife / damageable.MaxLife : 0f;

            //m_recoverBar.fillAmount = recoverRatio;
            m_recoveryCoroutine = StartCoroutine(SmoothBarUpdate(m_recoverBar, recoverRatio, m_recoveryUpdate));
        }

        public void OnCorruptedBarUpdate(Damageable damageable, float corruptedLife)
        {
            if (!damageable.IsAlive())
                return;

            m_corruptedBar.fillAmount = corruptedLife / damageable.MaxLife;
        }

        public void UpdateFromPlayer(Player player)
        {
            StopAllCoroutines();
            Damageable damageable = player.GetDameageable();
            m_lifeBar.fillAmount = damageable.Life / damageable.MaxLife;
            m_recoverBar.fillAmount = player.RecoveryLife / damageable.MaxLife;
            m_corruptedBar.fillAmount = player.CorruptedLife / damageable.MaxLife;
        }

        IEnumerator SmoothBarUpdate(Image bar, float newRatio, bool running)
        {
            running = true;

            //Debug.Log(bar + " => Smooth update " + bar.fillAmount + " to " + newRatio);
            float t = 1f;
            while (t >= 0f)
            {
                bar.fillAmount = Mathf.Lerp(newRatio, bar.fillAmount, t);

                t -= Time.deltaTime / m_smoothUpdateTime;
                yield return null;
            }

            running = false;

            yield return new WaitForEndOfFrame();
        }
    }
}