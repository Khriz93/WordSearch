using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMuteButton : MonoBehaviour
{
    public enum ButtonType
    {
        BackgroundMusic,
        SoundFX
    };

    public ButtonType buttonType;
    public Sprite onSprite;
    public Sprite offSprite;
    public GameObject button;
    public Vector3 offButtonPos;

    private Vector3 onButtonPos;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = onSprite;
        onButtonPos = button.GetComponent<RectTransform>().anchoredPosition;
        ToggleButton();
    }

    public void ToggleButton()
    {
        var muted = false;
        if (buttonType == ButtonType.BackgroundMusic)
            muted = SoundManager.instance.IsBackgroundMusicMuted();
        else
            muted = SoundManager.instance.IsSoundFXMuted();

        if (muted)
        {
            image.sprite = offSprite;
            button.GetComponent<RectTransform>().anchoredPosition = offButtonPos;
        }
        else
        {
            image.sprite = onSprite;
            button.GetComponent<RectTransform>().anchoredPosition = onButtonPos;
        }
    }
}
