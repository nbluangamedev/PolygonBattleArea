using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void HoverOnButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_HOVERBUTTON1);
        }
    }

    public void PressedOnButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_CONFIRMBUTTON);
        }
    }
}