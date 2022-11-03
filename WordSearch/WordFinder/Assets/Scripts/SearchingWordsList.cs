using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject searchingWordPrefab;
    public float offset = 0.0f;
    public int maxColumns = 5;
    public int maxRows = 4;

    private int columns = 2;
    private int rows;
    private int wordsNumber;

    private List<GameObject> words = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        wordsNumber = currentGameData.selectedBoardData.SearchWords.Count;

        if (wordsNumber < columns)
        {
            rows = 1;
        }
        else
        {
            CalculateColumnsAndRowNumbers();
        }

        CreateWordsObject();
        SetWordsPosition();
    }

    private void CalculateColumnsAndRowNumbers()
    {
        do
        {
            columns++;
            rows = wordsNumber / columns;
        } while (rows >= maxRows);

        if (columns > maxColumns)
        {
            columns = maxColumns;
            rows = wordsNumber / columns;
        }
    }

    private bool TryIncreaseColumnNumber()
    {
        columns++;
        rows = wordsNumber / columns;

        if (columns > maxColumns)
        {
            columns = maxColumns;
            rows = wordsNumber / columns;
            return false;
        }

        if (wordsNumber % columns > 0)
            rows++;

        return true;
    }

    private void CreateWordsObject()
    {
        var squareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for (var index = 0; index < wordsNumber; index++)
        {
            words.Add(Instantiate(searchingWordPrefab) as GameObject);
            words[index].transform.SetParent(this.transform);
            words[index].GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);
            words[index].GetComponent<RectTransform>().localScale = squareScale;
            words[index].GetComponent<SearchingWord>().SetWord(currentGameData.selectedBoardData.SearchWords[index].word);
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

       while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;

                return finalScale;
            }
        }

        return finalScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector2(1f, 1f);

        squareSize.x = squareRect.rect.width * targetScale.x + offset;
        squareSize.y = squareRect.rect.height * targetScale.y + offset;

        var totalSquareHeight = squareSize.y * rows;

        //Make sure all sqaures fit in the parent rectangle
        if (totalSquareHeight > parentRect.rect.height)
        {
            while (totalSquareHeight > parentRect.rect.height)
            {
                if (TryIncreaseColumnNumber())
                {
                    totalSquareHeight = squareSize.y * rows;
                }
                else
                {
                    return true;
                }
            }
        }

        var totalSqaureWidth = squareSize.x * columns;

        if (totalSqaureWidth > parentRect.rect.width)
            return true;
        
        return false;
    }

    private void SetWordsPosition()
    {
        var squareRect = words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offset
        };

        int columnNumber = 0;
        int rowNumber = 0;

        var startPosition = GetFirstSquarePosition();

        foreach (var _word in words)
        {
            if (columnNumber + 1 > columns)
            {
                columnNumber = 0;
                rowNumber++;
            }

            var positionX = startPosition.x + wordOffset.x * columnNumber;
            var positionY = startPosition.y - wordOffset.y * rowNumber;

            _word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, positionY);
            columnNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = words[0].GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + offset;
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + offset;

        //Make sure they are centered
        var shiftBy = (parentRect.rect.width - (squareSize.x * columns)) / 2;
        startPosition.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftBy;
        startPosition.y = (parentRect.rect.height - squareSize.y) / 2;

        return startPosition;
    }
}
