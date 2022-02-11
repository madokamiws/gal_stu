using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剧本数据类
/// </summary>
public class ScriptData
{
    public int loadType;//载入资源类型 1.背景 2.人 3.事件
    public string name;//角色名称
    public string spriteName;//资源
    public string dialogueContent;//对话内容
    public int characterPos;//1.左 2.右 3.中
    public bool ifRotate;//s是否翻转
    public int soundType;//音频类型1.对话 2.音效 3.音乐
    public string soundPath;//音频路径
    public int favorability;//好感度（改变值）
    public int energyValue;//精力值（改变值）
    public int characterID;//三人对话时的人物id
    //处理事件id
    public int eventID;//1.显示选择项 2 跳转到指定剧本位置 3.显示隐藏遮罩 4.特殊事件 5.显示或影藏人物
    //事件选项数据：
    //1  eventID为1时代表几个选择项  2  eventID为2时代表具体要跳转到的标记位 3.0隐藏1显示遮罩 4.eventID为3时代表事件ID 5.0是退场1是进场
    public int eventData;
    //剧本用于标记位，用于跳转
    public int scriptID;
    public int scriptIndex;//剧本索引
}
