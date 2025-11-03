using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : MonoBehaviour
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
            yield return new WaitForSeconds(6f);

            animator.Play("Cast Spell 02");
            
            if (GameManager.Instance.ac)
            {
                GameManager.Instance.BossSkillDir = GameManager.Instance.ac.transform.position - transform.position;
                GameManager.Instance.BossSkillDir.Normalize();
                ObjectPool.Instance.PopFromPool("FirstBossSkill");
            }
            if (GameManager.Instance.dc)
            {
                GameManager.Instance.BossSkillDir = GameManager.Instance.dc.transform.position - transform.position;
                GameManager.Instance.BossSkillDir.Normalize();
                ObjectPool.Instance.PopFromPool("FirstBossSkill");
            }
            if (GameManager.Instance.fc)
            {
                GameManager.Instance.BossSkillDir = GameManager.Instance.fc.transform.position - transform.position;
                GameManager.Instance.BossSkillDir.Normalize();
                ObjectPool.Instance.PopFromPool("FirstBossSkill");
            }

        }
    }
}
