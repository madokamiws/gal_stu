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
    // Start is called before the first frame update
    void Awake()
    {
        cubismRenderController = transform.GetChild(0).GetComponent<CubismRenderController>();
        cubismAudioMouthInput = transform.GetChild(0).GetComponent<CubismAudioMouthInput>();
        cubismexpressionController = transform.GetChild(0).GetComponent<CubismExpressionController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
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
            cubismAudioMouthInput.AudioInput = AudioSouceManager.Get.dialogueAudio;
        }
        else
        {
            cubismAudioMouthInput.AudioInput = null;
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
}
