using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnlockNewCategoryPopUp : MonoBehaviour
{
    [System.Serializable]
    public struct CategoryName
    {
        public string name;
        public Sprite sprite;
    };

    public GameData currentGameData;
    public List<CategoryName> categoryNames;
    private WinBox winBoxScript;
    public GameObject winBoxObject;
    public Image categoryNameImage;
    public AudioSource nextCategoryAudio;

    // Start is called before the first frame update
    void Start()
    {
        winBoxScript = GetComponent<WinBox>();
        nextCategoryAudio = GetComponent<AudioSource>();
        winBoxObject.SetActive(false);
        GameEvents.OnUnlockNextCategory += OnUnlockNextCategory;
    }

    private void OnDisable()
    {
        GameEvents.OnUnlockNextCategory -= OnUnlockNextCategory;
    }

    public void OnUnlockNextCategory()
    {
        bool captureNext = false;

        foreach (var writing in categoryNames)
        {
            if (captureNext)
            {
                categoryNameImage.sprite = writing.sprite;
                captureNext = false;
                break;
            }
            if (writing.name == currentGameData.SelectedCategoryName)
            {
                captureNext = true;
            }
        }

        winBoxScript.winAudio.Stop();
        nextCategoryAudio.Play();
        winBoxObject.SetActive(true);
    }
}
