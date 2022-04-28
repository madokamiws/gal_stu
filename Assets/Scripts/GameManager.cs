using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<ScriptData> ScriptDatasList;
    private int scriptIndex;
    private int energyValue;//目前角色的精力值
    public Dictionary<string, int> favorabilityDict;// 其他角色对玩家的好感度
    public Dictionary<int, Action<object>> eventDict;
    public GameObject hitPointGo;
    public Dictionary<int, Action<object>> itemEffectDict;
    public GameObject freeActivitiesGo;
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



    #region Unity中的生命周期函数
    private void Awake()
    {
        Instance = this;
        InitScriptsDataList();
        InitEventDict();
        InitGameData();
        HandleDate();
        ChangeEnergyValue();
    }


    #endregion

    #region 剧本的加载与存储信息
    /// <summary>
    /// 初始化剧本列表
    /// </summary>
    private void InitScriptsDataList()
    {
        ScriptDatasList = new List<ScriptData>()
        {
            new ScriptData()
            {
                 loadType =1,spriteName ="Title",soundType=3,soundPath = "Daily"
            },
            new ScriptData()
            {
                loadType =3,eventID=3
            },
            //new ScriptData()//test 进场
            //{
            //    loadType =3,eventID=5,name="Test",characterPos=2,eventData=1
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Test",dialogueContent = "222222222222222",characterPos=2,soundType=1,soundPath = "0",energyValue = 10
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Test",dialogueContent = "333333333333333",characterPos=1,ifRotate = true,soundType=1,soundPath = "1",favorability=5,animationNum = 1
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Test",dialogueContent = "4444444444444444444",characterPos=3,soundType=1,soundPath = "2"
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Test",dialogueContent = "666666666666666",characterPos=2,soundType=1,soundPath = "3",energyValue=-5,animationNum = 2
            //},
            //new ScriptData()
            //{
            //     loadType =1,spriteName ="Title",soundType=3,soundPath = "Normal"
            //},
            //new ScriptData()//debug 进场
            //{
            //    loadType =3,eventID=5,name="Debug",characterPos=1,eventData=1,characterID=1,
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "0",ifRotate=true,characterID=1,
            //    dialogueContent = "你好，我是debug",animationNum = 3
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Test",characterPos=2,soundType=1,soundPath = "6",
            //    dialogueContent = "你好，我是debug",
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "1",ifRotate=true,characterID=1,
            //    dialogueContent = "基建",energyValue=-50, expressionIndex = 4
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=1,eventData=3,scriptID=1
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=2,dialogueContent = "选项一剧情",
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=3,dialogueContent = "选项二剧情",
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=4,dialogueContent = "选项三剧情",
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "2",ifRotate=true,characterID=1,
            //    dialogueContent = "一选项触发的事件",scriptID=2, expressionIndex = 1
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=1,
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "3",ifRotate=true,characterID=1,
            //    dialogueContent = "二选项触发的事件",scriptID=3,
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=1,
            //},
            //new ScriptData()//test退场
            //{
            //    loadType =3,eventID=5,name="Test",characterPos=2,scriptID=4
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "4",ifRotate=true,characterID=1,
            //    dialogueContent = "那么我们要开始了",animationNum = 1,expressionIndex = 1
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=4,eventData=1,
            //},
            //new ScriptData()//失败时需要跳转的事件剧情位置
            //{
            //    loadType =3,eventID=2,eventData=5,
            //},
            //new ScriptData()//胜利时需要跳转的事件剧情位置
            //{
            //    loadType =3,eventID=2,eventData=6,
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "5",ifRotate=true,characterID=1,
            //    dialogueContent = "游戏失败",scriptID=5,expressionIndex = 6
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=7,
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "6",ifRotate=true,characterID=1,
            //    dialogueContent = "游戏成功",scriptID=6,animationNum = 2,expressionIndex = 4
            //},
            //new ScriptData()
            //{
            //    loadType =3,eventID=2,eventData=7,
            //},
            //new ScriptData()
            //{
            //    loadType =2,name="Debug",characterPos=1,soundType=1,soundPath = "7",ifRotate=true,characterID=1,
            //    dialogueContent = "在下告退",scriptID=7,
            //},
            //new ScriptData()//debug退场
            //{
            //    loadType =3,eventID=5,name="Debug",characterPos=1,ifRotate=true,characterID=1,
            //},
            new ScriptData()
            {
                loadType =2,dialogueContent = "接下来我想去找"
            },
            new ScriptData()
            {
                loadType =3,eventID=4,eventData=2,
            },
            new ScriptData()
            {
                loadType =3,eventID=3,eventData=1,
            },
        };
        for (int i = 0; i < ScriptDatasList.Count; i++)
        {
            ScriptDatasList[i].scriptIndex = i;
        }
    }
    /// <summary>
    /// 读取json信息文件
    /// </summary>
    private void LoadByJson()
    { 
    
    }

    private void SaveByJson()
    { 
    
    }
    #endregion

    #region 游戏数据处理
    private void InitGameData()
    {
        scriptIndex = 0;
        favorabilityDict = new Dictionary<string, int>()
        {
            {"Player",0},
            {"Test",80},
            {"Debug",10}
        };
        energyValue = 50;
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
    /// 获取当前选择项的文本
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public string[] GetChoiceContent(int num)
    {
        string[] choiceContent = new string[num];
        for (int i = 0; i < num; i++)
        {
            choiceContent[i] = ScriptDatasList[scriptIndex + i + 1].dialogueContent;
        }
        return choiceContent;
    }
    #endregion

    #region 游戏逻辑处理
    private void HandleDate()
    {
        if (scriptIndex >= ScriptDatasList.Count)
        {
            Debug.Log("游戏结束");
            return;
        }

        //PLaySound(ScriptDatasList[scriptIndex].soundType);
        PlayGameSound(ScriptDatasList[scriptIndex].soundPath, ScriptDatasList[scriptIndex].soundType, ScriptDatasList[scriptIndex].name);
        if (ScriptDatasList[scriptIndex].loadType == 1)
        {
            //设置背景图片
            SetBGImageSprite(ScriptDatasList[scriptIndex].spriteName);
            // 加载下一跳剧情数据
            LoadNextScript();

        }
        else if (ScriptDatasList[scriptIndex].loadType == 2)
        {
            //人物
            HandleCharacter();
        }
        else if (ScriptDatasList[scriptIndex].loadType == 3)
        {
            //事件
            switch (ScriptDatasList[scriptIndex].eventID)
            {
                //显示选择项
                case 1:
                    ShowChoiceUI(ScriptDatasList[scriptIndex].eventData, GetChoiceContent(ScriptDatasList[scriptIndex].eventData));
                    break;
                //跳转到标记剧本
                case 2:
                    SetScriptIndex(0);
                    break;
                case 3:
                    ShowOrHideMask(System.Convert.ToBoolean(ScriptDatasList[scriptIndex].eventData));
                    break;
                case 4:
                    eventDict[ScriptDatasList[scriptIndex].eventData](null);
                    break;
                case 5:
                    //ShowOrHideCharacterMask(System.Convert.ToBoolean(ScriptDatasList[scriptIndex].eventData)
                    //    , ScriptDatasList[scriptIndex].characterID);
                    //图片
                    DoShowOrHideCharacterTween(System.Convert.ToBoolean(ScriptDatasList[scriptIndex].eventData)
                        , true, 1, ScriptDatasList[scriptIndex].name, ScriptDatasList[scriptIndex].characterPos);
                    //HandleCharacter(true);
                    ShowCharacter(ScriptDatasList[scriptIndex].name);
                    break;

                default:
                    break;
            }
        }
        else
        {
            LoadNextScript();
        }
    }
    /// <summary>
    /// 处理下一条剧情
    /// </summary>
    /// <param name="addNum">处理往下的第几条剧本</param>
    public void LoadNextScript(int addNum = 1)
    {
        scriptIndex += addNum;
        HandleDate();
    }
    /// <summary>
    /// 处理人物相关内容
    /// </summary>
    public void HandleCharacter(bool showCharacterOnly = false)
    {
        UpdateCharacterName(ScriptDatasList[scriptIndex].name);
        //显示人物
        ShowCharacter(ScriptDatasList[scriptIndex].name);

        //设置人物位置
        SetCharacterPos(ScriptDatasList[scriptIndex].characterPos, ScriptDatasList[scriptIndex].name);
        if (!showCharacterOnly)
        {
            //更新对话框
            UpdateTalkLineText(ScriptDatasList[scriptIndex].dialogueContent);
            ChangeEnergyValue(ScriptDatasList[scriptIndex].energyValue);
            ChangeFavourValue(ScriptDatasList[scriptIndex].favorability, ScriptDatasList[scriptIndex].name);
            PlayMotion(ScriptDatasList[scriptIndex].name, ScriptDatasList[scriptIndex].animationNum);
            PlayExpression(ScriptDatasList[scriptIndex].name, ScriptDatasList[scriptIndex].expressionIndex);
        }

    }
    /// <summary>
    /// 设置剧本索引
    /// </summary>
    public void SetScriptIndex(object index)
    {
        for (int i = 0; i < ScriptDatasList.Count; i++)
        {
            if (ScriptDatasList[scriptIndex + (int)index].eventData == ScriptDatasList[i].scriptID)
            {
                scriptIndex = ScriptDatasList[i].scriptIndex;
                break;
            }
        }
        HandleDate();
    }
    #endregion

    #region UI逻辑处理
    /// <summary>
    /// 设置背景图片
    /// </summary>
    /// <param name="spriteName"></param>
    private void SetBGImageSprite(string spriteName)
    {
        UIManager.Get.SetBGImgSprite(spriteName);
    }
    /// <summary>
    /// 更新对话框文本
    /// </summary>
    /// <param name="dialogueContent"></param>
    /// <param name="loadNext"></param>
    public void UpdateTalkLineText(string dialogueContent, bool loadNext = true)
    {
        UIManager.Instance.UpdateTalkLineText(dialogueContent, loadNext);
    }
    /// <summary>
    /// 更新精力值ui
    /// </summary>
    public void UpdateEnergyValue(int value = 0)
    {
        UIManager.Instance.UpdateEnergyValue(value);
    }
    /// <summary>
    /// 更新好感度ui
    /// </summary>
    public void UpdateFavourValue(int value = 0, string name = null)
    {
        UIManager.Instance.UpdateFavourValue(value, name);
    }

    /// <summary>
    /// 显示多选项对话框
    /// </summary>
    /// <param name="choiceNum"></param>
    /// <param name="choiceContent"></param>
    public void ShowChoiceUI(int choiceNum, string[] choiceContent)
    {
        UIManager.Get.ShowChoiceUI(choiceNum, choiceContent, SetScriptIndex);
    }
    /// <summary>
    /// 显示或隐藏遮罩
    /// </summary>
    /// <param name="show">true显示</param>
    public void ShowOrHideMask(bool show)
    {
        UIManager.Get.ShowOrHideMask(show);
    }
    /// <summary>
    /// 执行ui渐隐渐现的动画
    /// </summary>
    /// <param name="show"></param>
    /// <param name="image"></param>
    public void DoShowOrHideUITween(bool show, bool ifLoadNext, float interval, params UnityEngine.UI.Image[] images)
    {
        UIManager.Get.DoShowOrHideUITween(show, ifLoadNext, interval, images);
    }

    /// <summary>
    /// 显示人物名称
    /// </summary>
    /// <param name="name"></param>
    public void UpdateCharacterName(string name)
    {
        UIManager.Get.UpdateCharacterName(name);
    }
    /// <summary>
    /// 关闭多项选择框
    /// </summary>
    public void CloseChoiceUI()
    {
        UIManager.Get.CloseChoiceUI();
    }
    /// <summary>
    /// 显示选择对话框
    /// </summary>
    /// <param name="choiceNum">几个选项</param>
    /// <param name="choiceContent">文本</param>
    public void ShowChoiceUI(int choiceNum, string[] choiceContent, UnityEngine.Events.UnityAction<object> unityAction)
    {
        UIManager.Get.ShowChoiceUI(choiceNum, choiceContent, unityAction);
    }
    /// <summary>
    /// 对话框选择按钮触发事件注册
    /// </summary>
    /// <param name="unityAction"></param>
    public void AddClickButtonListener(UnityEngine.Events.UnityAction<object> unityAction)
    {
        UIManager.Get.AddClickButtonListener(unityAction);
    }
    /// <summary>
    /// 显示对话框中的多项选择
    /// </summary>
    public void ShowChoiceLineUI(int choiceNum, string[] choiceContent, UnityEngine.Events.UnityAction<object> unityAction)
    {
        UIManager.Get.ShowChoiceLineUI(choiceNum, choiceContent, unityAction);
    }
    /// <summary>
    /// 判断是不是拥有这件物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IfOwnItem(int id)
    {
        return UIManager.Get.IfOwnItem(id);
    }
    /// <summary>
    /// 从物品数据字典中移除物品
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id)
    {
        UIManager.Get.RemoveItem(id);
    }
    /// <summary>
    /// 显示物品信息说明框
    /// </summary>
    /// <param name="iteminfo">物品信息</param>
    /// <param name="itemPos">显示位置</param>
    public void ShowTipInfo(ItemInfo iteminfo, Vector3 itemPos)
    {
        UIManager.Get.ShowTipInfo(iteminfo, itemPos);
    }
    public void HideTipInfo()
    {
        UIManager.Get.HideTipInfo();
    }
    #endregion

    #region 人物逻辑处理
    /// <summary>
    /// 显示人物
    /// </summary>
    /// <param name="name"></param>
    /// <param name="characterID"></param>
    public void ShowCharacter(string name, int characterID = 0)
    {
        //UIManager.Get.ShowCharacter(name, characterID);
        CubismManager.Get.ShowCharacter(name);
    }

    /// <summary>
    /// 设置人物位置
    /// </summary>
    /// <param name="posID">1.左 2.右 3.中</param>
    /// <param name="name"></param>
    public void SetCharacterPos(int posID, string name)
    {
        //UIManager.Instance.SetCharacterPos(posID, ifRotate, characterID);
        CubismManager.Get.SetCharacterPos(posID, name);
    }
    /// <summary>
    /// 人物进退场动画
    /// </summary>
    public void DoShowOrHideCharacterTween(bool show, bool ifLoadNext, float interval, string name, int posID)
    {
        CubismManager.Get.DoShowOrHideCharacterTween(show, ifLoadNext, interval, name, posID);
        UIManager.Get.ShowORHideTalkLine(false);
        UIManager.Get.CloseChoiceUI();
    }
    /// <summary>
    /// 播放人物动作
    /// </summary>
    /// <param name="name"></param>
    /// <param name="animationNum"></param>
    public void PlayMotion(string name, int animationNum)
    {
        CubismManager.Get.PlayMotion(name, animationNum);
    }
    /// <summary>
    /// 播放表情
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expressionIndex"></param>
    public void PlayExpression(string name, int expressionIndex)
    {
        if (expressionIndex != 0)
        {
            CubismManager.Get.PlayExpression(name, expressionIndex - 1);
        }

    }
    /// <summary>
    /// 人物说话
    /// </summary>
    /// <param name="name"></param>
    public void CharacterTalk(string name)
    {
        CubismManager.Get.Talk(name);
    }
    #endregion

    #region 音频处理
    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name=""></param>
    public void PlayGameSound(string soundPath, SOUNDTPPE soundtype = SOUNDTPPE.DIALOGUE, string name = null)
    {
        AudioSouceManager.Get.PlayGameSound(soundPath, soundtype, name);
    }
    public void PlayGameSound(string soundPath, int soundtype = 0, string name = null)
    {
        PlayGameSound(soundPath, (SOUNDTPPE)(soundtype - 1), name);
    }
    /// <summary>
    /// 获取对话音频组件
    /// </summary>
    /// <returns></returns>
    public AudioSource GetDialogueAudio()
    {
        return AudioSouceManager.Get.GetDialogueAudio();
    }
    #endregion

    #region 特殊事件

    /// <summary>
    /// 初始化特殊事件字典
    /// </summary>
    private void InitEventDict()
    {
        eventDict = new Dictionary<int, Action<object>>()
        {
            { 1,StartHitPointEvent},
            { 2,StartFreeActivitiesEvent}
        };
        itemEffectDict = new Dictionary<int, Action<object>>()
        {
            {1,UseRecoverEnergyItem}
        };
    }



    /// <summary>
    /// 点击玩法1
    /// </summary>
    /// <param name="src"></param>
    public void StartHitPointEvent(object src)
    {
        UIManager.Get.ShowORHideTalkLine(false);
        hitPointGo.SetActive(true);
    }
    /// <summary>
    /// 恢复体力
    /// </summary>
    /// <param name="src"></param>
    public void UseRecoverEnergyItem(object src)
    {
        ChangeEnergyValue((int)src);
    }

    /// <summary>
    /// 自由活动
    /// </summary>
    /// <param name="src"></param>
    public void StartFreeActivitiesEvent(object src)
    {
        UIManager.Get.ShowORHideTalkLine(false);
        freeActivitiesGo.SetActive(true);
    }
    #endregion





    ///// <summary>
    ///// 播放音效
    ///// </summary>
    ///// <param name="soundType"></param>
    //public void PLaySound(int soundType)
    //{
    //    switch (soundType)
    //    {
    //        case 1:
    //            AudioSouceManager.Instance.PlayDialogue(
    //                ScriptDatasList[scriptIndex].name + "/" + ScriptDatasList[scriptIndex].soundPath
    //                );
    //            CharacterTalk(ScriptDatasList[scriptIndex].name);
    //            break;
    //        case 2:
    //            AudioSouceManager.Instance.PlaySound(
    //                 ScriptDatasList[scriptIndex].soundPath
    //                );
    //            break;
    //        case 3:
    //            AudioSouceManager.Instance.PlayMusic(
    //                 ScriptDatasList[scriptIndex].soundPath
    //                );
    //            break;
    //    }

    //}









}

