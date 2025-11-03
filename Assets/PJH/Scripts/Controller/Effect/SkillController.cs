using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public Coroutine particleTimerCoroutine;

    private void OnEnable()
    {
        particleTimerCoroutine = StartCoroutine(particleTimer());
    }

    private void OnDisable()
    {
        StopCoroutine(particleTimer());
    }

    IEnumerator particleTimer()
    {
        ParticleSystem ps = this.GetComponent<ParticleSystem>();

        while (true && ps != null)
        {
            yield return new WaitForSeconds(0.5f);

            if (!ps.IsAlive(true))
            {
                ObjectPool.Instance.PushToPool(gameObject.name, gameObject);
            }
        }
    }
}
