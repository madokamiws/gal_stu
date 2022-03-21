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
    //public bool showMask;//显示或隐藏遮罩
    public Image mask;//全屏遮罩
    //private Image imageTween;//需要做ui动画遮罩的图片对象
    private List<UIInfo> imageTweenList;//需要做ui动画遮罩的图片对象
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
        imageTweenList = new List<UIInfo>();
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

        CloseChoiceUI();
        textName.text = name;
        ShowORHideTalkLine(false);
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
        ShowORHideTalkLine(true);
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
        if (show)
        {
            CloseChoiceUI();
        }
        talkLineGo.SetActive(show);
    }
    /// <summary>
    /// 显示或隐藏遮罩
    /// </summary>
    /// <param name="show">true显示</param>
    public void ShowOrHideMask(bool show)
    {
        DoShowOrHideUITween(show, true,2,mask);

    }
    /// <summary>
    /// 人物渐隐渐现的动画
    /// </summary>
    /// <param name="show"></param>
    /// <param name="characterID"></param>
    public void ShowOrHideCharacterMask(bool show,int characterID)
    {
        if (characterID == 0)
        {
            DoShowOrHideUITween(show, true,1.5f,imgCharacter);
        }
        if (characterID == 1)
        {
            DoShowOrHideUITween(show, true,1.5f,imgCharacter2);
        }

    }

    /// <summary>
    /// 执行ui渐隐渐现的动画
    /// </summary>
    /// <param name="show"></param>
    /// <param name="ifLoadNext"></param>
    /// <param name="interval"></param>动画时间间隔
    /// <param name="image"></param>
    public void DoShowOrHideUITween(bool show, bool ifLoadNext,float interval, params Image[] image)
    {
        toNextScene = true;
        float percent;
        if (show)
        {
            percent = 0;
        }
        else
        {
            percent = 1;
        } 
        for (int i = 0; i < image.Length; i++)
        {
            imageTweenList.Add(new UIInfo() { show = show, imageTween = image[i], ifLoadNext = ifLoadNext,lerSpeed=1/ interval,percent = percent });
        }
       
    }
    public void ShowUi(UIInfo info)
    {
        info.percent += info.lerSpeed * Time.deltaTime;
        info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b,
            info.percent);
        if (info.imageTween.color.a >= 0.995f)
        {
            info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b, 1);
            if (info.ifLoadNext)
            {
                GameManager.Get.LoadNextScript();
            }
            imageTweenList.Remove(info);
            if (imageTweenList.Count <= 0)
            {
                toNextScene = false;
            }

        }
    }
    public void HideUi(UIInfo info)
    {
        info.percent -= info.lerSpeed * Time.deltaTime;
        info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b,
            info.percent);
        if (info.imageTween.color.a <= 0.005f)
        {
            info.imageTween.color = new Color(info.imageTween.color.r, info.imageTween.color.g, info.imageTween.color.b, 0);

            if (info.ifLoadNext)
            {
                GameManager.Get.LoadNextScript();
            }
            imageTweenList.Remove(info);
            if (imageTweenList.Count<=0)
            {
                toNextScene = false;
            }
        }
    }
    private void Update()
    {
        if (toNextScene)
        {
            for (int i = 0; i < imageTweenList.Count; i++)
            {
                if (imageTweenList[i].show)
                {
                    ShowUi(imageTweenList[i]);
                }
                else
                {
                    HideUi(imageTweenList[i]);
                }
            }
        }
    }

}

public class UIInfo
{
    public bool show;
    public Image imageTween;
    public bool ifLoadNext;
    public float percent;
    public float lerSpeed;
}
