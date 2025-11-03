using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : MonoBehaviour
{
    private Vector3 skillDir;
    private Coroutine skillCoroutine;
    private bool onceForCoroutine;
    private Animator animator;

    void Start()
    {
        skillDir = new Vector3(0f, 0f, 0f);
        onceForCoroutine = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.meetEnemy && onceForCoroutine)
        {
            onceForCoroutine = false;
            skillCoroutine = StartCoroutine(SkillTimer());
        }
    }

    private void OnDisable()
    {
        if (skillCoroutine != null)
            StopCoroutine(skillCoroutine);

        onceForCoroutine = true;
    }

    IEnumerator SkillTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(9f);

            animator.SetBool("Spin Attack", true);

            if (GameManager.Instance.ac)
            {
                GameObject obj = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect01");
                obj.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 0.1f, 0f);
            }
            if (GameManager.Instance.dc)
            {
                GameObject obj = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect01");
                obj.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 0.1f, 0f);
            }
            if (GameManager.Instance.fc)
            {
                GameObject obj = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect01");
                obj.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 0.1f, 0f);
            }

            yield return new WaitForSeconds(0.3f);

            if (GameManager.Instance.ac)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.ac.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }
            if (GameManager.Instance.dc)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.dc.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }
            if (GameManager.Instance.fc)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.fc.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }

            yield return new WaitForSeconds(0.3f);

            if (GameManager.Instance.ac)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.ac.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }
            if (GameManager.Instance.dc)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.dc.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }
            if (GameManager.Instance.fc)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.fc.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }

            yield return new WaitForSeconds(0.3f);

            if (GameManager.Instance.ac)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.ac.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.ac.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }
            if (GameManager.Instance.dc)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.dc.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.dc.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }
            if (GameManager.Instance.fc)
            {
                GameObject obj1 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect02");
                GameObject obj2 = ObjectPool.Instance.PopFromPool("FinalBossSkillEffect03");
                obj1.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
                obj2.transform.position = GameManager.Instance.fc.transform.position + new Vector3(0f, 1f, 0f);
                GameManager.Instance.fc.TakeDamage(GameManager.Instance.finalBossSkillAttackDamage);
            }

            animator.SetBool("Spin Attack", false);
        }
    }
}
