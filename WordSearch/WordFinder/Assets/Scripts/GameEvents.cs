using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void EnableSquareSelection();
    public static event EnableSquareSelection OnEnableSquareSelection;

    public static void EnbaleSquareSelectionMethod()
    {
        if (OnEnableSquareSelection != null)
            OnEnableSquareSelection();
    }

    //*********************************************************************

    public delegate void DisableSquareSelection();
    public static event DisableSquareSelection OnDisableSquareSelection;

    public static void DisableSquareSelectionMethod()
    {
        if (OnDisableSquareSelection != null)
            OnDisableSquareSelection();
    }

    //*********************************************************************

    public delegate void SelectSqaure(Vector3 position);
    public static event SelectSqaure OnSelectSquare;

    public static void SelectSquareMethod(Vector3 position)
    {
        if (OnSelectSquare != null)
            OnSelectSquare(position);
    }

    //*********************************************************************

    public delegate void CheckSqaure(string letter, Vector3 squarePosition, int squareIndex);
    public static event CheckSqaure OnCheckSqaure;

    public static void CheckSquareMethod(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (OnCheckSqaure != null)
            OnCheckSqaure(letter, squarePosition, squareIndex);
    }

    //*********************************************************************

    public delegate void ClearSelection();
    public static event ClearSelection OnClearSelection;

    public static void ClearSelectionMethod()
    {
        if (OnClearSelection != null)
            OnClearSelection();
    }

    //*********************************************************************

    public delegate void CorrectWord(string word, List<int> squareIndexes);
    public static event CorrectWord OnCorrectWord;
    
    public static void CorrectWordMethod(string word, List<int> squareIndexes)
    {
        if (OnCorrectWord != null)
        {
            OnCorrectWord(word, squareIndexes);
        }
    }

    //*********************************************************************

    public delegate void BoardCompleted();
    public static event BoardCompleted OnBoardCompleted;

    public static void BoardCompletedMethod()
    {
        if (OnBoardCompleted != null)
            OnBoardCompleted();
    }

    //*********************************************************************

    public delegate void UnlockNextCategory();
    public static event UnlockNextCategory OnUnlockNextCategory;

    public static void UnlockNextCategoryMethod()
    {
        if (OnUnlockNextCategory != null)
            OnUnlockNextCategory();
    }

    //*********************************************************************

    public delegate void LoadNextLevel();
    public static event LoadNextLevel OnLoadNextLevel;

    public static void LoadNextLevelMethod()
    {
        if (OnLoadNextLevel != null)
            OnLoadNextLevel();
    }

    //*********************************************************************

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public static void GameOverMethod()
    {
        if (OnGameOver != null)
            OnGameOver();
    }

    //*********************************************************************

    public delegate void ToggleSoundFX();
    public static event ToggleSoundFX OnToggleSoundFX;

    public static void ToggleSoundFXMethod()
    {
        if (OnToggleSoundFX != null)
            OnToggleSoundFX();
    }
}
