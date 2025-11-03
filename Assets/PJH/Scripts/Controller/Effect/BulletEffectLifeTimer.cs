using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectLifeTimer : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(BulletLifeTimer());
    }

    IEnumerator BulletLifeTimer()
    {
        yield return new WaitForSeconds(1f);
        ObjectPool.Instance.PushToPool(gameObject.name, gameObject);
    }
}
