using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<ScriptData> scriptDatas;
    private int scriptIndex;

    private void Awake()
    {
        Instance = this;
        scriptDatas = new List<ScriptData>()
        {
            new ScriptData()
            {
                 loadType =1,spriteName ="Title"
            },
            new ScriptData()
            {
                loadType =2,name="Test",dialogueContent = "222222222222222",characterPos=2
            },
            new ScriptData()
            {
                loadType =3,name="Test",dialogueContent = "333333333333333",characterPos=1,ifRotate = true
            },
            new ScriptData()
            {
                loadType =4,name="Test",dialogueContent = "4444444444444444444",characterPos=3
            },
            new ScriptData()
            {
                loadType =4,name="Test",dialogueContent = "666666666666666",characterPos=2
            },

        };
        scriptIndex = 0;
        HandleDate();
    }
    /// <summary>
    /// 处理每一条剧情数据
    /// </summary>
    private void HandleDate()
    {
        if (scriptIndex >= scriptDatas.Count)
        {
            Debug.Log("游戏结束");
            return;
        }
        if (scriptDatas[scriptIndex].loadType == 1)
        {
            //设置背景图片
            SetBGImageSprite(scriptDatas[scriptIndex].spriteName);
            // 加载吓一跳剧情数据
            LoadNextScript();

        }
        else
        {
            //人物
            //显示人物
            ShowCharacter(scriptDatas[scriptIndex].name);
            //更新对话框
            UpdateTalkLineText(scriptDatas[scriptIndex].dialogueContent);
            //设置任务位置
            SetCharacterPos(scriptDatas[scriptIndex].characterPos);
        }
    }
    //设置背景图片
    private void SetBGImageSprite(string spriteName)
    {
        UIManager.Instance.SetBGImgSprite(spriteName);
    }
    public void LoadNextScript()
    {
        scriptIndex++;
        HandleDate();
    }
    //显示人物
    private void ShowCharacter(string name)
    {
        UIManager.Instance.ShowCharacter(name);
    }
    //更新对话框
    private void UpdateTalkLineText(string dialogueContent)
    {
        UIManager.Instance.UpdateTalkLineText(dialogueContent);
    }

    public void SetCharacterPos(int posID)
    {
        UIManager.Instance.SetCharacterPos(posID);
    }
}
/// <summary>
/// 剧本数据类
/// </summary>
public class ScriptData
{
    public int loadType;//载入资源类型 1.背景 2.人
    public string name;//角色名称
    public string spriteName;//资源
    public string dialogueContent;//对话内容
    public int characterPos;//1.左 2.右 3.中
    public bool ifRotate;
}