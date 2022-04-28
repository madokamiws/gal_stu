using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;
using Live2D.Cubism.Framework.MouthMovement;
using Live2D.Cubism.Framework.Expression;
public class CubismObject : MonoBehaviour
{

    private CubismRenderController cubismRenderController;
    private CubismAudioMouthInput cubismAudioMouthInput;
    private CubismExpressionController cubismexpressionController;
    private Animator animator;
    private CubismMouthController cubismMouthController;
    // Start is called before the first frame update
    void Awake()
    {
        cubismRenderController = transform.GetChild(0).GetComponent<CubismRenderController>();
        cubismAudioMouthInput = transform.GetChild(0).GetComponent<CubismAudioMouthInput>();
        cubismexpressionController = transform.GetChild(0).GetComponent<CubismExpressionController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        cubismMouthController = transform.GetChild(0).GetComponent<CubismMouthController>();
        //animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetOpacityValue(float value)
    {
        cubismRenderController.Opacity = value;
    }
    public float GetOpacityValue()
    {
        return cubismRenderController.Opacity;
    }
    public void Talk(bool isTalking)
    {
        if (isTalking)
        {
            cubismAudioMouthInput.AudioInput = GameManager.Get.GetDialogueAudio();
        }
        else
        {
            cubismAudioMouthInput.AudioInput = null;
            cubismMouthController.MouthOpening = 0;
        }
    }
    public void PlayExpression(int index)
    {
        cubismexpressionController.CurrentExpressionIndex = index;
    }
    public void EnableAnimator(bool enabledValue)
    {
        animator.enabled = enabledValue;
    }
    public void PlayMotion(int animationNum)
    {
        animator.SetTrigger(animationNum.ToString());
    }

}
