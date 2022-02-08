using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<ScriptData> scriptDatas;
    private int scriptIndex;
    private int energyValue;//目前角色的精力值
    public Dictionary<string, int> favorabilityDict;// 其他角色对玩家的好感度
    public static GameManager Get
    {
        get
        {
            if (!Instance)
            {
                Instance = new GameManager();
            }
            return Instance;
        }
    }
    private void Awake()
    {
        Instance = this;
        scriptDatas = new List<ScriptData>()
        {
            new ScriptData()
            {
                 loadType =1,spriteName ="Title",soundType=3,soundPath = "Daily"
            },
            new ScriptData()
            {
                loadType =2,name="Test",dialogueContent = "222222222222222",characterPos=2,soundType=1,soundPath = "0",energyValue = 50
            },
            new ScriptData()
            {
                loadType =2,name="Test",dialogueContent = "333333333333333",characterPos=1,ifRotate = true,soundType=1,soundPath = "1",favorability=5
            },
            new ScriptData()
            {
                loadType =2,name="Test",dialogueContent = "4444444444444444444",characterPos=3,soundType=1,soundPath = "2"
            },
            new ScriptData()
            {
                loadType =2,name="Test",dialogueContent = "666666666666666",characterPos=2,soundType=1,soundPath = "3",energyValue=-5
            },
            new ScriptData()
            {
                 loadType =1,spriteName ="Title",soundType=3,soundPath = "Normal"
            },
            new ScriptData()
            {
                loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "0",ifRotate=true,characterID=1,
                dialogueContent = "你好，我是debug",
            },
            new ScriptData()
            {
                loadType =2,name="Test",characterPos=2,soundType=1,soundPath = "6",
                dialogueContent = "你好，我是debug",
            },
            new ScriptData()
            {
                loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "1",ifRotate=true,characterID=1,
                dialogueContent = "基建",energyValue=-50
            },
            new ScriptData()
            {
                loadType =3,eventID=1,eventData=3,scriptID=1
            },
            new ScriptData()
            {
                loadType =3,eventID=2,eventData=2,dialogueContent = "选项一剧情",
            },
            new ScriptData()
            {
                loadType =3,eventID=2,eventData=3,dialogueContent = "选项二剧情",
            },
            new ScriptData()
            {
                loadType =3,eventID=2,eventData=4,dialogueContent = "选项三剧情",
            },
            new ScriptData()
            {
                loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "2",ifRotate=true,characterID=1,
                dialogueContent = "一选项触发的事件",scriptID=2,
            },
            new ScriptData()
            {
                loadType =3,eventID=2,eventData=1,
            },
            new ScriptData()
            {
                loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "3",ifRotate=true,characterID=1,
                dialogueContent = "二选项触发的事件",scriptID=3,
            },
            new ScriptData()
            {
                loadType =3,eventID=2,eventData=1,
            },
            new ScriptData()
            {
                loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "4",ifRotate=true,characterID=1,
                dialogueContent = "三选项触发继续剧情",scriptID=4,
            },

        };
        scriptIndex = 0;
        HandleDate();
        energyValue = 100;
        ChangeEnergyValue();
        favorabilityDict = new Dictionary<string, int>()
        {
            {"Player",0},
            {"Test",80},
            {"Debug",10}
        };
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            scriptDatas[i].scriptIndex = i;
        }
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

        PLaySound(scriptDatas[scriptIndex].soundType);
        if (scriptDatas[scriptIndex].loadType == 1)
        {
            //设置背景图片
            SetBGImageSprite(scriptDatas[scriptIndex].spriteName);
            // 加载下一跳剧情数据
            LoadNextScript();

        }
        else if (scriptDatas[scriptIndex].loadType == 2)
        {
            //人物
            HandleCharacter();
        }
        else if (scriptDatas[scriptIndex].loadType == 3)
        {
            //事件
            switch (scriptDatas[scriptIndex].eventID)
            {
                //显示选择项
                case 1:
                    ShowChoiceUI(scriptDatas[scriptIndex].eventData, GetChoiceContent(scriptDatas[scriptIndex].eventData));
                    break;
                //跳转到标记剧本
                case 2:
                    SetScriptIndex();
                    break;
                case 3:


                default:
                    break;
            }
        }
        else
        {
            LoadNextScript();
        }
    }
    //设置背景图片
    private void SetBGImageSprite(string spriteName)
    {
        UIManager.Get.SetBGImgSprite(spriteName);
    }
    public void LoadNextScript()
    {
        scriptIndex++;
        HandleDate();
    }
    //显示人物
    private void ShowCharacter(string name, int characterID = 0)
    {
        UIManager.Get.ShowCharacter(name, characterID);
    }
    //更新对话框
    private void UpdateTalkLineText(string dialogueContent)
    {
        UIManager.Instance.UpdateTalkLineText(dialogueContent);
    }

    public void SetCharacterPos(int posID, bool ifRotate = false, int characterID = 0)
    {
        UIManager.Instance.SetCharacterPos(posID, ifRotate, characterID);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundType"></param>
    public void PLaySound(int soundType)
    {
        switch (soundType)
        {
            case 1:
                AudioSouceManager.Instance.PlayDialogue(
                    scriptDatas[scriptIndex].name + "/" + scriptDatas[scriptIndex].soundPath
                    );
                break;
            case 2:
                AudioSouceManager.Instance.PlaySound(
                     scriptDatas[scriptIndex].soundPath
                    );
                break;
            case 3:
                AudioSouceManager.Instance.PlayMusic(
                     scriptDatas[scriptIndex].soundPath
                    );
                break;
        }

    }
    /// <summary>
    /// 改变精力值
    /// </summary>
    /// <param name="value">需要具体改变的值</param>
    public void ChangeEnergyValue(int value = 0)
    {
        if (value == 0)
        {
            UpdateEnergyValue(energyValue);
            return;
        }
        if (value > 0)
        {
            AudioSouceManager.Instance.PlaySound("Energy");
        }
        energyValue += value;
        if (energyValue >= 100)
        {
            energyValue = 100;
        }
        else if (energyValue <= 0)
        {
            energyValue = 0;
        }
        UpdateEnergyValue(energyValue);
    }
    /// <summary>
    /// 更新精力值ui
    /// </summary>
    public void UpdateEnergyValue(int value = 0)
    {
        UIManager.Instance.UpdateEnergyValue(value);
    }

    /// <summary>
    /// 改变好感度
    /// </summary>
    /// <param name="value"></param>
    public void ChangeFavourValue(int value = 0, string name = null)
    {
        if (value == 0)
        {
            return;
        }
        if (value > 0)
        {
            AudioSouceManager.Instance.PlaySound("Favor");
        }
        favorabilityDict[name] += value;
        if (favorabilityDict[name] >= 100)
        {
            favorabilityDict[name] = 100;
        }
        else if (favorabilityDict[name] <= 0)
        {
            favorabilityDict[name] = 0;
        }
        UpdateFavourValue(favorabilityDict[name], name);
    }
    /// <summary>
    /// 更新好感度ui
    /// </summary>
    public void UpdateFavourValue(int value = 0, string name = null)
    {
        UIManager.Instance.UpdateFavourValue(value, name);
    }
    /// <summary>
    /// 处理人物相关内容
    /// </summary>
    public void HandleCharacter()
    {
        //显示人物
        ShowCharacter(scriptDatas[scriptIndex].name, scriptDatas[scriptIndex].characterID);
        //更新对话框
        UpdateTalkLineText(scriptDatas[scriptIndex].dialogueContent);
        //设置人物位置
        SetCharacterPos(scriptDatas[scriptIndex].characterPos, scriptDatas[scriptIndex].ifRotate, scriptDatas[scriptIndex].characterID);
        ChangeEnergyValue(scriptDatas[scriptIndex].energyValue);
        ChangeFavourValue(scriptDatas[scriptIndex].favorability, scriptDatas[scriptIndex].name);
    }
    /// <summary>
    /// 显示多选项对话框
    /// </summary>
    /// <param name="choiceNum"></param>
    /// <param name="choiceContent"></param>
    public void ShowChoiceUI(int choiceNum, string[] choiceContent)
    {
        UIManager.Get.ShowChoiceUI(choiceNum, choiceContent);
    }
    /// <summary>
    /// 获取当前选择项的文本
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public string[] GetChoiceContent(int num)
    {
        string[] choiceContent = new string[num];
        for (int i = 0; i < num; i++)
        {
            choiceContent[i] = scriptDatas[scriptIndex + i + 1].dialogueContent;
        }
        return choiceContent;
    }
    /// <summary>
    /// 设置剧本索引
    /// </summary>
    public void SetScriptIndex(int index = 0)
    {
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            if (scriptDatas[scriptIndex+index].eventData == scriptDatas[i].scriptID)
            {
                scriptIndex = scriptDatas[i].scriptIndex;
                break;
            }
        }
        HandleDate();
    }
}