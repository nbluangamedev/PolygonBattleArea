using UnityEngine;

public class ButtonController : MonoBehaviour
{
    //[SerializeField] RectTransform[] buttonList;
    //[SerializeField] RectTransform indicator;

    //int indicatorPosition = 0;
    //Animator animator;

    private void Awake()
    {
        //animator = GameObject.FindGameObjectWithTag("Indicator").GetComponent<Animator>();
    }

    void Update()
    {
        //indicator.localPosition = buttonList[indicatorPosition].localPosition;
    }

    public void HoverOnButton(int buttonPos)
    {
        //indicatorPosition = buttonPos;
        AudioManager.Instance.PlaySE(AUDIO.SE_HOVERBUTTON1);
    }

    public void PressedOnButton()
    {
        //AudioManager.Instance.PlaySE(AUDIO.SE_51_FLEE);
        //animator.SetTrigger(AnimationStrings.buttonClickTrigger);
    }
}