using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    public Image imgBG;
    public Image imgCharacter;
    public Image imgCharacter2;
    public Text textName;
    public Text textTalkline;
    public GameObject talkLineGo;//对话框父对象
    public Transform[] characterPosTrans;
    public Text textEnergyValue;
    public Text textFavorValue;
    public GameObject empChoiceUIGo;//多项选择框父对象
    public GameObject[] choiceUIGos;
    public Text[] textChoiceUIs;
    public bool toNextScene;//是否开始遮罩动画
    public bool showMask;//显示或隐藏遮罩
    public Image mask;//全屏遮罩

    public static UIManager Get
    {
        get
        {
            if (!Instance)
            {
                Instance = new UIManager();
            }
            return Instance;
        }
    }
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
    public void ShowCharacter(string name,int characterID=0)
    {
        ShowORHideTalkLine(true);
        CloseChoiceUI();
        textName.text = name;
        if (characterID == 0)
        {
            imgCharacter.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
            imgCharacter.SetNativeSize();
            imgCharacter.gameObject.SetActive(true);
        }
        else
        {
            imgCharacter2.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
            imgCharacter2.SetNativeSize();
            imgCharacter2.gameObject.SetActive(true);
        }
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
    /// 设置人物位置
    /// </summary>
    /// <param name="posID"></param>
    public void SetCharacterPos(int posID,bool ifRotate = false,int characterID=0)
    {
        if (characterID == 0)
        {
            setPos(posID, imgCharacter, ifRotate);
        }
        else
        {
            setPos(posID, imgCharacter2, ifRotate);
        }

    }

    public void setPos(int posID, Image targetImage,bool ifRotate = false )
    {
        targetImage.transform.localPosition = characterPosTrans[posID - 1].localPosition;
        if (ifRotate)
        {
            targetImage.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            targetImage.transform.eulerAngles = Vector3.zero;
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
    /// <summary>
    /// 显示选择对话框
    /// </summary>
    /// <param name="choiceNum">几个选项</param>
    /// <param name="choiceContent">文本</param>
    public void ShowChoiceUI(int choiceNum,string[] choiceContent)
    {
        empChoiceUIGo.SetActive(true);
        ShowORHideTalkLine(false);
        for (int i = 0; i < choiceUIGos.Length; i++)
        {
            choiceUIGos[i].SetActive(false);
        }
        for (int i = 0; i < choiceNum; i++)
        {
            choiceUIGos[i].SetActive(true);
            textChoiceUIs[i].text = choiceContent[i];
        }
    }
    public void CloseChoiceUI()
    {
        empChoiceUIGo.SetActive(false);
    }

    /// <summary>
    /// 显示活隐藏对话框
    /// </summary>
    /// <param name="show"></param>
    public void ShowORHideTalkLine(bool show = true)
    {
        talkLineGo.SetActive(show);


    }
    /// <summary>
    /// 显示或隐藏遮罩
    /// </summary>
    /// <param name="show">true显示</param>
    public void ShowOrHideMask(bool show)
    {
        toNextScene = true;
        showMask = show;
    }
    public void ShowMask()
    {
        mask.color += new Color(0, 0, 0,Mathf.Lerp(0, 1, 0.5f) * Time.deltaTime);
        if (mask.color.a >= 0.9f)
        {
            mask.color = new Color(0, 0, 0, 1);
            toNextScene = false;
            GameManager.Get.LoadNextScript();
        }
    }
    public void Hidemask()
    {
        mask.color -= new Color(0,0,0,Mathf.Lerp(1, 0, 0.5f) * Time.deltaTime);
        if (mask.color.a <= 0.1f)
        {
            mask.color = new Color(0,0,0, 0);
            toNextScene = false;
            GameManager.Get.LoadNextScript();
        }
    }
    private void Update()
    {
        if (toNextScene)
        {
            if (showMask)
            {
                ShowMask();
            }
            else
            {
                Hidemask();
            }
        }
    }

}
