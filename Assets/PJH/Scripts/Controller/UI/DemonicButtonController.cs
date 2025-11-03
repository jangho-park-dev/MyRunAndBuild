using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DemonicButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button btn;

    private GameObject[] enemyArray;
    private Coroutine demonicSkillCoroutine;
    private float demonicSkillDamage;

    private void Awake()
    {
        demonicSkillDamage = 150f;
    }

    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();

        demonicSkillCoroutine = null;
    }

    private void OnDisable()
    {
        if (demonicSkillCoroutine != null)
        {
            StopCoroutine(demonicSkillCoroutine);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn.enabled)
        {
            if (!GameManager.Instance.meetEnemy)
            {
                GameManager.Instance.dc.animator.SetBool("Run", false);
                GameManager.Instance.dc.animator.Play("Melee Left Attack 01");
                demonicSkillCoroutine = StartCoroutine(DemonicSkill());
                GameManager.Instance.dc.animator.SetBool("Run", true);
            }
            else
            {
                GameManager.Instance.dc.animator.Play("Melee Left Attack 01");
                demonicSkillCoroutine = StartCoroutine(DemonicSkill());
            }
        }
    }

    IEnumerator DemonicSkill()
    {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        int saveNumber = 0;
        float maxHp = 0f;

        if (enemyArray[0].name.Substring(0, 4) == "Enem")
            maxHp = enemyArray[0].GetComponent<EnemyController>().enemyHp;
        else
            maxHp = enemyArray[0].GetComponent<BossController>().bossHp;

        for (int i = 1; i < enemyArray.Length; ++i)
        {
            if (enemyArray[i].name.Substring(0, 4) == "Enem")
            {
                if (maxHp < enemyArray[i].GetComponent<EnemyController>().enemyHp)
                {
                    maxHp = enemyArray[i].GetComponent<EnemyController>().enemyHp;
                    saveNumber = i;
                }
            }
            else
            {
                if (maxHp < enemyArray[i].GetComponent<BossController>().bossHp)
                {
                    maxHp = enemyArray[i].GetComponent<BossController>().bossHp;
                    saveNumber = i;
                }
            }
        }

        ObjectPool.Instance.PopFromPool("DemonicSkillForFog").transform.position = GameManager.Instance.dc.transform.position;

        Vector3 dir = GameManager.Instance.fc.transform.position - enemyArray[saveNumber].transform.position;
        dir.Normalize();
        GameManager.Instance.dc.transform.position = enemyArray[saveNumber].transform.position - dir * 2f;
        GameManager.Instance.dc.transform.LookAt(enemyArray[saveNumber].transform);

        ObjectPool.Instance.PopFromPool("DemonicSkillForFog").transform.position = GameManager.Instance.dc.transform.position;
        ObjectPool.Instance.PopFromPool("DemonicSkillForBats").transform.position = GameManager.Instance.dc.transform.position;

        yield return new WaitForSeconds(0.5f);

        ObjectPool.Instance.PopFromPool("DemonicSkillForHit").transform.position = enemyArray[saveNumber].transform.position + new Vector3(0f, 1f, 0f);
        SkillEffect(saveNumber);

        yield return new WaitForSeconds(0.5f);
        ObjectPool.Instance.PopFromPool("DemonicSkillForFog").transform.position = GameManager.Instance.dc.transform.position;

        GameManager.Instance.dc.transform.position = GameManager.Instance.dc.basePosition;
        GameManager.Instance.dc.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 1));

        ObjectPool.Instance.PopFromPool("DemonicSkillForFog").transform.position = GameManager.Instance.dc.transform.position;
        ObjectPool.Instance.PopFromPool("DemonicSkillForBats").transform.position = GameManager.Instance.dc.transform.position;
    }

    private void SkillEffect(int idx)
    {
        if (enemyArray[idx].name.Substring(0, 4) == "Enem")
            enemyArray[idx].GetComponent<EnemyController>().TakeDamage(demonicSkillDamage);
        else
            enemyArray[idx].GetComponent<BossController>().TakeDamage(demonicSkillDamage);
    }
}
