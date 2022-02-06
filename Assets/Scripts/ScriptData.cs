using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool ifRotate;//s是否翻转
    public int soundType;//音频类型1.对话 2.音效 3.音乐
    public string soundPath;//音频路径
    public int favorability;//好感度（改变值）
    public int energyValue;//精力值（改变值）
}
