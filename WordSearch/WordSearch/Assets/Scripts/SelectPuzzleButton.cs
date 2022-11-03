using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectPuzzleButton : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public Text categoryText;
    public Image progressBarFill;
    private string gameSceneName = "GameScene";
    private bool levelLocked;

    // Start is called before the first frame update
    void Start()
    {
        levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.interactable = true;
        UpdateButtonInfo();

        if (levelLocked)
         button.interactable = false;
        else
            button.interactable = true;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void UpdateButtonInfo()
    {
        var currentIndex = -1;
        var totalBoards = 0;

        foreach (var data in levelData.data)
        {
            if (data.categoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCategoryIndexValues(gameObject.name);
                totalBoards = data.boardData.Count;

                if (levelData.data[0].categoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCategoryData(levelData.data[0].categoryName, 0);
                    currentIndex = DataSaver.ReadCategoryIndexValues(gameObject.name);
                    totalBoards = data.boardData.Count;
                }
            }
        }

        if (currentIndex == -1)
            levelLocked = true;

        categoryText.text = levelLocked ? string.Empty : (currentIndex.ToString() + "/" + totalBoards.ToString());
        progressBarFill.fillAmount = (currentIndex > 0 && totalBoards > 0) ? ((float) currentIndex / (float) totalBoards) : 0f;
    }

private void OnButtonClick()
    {
        gameData.SelectedCategoryName = gameObject.name;

        SceneManager.LoadScene(gameSceneName);
    }
}
