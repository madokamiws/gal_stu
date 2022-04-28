using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubismManager : MonoBehaviour
{

    public static CubismManager Instance { get; private set; }

    private Dictionary<string, CubismObject> characterDict;

    public Transform[] characterPos;

    public List<CharacterInfo> characterTweenList;
    private bool startShowCharacterTween;
    private Dictionary<string, CubismObject> currentCharacteDict;
    public static CubismManager Get
    {
        get
        {
            if (!Instance)
            {
                Instance = new CubismManager();
            }
            return Instance;
        }
    }
    // Start is called before the first frame update


    private void Awake()
    {
        Instance = this;
        characterDict = new Dictionary<string, CubismObject>();
        characterTweenList = new List<CharacterInfo>();
        currentCharacteDict = new Dictionary<string, CubismObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startShowCharacterTween)
        {
            for (int i = 0; i < characterTweenList.Count; i++)
            {
                if (characterTweenList[i].show)
                {
                    ShowCharacter(characterTweenList[i]);
                }
                else
                { 
                    HideCharacter(characterTweenList[i]);
                }
            }
        }
    }
    /// <summary>
    /// 显示人物加载模型
    /// </summary>
    /// <param name="name"></param>
    public void ShowCharacter(string name)
    {
        if (name == null)
        {
            return;
        }
        if (characterDict.ContainsKey(name))
        {
            characterDict[name].gameObject.SetActive(true);
        }
        else
        {
            characterDict.Add(name, Instantiate(Resources.Load<GameObject>
                ("Prefabs/Character/" + name)).transform.GetComponent<CubismObject>());
            
        }
    }
    /// <summary>
    /// 设置人物位置
    /// </summary>
    /// <param name="posID">位置id</param>
    /// <param name="name"></param>
    public void SetCharacterPos(int posID,string name)
    {
        if (name == null)
        {
            return;
        }
        if (characterDict.ContainsKey(name))
        {
            characterDict[name].transform.SetParent(characterPos[posID-1]);
            characterDict[name].transform.localPosition = Vector3.zero;
        }
        
    }
    /// <summary>
    /// 人物进退场动画
    /// </summary>
    /// <param name="show"></param>是否显示
    /// <param name="ifLoadNext"></param>是否加载加个剧本
    /// <param name="interval"></param>间隔时间
    /// <param name="cubismObject"></param>
    public void DoShowOrHideCharacterTween(bool show,bool ifLoadNext,float interval, string name,int posID)
    {
        StartCoroutine(DoShowOrHideCharacter(show,ifLoadNext,interval,name,posID));
    }
    private IEnumerator DoShowOrHideCharacter(bool show, bool ifLoadNext, float interval, string name,int posID)
    {
        float percent;
        CubismObject cubismObject;
        if (characterDict.ContainsKey(name))
        {
            cubismObject = characterDict[name];
        }
        else
        {
            cubismObject = Instantiate(Resources.Load<GameObject>
                ("Prefabs/Character/" + name)).GetComponent<CubismObject>();
            characterDict.Add(name, cubismObject);
        }
        if (show)
        {
            //设置到屏幕外
            SetCharacterPos(4, name);
        }
        yield return new WaitForSeconds(0.1f);
        //当前剧本需要显示到的位置
        SetCharacterPos(posID, name);
        cubismObject.EnableAnimator(false);
        if (show)
        {
            percent = 0;
            cubismObject.SetOpacityValue(percent);
            currentCharacteDict.Add(name, cubismObject);
        }
        else
        {
            percent = 1;
            currentCharacteDict.Remove(name);
        }
        characterTweenList.Add(new CharacterInfo()
        {
            show = show,
            ifLoadNext = ifLoadNext,
            percent = percent,
            lerpSpeed = 1 / interval,
            cubismObject = cubismObject
        });
        startShowCharacterTween = true;
    }
    /// <summary>
    /// 人物进场
    /// </summary>
    /// <param name="characterInfo"></param>
    public void ShowCharacter(CharacterInfo characterInfo)
    {
        if (name == null)
        {
            return;
        }
        characterInfo.percent += characterInfo.lerpSpeed * Time.deltaTime;
        characterInfo.cubismObject.SetOpacityValue(characterInfo.percent);
        if (characterInfo.cubismObject.GetOpacityValue()>=0.995f)
        {
            characterInfo.cubismObject.SetOpacityValue(1);
            characterInfo.cubismObject.EnableAnimator(true);
            if (characterInfo.ifLoadNext)
            {
                GameManager.Get.LoadNextScript();
            }
            characterTweenList.Remove(characterInfo);
            if (characterTweenList.Count<=0)
            {
                startShowCharacterTween = false;
            }
        }
    }
    /// <summary>
    /// 人物退场
    /// </summary>
    /// <param name="characterInfo"></param>
    public void HideCharacter(CharacterInfo characterInfo)
    {
        characterInfo.percent -= characterInfo.lerpSpeed * Time.deltaTime;
        characterInfo.cubismObject.SetOpacityValue(characterInfo.percent);
        if (characterInfo.cubismObject.GetOpacityValue() <= 0.005f)
        {
            characterInfo.cubismObject.SetOpacityValue(0);
            if (characterInfo.ifLoadNext)
            {
                GameManager.Get.LoadNextScript();
            }
            characterTweenList.Remove(characterInfo);
            if (characterTweenList.Count <= 0)
            {
                startShowCharacterTween = false;
            }
        }
    }
    /// <summary>
    /// 唇形同步
    /// </summary>
    /// <param name="name"></param>
    public void Talk(string name)
    {
        if (!currentCharacteDict.ContainsKey(name))
        {
            return;
        }
        foreach (var item in currentCharacteDict)
        {
            item.Value.Talk(false);
        }
        currentCharacteDict[name].Talk(true);
    }
    /// <summary>
    /// 播放动作
    /// </summary>
    /// <param name="name"></param>
    /// <param name="animationNum"></param>
    public void PlayMotion(string name,int animationNum)
    {

        foreach (var item in currentCharacteDict)
        {
            if (item.Key != name)
            {
                item.Value.PlayMotion(0);
            }
            else
            {
                currentCharacteDict[name].PlayMotion(animationNum);
            }
        }

    }
    /// <summary>
    /// 表情
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index"></param>
    public void PlayExpression(string name, int index)
    {
        currentCharacteDict[name].PlayExpression(index);
    }

}
public class CharacterInfo
{
    public bool show;//显示隐藏
    public CubismObject cubismObject;
    public bool ifLoadNext;//加载完毕后是否加载下一个剧本
    public float percent;//进度
    public float lerpSpeed;//速度
}