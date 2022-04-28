using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemUI : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{

    public ItemInfo itemInfo;
    private Transform imgBagBackground;
    private CanvasGroup canvasGroup;
    private GameManager gameManager;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameManager = GameManager.Get;
    }
    /// <summary>
    /// 鼠标拖时候自动调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        imgBagBackground = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(UIManager.Instance.transform);
    }
    /// <summary>
    /// 拖拽中处理
    /// </summary>
    /// <param name="eventData"></param>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    /// <summary>
    /// 拖拽结束处理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
       GameObject pointObj = eventData.pointerEnter;
        if (pointObj == null || pointObj.tag == "Bag")//鼠标下方空白
        {
            transform.SetParent(imgBagBackground);
            transform.localPosition = Vector3.zero;
        }
        else if (pointObj.tag == "item")
        {
            if (pointObj.GetComponent<ItemUI>().itemInfo.id == itemInfo.id)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.SetParent(pointObj.transform.parent);
                transform.localPosition = Vector3.zero;
                transform.SetSiblingIndex(pointObj.transform.GetSiblingIndex());
                pointObj.transform.SetSiblingIndex(transform.parent.childCount + 1);
            }

        }
        else
        {
            transform.SetParent(imgBagBackground);
            transform.localPosition = Vector3.zero;
        }
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 使用物品
    /// </summary>
    public void UseItem()
    {
        if (gameManager.itemEffectDict.ContainsKey(itemInfo.id))
        {
            gameManager.itemEffectDict[itemInfo.id](itemInfo.src);
            gameManager.HideTipInfo();
            Destroy(gameObject);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.ShowTipInfo(itemInfo, transform.position);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.HideTipInfo();
    }
}
