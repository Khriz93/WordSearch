using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWord : MonoBehaviour
{
    public GameData currentGameData;
    public GameLevelData levelData;
    public AudioSource nextCategoryAudio;
    private string word;
    private string gameSceneName = "GameScene";
    private int assignedPoints = 0;
    private int completedWords = 0;
    private Ray rayUp, rayDown;
    private Ray rayLeft, rayRight;
    private Ray rayDiagonialLeftUp, rayDiagonalLeftDown;
    private Ray rayDiagonialRightUp, rayDiagonalRightDown;
    private Ray currentRay = new Ray();
    private Vector3 rayStartPosition;
    private List<int> correctSqaureList = new List<int>();

    private void OnEnable()
    {
        AdManager.OnInterstitialClosed += IntersititialAdClosed;
        GameEvents.OnCheckSqaure += SelectedSquare;
        GameEvents.OnClearSelection += ClearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;
    }

    private void OnDisable()
    {
        AdManager.OnInterstitialClosed -= IntersititialAdClosed;
        GameEvents.OnCheckSqaure -= SelectedSquare;
        GameEvents.OnClearSelection -= ClearSelection;
        GameEvents.OnLoadNextLevel -= LoadNextGameLevel;
    }

    private void IntersititialAdClosed()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        nextCategoryAudio = GetComponent<AudioSource>();
        currentGameData.selectedBoardData.ClearData();
        assignedPoints = 0;
        completedWords = 0;
        AdManager.Instance.ShowBanner();
    }

    // Update is called once per frame
    void Update()
    {
        //To see the direction of the ray within the scene window
        if (assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(rayUp.origin, rayUp.direction * 4);
            Debug.DrawRay(rayDown.origin, rayDown.direction * 4);
            Debug.DrawRay(rayLeft.origin, rayLeft.direction * 4);
            Debug.DrawRay(rayRight.origin, rayRight.direction * 4);
            Debug.DrawRay(rayDiagonialLeftUp.origin, rayDiagonialLeftUp.direction * 4);
            Debug.DrawRay(rayDiagonalLeftDown.origin, rayDiagonalLeftDown.direction * 4);
            Debug.DrawRay(rayDiagonialRightUp.origin, rayDiagonialRightUp.direction * 4);
            Debug.DrawRay(rayDiagonalRightDown.origin, rayDiagonalRightDown.direction * 4);
        }
    }

    private void LoadNextGameLevel()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void SelectedSquare(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (assignedPoints == 0)
        {
            rayStartPosition = squarePosition;
            correctSqaureList.Add(squareIndex);
            word += letter;

            //Getting the correct direction for the ray
            rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, 1));
            rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, -1));
            rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 0f));
            rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 0f));
            rayDiagonialLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 1));
            rayDiagonalLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, -1));
            rayDiagonialRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 1));
            rayDiagonalRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, -1));
        }
        else if (assignedPoints == 1)
        {
            correctSqaureList.Add(squareIndex);
            currentRay = SelectRay(rayStartPosition, squarePosition);
            GameEvents.SelectSquareMethod(squarePosition);
            word += letter;
            WordCheck();
        }
        else
        {
            if (IsPointOnTheRay(currentRay, squarePosition))
            {
                correctSqaureList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                word += letter;
                WordCheck();
            }
        }

        assignedPoints++;
    }

    private void WordCheck()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.SearchWords)
        {
            if (word == searchingWord.word && searchingWord.found == false)
            {
                searchingWord.found = true;
                GameEvents.CorrectWordMethod(word, correctSqaureList);
                completedWords++;
                word = string.Empty;
                correctSqaureList.Clear();
                CheckBoardCompleted();
                return;
            }
        }
    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);
        for (int i =0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
            {
                return true;
            }
        }

        return false;
    }

    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPositon)
    {
        var direction = (secondPositon - firstPosition).normalized;
        float tolerance = 0.01f;
        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - 1f) < tolerance)
        {
            return rayUp;
        }
        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - (-1f)) < tolerance)
        {
            return rayDown;
        }
        if (Mathf.Abs(direction.x - (-1f)) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return rayLeft;
        }
        if (Mathf.Abs(direction.x - 1f) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return rayRight;
        }
        if (direction.x < 0f && direction.y > 0f)
        {
            return rayDiagonialLeftUp;
        }
        if (direction.x < 0f && direction.y < 0f)
        {
            return rayDiagonalLeftDown;
        }
        if (direction.x > 0f && direction.y > 0f)
        {
            return rayDiagonialRightUp;
        }
        if (direction.x > 0f && direction.y < 0f)
        {
            return rayDiagonalRightDown;
        }

        return rayDown;
    }

    private void ClearSelection()
    {
        assignedPoints = 0;
        correctSqaureList.Clear();
        word = string.Empty;
    }

    private void CheckBoardCompleted()
    {
        bool loadNextCategory = false;

        if (currentGameData.selectedBoardData.SearchWords.Count == completedWords)
        {
            //Save current level progress
            var categoryName = currentGameData.SelectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCategoryIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;

            for (int index = 0; index < levelData.data.Count; index++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCategoryIndexValues(levelData.data[index].categoryName);
                    readNextLevelName = false;
                }

                if (levelData.data[index].categoryName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = index;
                }
            }

            var currentLevelSize = levelData.data[currentCategoryIndex].boardData.Count;
            if (currentBoardIndex < currentLevelSize)
                currentBoardIndex++;

            DataSaver.SaveCategoryData(categoryName, currentBoardIndex);

            //Unlock Next Category
            if (currentBoardIndex >= currentLevelSize)
            {
                currentCategoryIndex++;

                if (currentCategoryIndex < levelData.data.Count)
                {
                    categoryName = levelData.data[currentCategoryIndex].categoryName;
                    currentBoardIndex = 0;
                    loadNextCategory = true;

                    if (nextBoardIndex <= 0)
                    {
                        DataSaver.SaveCategoryData(categoryName, currentBoardIndex);
                    }
                }
                else
                {
                    SceneManager.LoadScene("SelectCategory");
                }
            }

            //winBox.SetActive(true);
            GameEvents.BoardCompletedMethod();
            
            if (loadNextCategory)
            {
                //nextCategoryAudio.Play();
                GameEvents.UnlockNextCategoryMethod();
            }
        }
    }
}
