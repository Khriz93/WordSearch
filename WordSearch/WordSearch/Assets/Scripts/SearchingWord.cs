using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public Text displayText;
    public Image crossLine;
    private string word;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord;
    }
    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWord(string word)
    {
        this.word = word;
        displayText.text = word;
    }

    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (word == this.word)
        {
            crossLine.gameObject.SetActive(true);
        }
    }
}
