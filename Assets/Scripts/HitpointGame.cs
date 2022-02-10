using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointGame : MonoBehaviour
{
    private bool gamestart;
    private float startTime;
    private float timeVal;
    public GameObject[] points;
    public int hitnum;
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
        points[Random.Range(0, points.Length)].SetActive(true);
    }
    public void HitPoint(GameObject obj)
    {
        hitnum++;
        obj.SetActive(false);
    }
}
