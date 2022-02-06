using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    public Image imgBG;
    public Image imgCharacter;
    public Text textName;
    public Text textTalkline;
    public GameObject talkLineGo;//对话框父对象
    public Transform[] characterPosTrans;
    public Text textEnergyValue;
    public Text textFavorValue;
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 设置背景图片
    /// </summary>
    /// <param name="spriteName"></param>
    public void SetBGImgSprite(string spriteName)
    {
        imgBG.sprite = Resources.Load<Sprite>("Sprites/BG/"+spriteName);
    }
    /// <summary>
    /// 显示人物
    /// </summary>
    /// <param name="name"></param>
    public void ShowCharacter(string name)
    {
        talkLineGo.SetActive(true);
        imgCharacter.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
        imgCharacter.gameObject.SetActive(true);
        textName.text = name;
    }
    /// <summary>
    /// 更新对话内容
    /// </summary>
    /// <param name="dialogueContent"></param>
    public void UpdateTalkLineText(string dialogueContent)
    {
        textTalkline.text = dialogueContent;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="posID"></param>
    public void SetCharacterPos(int posID,bool ifRotate = false)
    {
        imgCharacter.transform.localPosition = characterPosTrans[posID-1].localPosition;
        if (ifRotate)
        {
            imgCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            imgCharacter.transform.eulerAngles = Vector3.zero;
        }
    }
    /// <summary>
    /// 更新精力值ui
    /// </summary>
    public void UpdateEnergyValue(int value = 0)
    {
        textEnergyValue.text = value.ToString();
    }
    /// <summary>
    /// 更新好感度值ui
    /// </summary>
    /// <param name="value"></param>
    public void UpdateFavourValue(int value = 0, string name = null)
    {
        textFavorValue.text = value.ToString();
    }
}
