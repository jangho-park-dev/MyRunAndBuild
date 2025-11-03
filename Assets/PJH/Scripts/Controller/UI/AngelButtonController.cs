using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AngelButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button btn;

    private GameObject[] enemyArray;
    private Coroutine angelSkillCoroutine;
    private float angelSkillDamage;

    private void Awake()
    {
        angelSkillDamage = 70f;
    }

    // Start is called before the first frame update
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        
        angelSkillCoroutine = null;
    }

    private void OnDisable()
    {
        if (angelSkillCoroutine != null)
        {
            StopCoroutine(angelSkillCoroutine);
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
                GameManager.Instance.ac.animator.SetBool("Run", false);
                GameManager.Instance.ac.animator.Play("Jump Right Attack 01");
                angelSkillCoroutine = StartCoroutine(AngelSkill());
                GameManager.Instance.ac.animator.SetBool("Run", true);
            }
            else
            {
                GameManager.Instance.ac.animator.Play("Jump Right Attack 01");
                angelSkillCoroutine = StartCoroutine(AngelSkill());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AngelSkill()
    {
        yield return new WaitForSeconds(0.5f);

        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyArray.Length; ++i)
        {
            ObjectPool.Instance.PopFromPool("AngelSkillForGround").transform.position = enemyArray[i].transform.position;
            ObjectPool.Instance.PopFromPool("AngelSkillForAir").transform.position = enemyArray[i].transform.position + new Vector3(0f, 1f, 0f);
            SkillEffect(i);
        }
    }

    private void SkillEffect(int idx)
    {
        if (enemyArray[idx].name.Substring(0, 4) == "Enem")
            enemyArray[idx].GetComponent<EnemyController>().TakeDamage(angelSkillDamage);
        else
            enemyArray[idx].GetComponent<BossController>().TakeDamage(angelSkillDamage);
    }
}
