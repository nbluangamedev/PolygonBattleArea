using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void HoverOnButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTONROLLOVER);
        }
    }

    public void PressedOnButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTONCLICKRELEASE);
        }
    }
}