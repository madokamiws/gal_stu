using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeActivities : MonoBehaviour
{

    private Dictionary<int, System.Action> chooseActivies;
    private Dictionary<int, System.Action> actActions;
    private Dictionary<string, string[]> characterDialogue;
    private string currentCharacterName;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Get;
        ShowChoiceUI();
        chooseActivies = new Dictionary<int, System.Action>()
        {
            { 1,ChooseTest},
            { 2,ChooseDebug},
            { 3,FinshActivity}
        };
        //ShowChoiceUI();
        actActions = new Dictionary<int, System.Action>()
        {
            { 1,Talk},
            { 2,GiveGifts},
            { 3,Date},
            { 4,ReturnToChoice}
        };
        characterDialogue = new Dictionary<string, string[]>()
        {
            {"Test",new string[]
                {
                    "你来找我有什么事？",
                    "你是个xxxx",
                    "谢谢大佬",
                    "12312312312",
                    "222222222"
                }
            },
            {"Debug",new string[]
                {
                    "阁下找我有什么事？",
                    "阁下击剑很棒",
                    "谢谢",
                    "击剑把",
                    "222222222"
                }
            }
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowChoiceUI()
    {
        gameManager.ShowChoiceUI(3, new string[] { "test", "Debug", "结束" }, ChooseCharacter);
    }


    private void ChooseCharacterAndHandle()
    {
        gameManager.UpdateCharacterName(currentCharacterName);
        gameManager.CloseChoiceUI();
        GameManager.Instance.DoShowOrHideCharacterTween(true, false, 1, currentCharacterName, 3);
        Invoke("PlaySoundAndShowChoiceLineUI", 1.1f);
    }
    /// <summary>
    /// 选择Test选项
    /// </summary>
    private void ChooseTest()
    {
        currentCharacterName = "Test";
        ChooseCharacterAndHandle();
    }
    private void PlaySoundAndShowChoiceLineUI()
    {
        gameManager.PlayGameSound("Single/0", 1, currentCharacterName);
        ShowChoiceLineUI();
        gameManager.AddClickButtonListener(ShowChoiceLineUI);

    }
    /// <summary>
    /// 显示与人物之间的交流行为
    /// </summary>
    private void ShowChoiceLineUI(object src=null)
    {
        gameManager.ShowChoiceLineUI(4, new string[] { "交谈", "透批", "表白", "没事了" }, Act);
    }

    private void ChooseDebug()
    {
        currentCharacterName = "Debug";
        ChooseCharacterAndHandle();
    }
    private void FinshActivity()
    {
        gameManager.CloseChoiceUI();
        gameManager.LoadNextScript();
    }
    public void ChooseCharacter(object chooseID)
    {
        chooseActivies[(int)chooseID]();
    }

    public void Act(object actID)
    {
        actActions[(int)actID]();
    }
    private void Talk()
    {
        gameManager.PlayGameSound("Single/1", 1, currentCharacterName);
        gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][1],false);
    }
    private void GiveGifts()
    {
        if (UIManager.Get.IfOwnItem(2) && currentCharacterName == "Test")
        {
            gameManager.PlayGameSound("Single/2", 1, currentCharacterName);
            gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][2], false);
            gameManager.RemoveItem(2);
            gameManager.ChangeFavourValue(20, currentCharacterName);
        }
        else
        {
            gameManager.PlayGameSound("Single/4", 1, currentCharacterName);
            gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][4], false);
        }
    }
    private void Date()
    {
        gameManager.PlayGameSound("Single/3", 1, currentCharacterName);
        gameManager.UpdateTalkLineText(characterDialogue[currentCharacterName][3], false);
    }
    private void ReturnToChoice()
    {
        gameManager.DoShowOrHideCharacterTween(false, false, 1f, currentCharacterName, 3);
        Invoke("ShowChoiceUI", 1.1f);
    }
}
