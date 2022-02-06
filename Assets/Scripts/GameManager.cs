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

        };
        scriptIndex = 0;
        HandleDate();
        energyValue = 50;
        favorabilityDict = new Dictionary<string, int>()
        {
            {"Player",0},
            {"Test",80},
        };
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
        else
        {
            //人物
            HandleCharacter();
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

    public void SetCharacterPos(int posID, bool ifRotate = false)
    {
        UIManager.Instance.SetCharacterPos(posID,ifRotate);
    }
    public void PLaySound(int soundType)
    {
        switch (soundType)
        {
            case 1:
                AudioSouceManager.Instance.PlayDialogue(
                    scriptDatas[scriptIndex].name+"/"+scriptDatas[scriptIndex].soundPath
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
    public void UpdateEnergyValue(int value =0)
    {
        UIManager.Instance.UpdateEnergyValue(value);
    }

    /// <summary>
    /// 改变好感度
    /// </summary>
    /// <param name="value"></param>
    public void ChangeFavourValue(int value = 0,string name = null)
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
        UpdateFavourValue(favorabilityDict[name],name);
    }
    /// <summary>
    /// 更新好感度ui
    /// </summary>
    public void UpdateFavourValue(int value = 0, string name = null)
    {
        UIManager.Instance.UpdateFavourValue(value,name);
    }
    /// <summary>
    /// 处理人物相关内容
    /// </summary>
    public void HandleCharacter()
    {
        //显示人物
        ShowCharacter(scriptDatas[scriptIndex].name);
        //更新对话框
        UpdateTalkLineText(scriptDatas[scriptIndex].dialogueContent);
        //设置任务位置
        SetCharacterPos(scriptDatas[scriptIndex].characterPos, scriptDatas[scriptIndex].ifRotate);
        ChangeEnergyValue(scriptDatas[scriptIndex].energyValue);
        ChangeFavourValue(scriptDatas[scriptIndex].favorability, scriptDatas[scriptIndex].name);
    }
}