using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonCooltimeController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image img;
    public Button btn;
    public Text txt;
    public float angelCooltime;
    public float demonCooltime;
    public float fairyCooltime;

    private float leftTime;
    private Coroutine CooltimeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (img == null)
            img = gameObject.GetComponent<Image>();
        if (btn == null)
            btn = gameObject.GetComponent<Button>();
        if (txt == null)
            txt = gameObject.GetComponentInChildren<Text>();

        angelCooltime = 11f;
        demonCooltime = 13f;
        fairyCooltime = 17f;
        leftTime = 0f;
    }

    private void OnDisable()
    {
        if (CooltimeCoroutine != null)
            StopCoroutine(CooltimeCoroutine);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn.enabled)
        {
            if (btn)
                btn.enabled = false;

            if (btn.name.Substring(0, 5) == "Angel")
            {
                print(btn.name.Substring(0, 5));
                leftTime = angelCooltime;
                CooltimeCoroutine = StartCoroutine(StartCooltime(angelCooltime));
            }
            else if (btn.name.Substring(0, 5) == "Demon")
            {
                print(btn.name.Substring(0, 5));
                leftTime = demonCooltime;
                CooltimeCoroutine = StartCoroutine(StartCooltime(demonCooltime));
            }
            else if (btn.name.Substring(0, 5) == "Fairy")
            {
                print(btn.name.Substring(0, 5));
                leftTime = fairyCooltime;
                CooltimeCoroutine = StartCoroutine(StartCooltime(fairyCooltime));
            }
        }
    }

    IEnumerator StartCooltime(float cooltime)
    {
        while (leftTime > 0f)
        {
            leftTime -= Time.deltaTime;
            if (leftTime < 0f)
            {
                leftTime = 0f;
                if (btn)
                    btn.enabled = true;
            }

            float ratio = 1.0f - (leftTime / cooltime);
            if (img)
                img.fillAmount = ratio;
            if (txt)
                txt.text = string.Format("{0:0.#}", leftTime);

            yield return null;
        }
        if (txt)
            txt.text = "";
    }
}
