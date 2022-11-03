using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSqauare : MonoBehaviour
{
    public int squareIndex { get; set; }
    private AlphabetData.LetterData _normalLetterData;
    private AlphabetData.LetterData _selectedLetterData;
    private AlphabetData.LetterData _correctLetterData;
    private SpriteRenderer displayedImage;
    private bool _selected;
    private bool _clicked;
    private bool correct;
    private int _index = -1;
    private AudioSource audioSource;
    public GameOverScreen gameOverScreen;
    public AudioClip clip;


    public void SetIndex(int index)
    {
        _index = index;
    }

    public int GetIndex()
    {
        return _index;
    }

    // Start is called before the first frame update
    void Start()
    {
        _selected = false;
        _clicked = false;
        correct = false;
        displayedImage = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSqaureSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += SelectSqaure;
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSqaureSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= SelectSqaure;
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (_selected && squareIndexes.Contains(_index))
        {
            if (SoundManager.instance.IsSoundFXMuted() == false)
                audioSource.PlayOneShot(clip, 0.2f);

            correct = true;
            displayedImage.sprite = _correctLetterData.image;
        }

        _selected = false;
        _clicked = false;
    }

    public void OnEnableSqaureSelection()
    {
        _clicked = true;
        _selected = false;
    }

    public void OnDisableSquareSelection()
    {
        _selected = false;
        _clicked = false;

        if (correct == true)
        {
            displayedImage.sprite = _correctLetterData.image;
        }
        else
            displayedImage.sprite = _normalLetterData.image;
    }

    private void SelectSqaure(Vector3 position)
    {
        if (this.gameObject.transform.position == position)
        {
            displayedImage.sprite = _selectedLetterData.image;
        }
    }

    public void SetSprite(AlphabetData.LetterData normalLetterData, AlphabetData.LetterData selectedLetterData, AlphabetData.LetterData correctLetterData)
    {
        _normalLetterData = normalLetterData;
        _selectedLetterData = selectedLetterData;
        _correctLetterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = _normalLetterData.image;
    }

    private void OnMouseDown()
    {
        OnEnableSqaureSelection();
        GameEvents.EnbaleSquareSelectionMethod();
        CheckSquare();
        displayedImage.sprite = _selectedLetterData.image;
    }

    private void OnMouseEnter()
    {
        CheckSquare();
    }

    private void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }

    public void CheckSquare()
    {
        if (_selected == false && _clicked == true)
        {
            if (SoundManager.instance.IsSoundFXMuted() == false)
                audioSource.Play();

            _selected = true;
            GameEvents.CheckSquareMethod(_normalLetterData.letter, gameObject.transform.position, _index);
        }
    }
}
