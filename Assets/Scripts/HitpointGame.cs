using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HitpointGame : MonoBehaviour
{
    private bool gamestart;
    private float startTime;
    private float timeVal;
    public GameObject[] points;
    public int hitnum;
    private Image[] imgpoints;
    // Start is called before the first frame update
    void Start()
    {
        gamestart = true;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamestart)
        {
            if (Time.time - startTime >= 10)
            {
                //游戏结束
                gamestart = false;
                if (hitnum >= 3)
                {
                    //win
                    GameManager.Instance.LoadNextScript(2);
                }
                else
                {
                    //lost
                    GameManager.Instance.LoadNextScript();
                }
                gameObject.SetActive(false);
            }
            else
            {
                timeVal -= Time.deltaTime;
                if (timeVal <= 0)
                {
                    ShowPoints();
                    timeVal = 2;
                }
            
            }
        }

    }
    private void ShowPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].SetActive(false);
        }
        GameObject point = points[Random.Range(0, points.Length)];
        point.SetActive(true);
        Image[] images = point.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r,
                images[i].color.g,
                images[i].color.b,0);
        }
        GameManager.Get.DoShowOrHideUITween(true, false,2,images);
    }
    public void HitPoint(GameObject obj)
    {
        hitnum++;
        obj.SetActive(false);
    }
}
