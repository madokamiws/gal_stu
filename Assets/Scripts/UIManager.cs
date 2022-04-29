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
    public Text textTalkline; //对话框名字
    public Text choiceTalkLineTextName;//选项对话框名字
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
    public GameObject bagPanelGo;
    public bool isOpenBag;
    public GameObject imgFavorability;

    private List<ItemInfo> itemInfos;

    public Transform imgBagBackground;
    public Dictionary<int, GameObject> itemBtnGosDict;//存背包物品的字典

    public GameObject tipInfoGo;
    public Text textTipInfo;
    public GameObject choiceLineGo;//对话框中的多项选择内容
    public GameObject[] choiceLineUIGos;
    public Text[] textChoiceLineUI;
    public Button clickEventButton;
    public Button loadNextButton;
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
        //itemInfos = new List<ItemInfo>()
        //{
        //    new ItemInfo(){name = "冰棍",id = 1,instruction = "123123123" ,src = 10},
        //    new ItemInfo(){name = "kuma",id = 2,instruction = "akumanochikara",src = 15 },
        //};

        //AddItem(itemInfos[1]);
    }
    private void Start()
    {
        //GameManager.Get.SaveByJson<List<ItemInfo>>(itemInfos, "/Json/ItemInfo.json");
        itemInfos = GameManager.Get.LoadByJson<List<ItemInfo>>("/Json/ItemInfo.json");
        itemBtnGosDict = new Dictionary<int, GameObject>();
        for (int i = 0; i < itemInfos.Count; i++)
        {
            AddItem(itemInfos[i]);
        }
    }

    /// <summary>
    /// 加载并添加物品到背包（包括物品数据添加到物品数据存储字典）
    /// </summary>
    /// <param name="itemInfo"></param>
    public void AddItem(ItemInfo itemInfo)
    {
        Button btnItem = Instantiate(Resources.Load<GameObject>("Prefabs/Item/Button_item")).GetComponent<Button>();
        btnItem.transform.SetParent(imgBagBackground);
        btnItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/"+itemInfo.id);
        btnItem.GetComponent<ItemUI>().itemInfo = itemInfo;
        if (!itemBtnGosDict.ContainsKey(itemInfo.id))
        {
            itemBtnGosDict.Add(itemInfo.id, btnItem.gameObject);
        }
        //itemBtnGosDict.Add(itemInfo.id, btnItem.gameObject);
    }
    /// <summary>
    /// 从物品数据字典中移除物品
    /// </summary>
    /// <param name="id"></param>
    public void RemoveItem(int id)
    {
        if (itemBtnGosDict.ContainsKey(id))
        {
            Destroy(itemBtnGosDict[id]);
            itemBtnGosDict.Remove(id);
        }
    }
    /// <summary>
    /// 设置背景图片
    /// </summary>
    /// <param name="spriteName"></param>
    public void SetBGImgSprite(string spriteName)
    {
        imgBG.sprite = Resources.Load<Sprite>("Sprites/BG/" + spriteName);
    }
    /// <summary>
    /// 显示人物
    /// </summary>
    /// <param name="name"></param>
    public void ShowCharacter(string name, int characterID = 0)
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
    public void UpdateTalkLineText(string dialogueContent,bool loadNext = true)
    {
        CloseChoiceLineUI();
        CloseChoiceUI();
        ShowORHideTalkLine(true);
        textTalkline.text = dialogueContent;
        ShowEventButton(loadNext);
    }
    /// <summary>
    /// 设置人物位置
    /// </summary>
    /// <param name="posID"></param>
    public void SetCharacterPos(int posID, bool ifRotate = false, int characterID = 0)
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

    public void setPos(int posID, Image targetImage, bool ifRotate = false)
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
    public void ShowChoiceUI(int choiceNum, string[] choiceContent, UnityEngine.Events.UnityAction<object> unityAction)
    {
        SetChoiceUI(empChoiceUIGo,choiceUIGos, textChoiceUIs,  choiceNum, choiceContent, unityAction);
    }
    /// <summary>
    /// 关闭多项选择框
    /// </summary>
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
        DoShowOrHideUITween(show, true, 2, mask);

    }
    /// <summary>
    /// 人物渐隐渐现的动画
    /// </summary>
    /// <param name="show"></param>
    /// <param name="characterID"></param>
    public void ShowOrHideCharacterMask(bool show, int characterID)
    {
        if (characterID == 0)
        {
            DoShowOrHideUITween(show, true, 1.5f, imgCharacter);
        }
        if (characterID == 1)
        {
            DoShowOrHideUITween(show, true, 1.5f, imgCharacter2);
        }

    }

    /// <summary>
    /// 执行ui渐隐渐现的动画
    /// </summary>
    /// <param name="show"></param>
    /// <param name="ifLoadNext"></param>
    /// <param name="interval"></param>动画时间间隔
    /// <param name="image"></param>
    public void DoShowOrHideUITween(bool show, bool ifLoadNext, float interval, params Image[] image)
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
            imageTweenList.Add(new UIInfo() { show = show, imageTween = image[i], ifLoadNext = ifLoadNext, lerSpeed = 1 / interval, percent = percent });
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
            if (imageTweenList.Count <= 0)
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
    /// <summary>
    /// 打开或关闭背包
    /// </summary>
    public void OpenOrCloseBag()
    {
        isOpenBag = !isOpenBag;
        bagPanelGo.SetActive(isOpenBag);
    }
    /// <summary>
    /// 更新角色名称
    /// </summary>
    /// <param name="name"></param>
    public void UpdateCharacterName(string name)
    {
        textName.text = name;
        choiceTalkLineTextName.text = name;
        ShowOrHideFavourUI(true);
    }
    /// <summary>
    /// 是否显示好感度
    /// </summary>
    /// <param name="show"></param>
    private void ShowOrHideFavourUI(bool show)
    {
        imgFavorability.SetActive(show);
    }
    /// <summary>
    /// 显示物品信息说明框
    /// </summary>
    /// <param name="iteminfo">物品信息</param>
    /// <param name="itemPos">显示位置</param>
    public void ShowTipInfo(ItemInfo iteminfo, Vector3 itemPos)
    {
        tipInfoGo.SetActive(true);
        tipInfoGo.transform.position = itemPos;
        textTipInfo.text = "<color=#FFE7D6>"+iteminfo.name + "</color>"+"\n" + "<color=#F7F8D6>"+ iteminfo.instruction + "</color>";
    }
    public void HideTipInfo()
    {
        tipInfoGo.SetActive(false);
    }
    /// <summary>
    /// 显示对话框中的多项选择
    /// </summary>
    public void ShowChoiceLineUI(int choiceNum, string[] choiceContent, UnityEngine.Events.UnityAction<object> unityAction)
    {
        SetChoiceUI(choiceLineGo, choiceLineUIGos, textChoiceLineUI, choiceNum, choiceContent, unityAction);
    }
    private void CloseChoiceLineUI()
    {
        choiceLineGo.SetActive(false);
    }
    /// <summary>
    /// 设置选择类型的UI方法
    /// </summary>
    /// <param name="choiceUIGo">选择Ui的父对象物体</param>
    /// <param name="choiceUIGos">显示多少选项物体</param>
    /// <param name="choiceUIGoTexts">选择UI文本的引用</param>
    /// <param name="choiceNum">有几个选项</param>
    /// <param name="choiceContent">按钮上显示的文本内容</param>
    /// <param name="unityAction">点击按钮后触发的回调事件</param>
    public void SetChoiceUI(GameObject choiceUIGo,GameObject[] choiceUIGos,Text[] choiceUIGoTexts,int choiceNum,string[] choiceContent,UnityEngine.Events.UnityAction<object> unityAction)
    {
        ShowORHideTalkLine(false);
        CloseChoiceUI();
        CloseChoiceLineUI();
        choiceUIGo.SetActive(true);

        for (int i = 0; i < choiceUIGos.Length; i++)
        {
            choiceUIGos[i].SetActive(false);
            choiceUIGos[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 0; i < choiceNum; i++)
        {
            choiceUIGos[i].SetActive(true);
            choiceUIGoTexts[i].text = choiceContent[i];

            int src = i;
            choiceUIGos[i].GetComponent<Button>().onClick.AddListener(() => { unityAction(src + 1); });

        }
    }
    /// <summary>
    /// 对话框选择按钮触发事件注册
    /// </summary>
    /// <param name="unityAction"></param>
    public void AddClickButtonListener(UnityEngine.Events.UnityAction<object> unityAction)
    {
        clickEventButton.onClick.RemoveAllListeners();
        clickEventButton.onClick.AddListener(() => { unityAction(null); });
    }
    /// <summary>
    /// 显示加载下一个剧本的按钮还是其他回调事件按钮
    /// </summary>
    /// <param name="loadNext"></param>
    public void ShowEventButton(bool loadNext = true)
    {
        if (loadNext)
        {
            loadNextButton.gameObject.SetActive(true);
            clickEventButton.gameObject.SetActive(false);
        }
        else
        {
            loadNextButton.gameObject.SetActive(false);
            clickEventButton.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 判断是不是拥有这件物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IfOwnItem(int id)
    {
        return itemBtnGosDict.ContainsKey(id);
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
public struct ItemInfo
{
    public string name;
    public int id;
    public string instruction;
    public object src;
}
