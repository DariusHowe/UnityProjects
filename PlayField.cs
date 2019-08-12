using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayField : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    //Use this for counting num of lines deleted
    public static int lineCount;
    //Creates a grid of hieght 20 and width 10
    public static Transform[,] grid = new Transform[w, h];
    public static int clearCount;
    public static int level = 1;
    public static int score;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lineText;

    //Functions keeps coordinates of rotated minos as whole numbers
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    //Making restrictions to the possible lengths the mino can move to
    public static bool InsideBorder(Vector2 pos)
    {
        //Checks if position is within the window
        return ((int)pos.x >= 0 && (int)pos.x < w && (int)pos.y >= 0);
    }
    //Deletes rows that are full and increment our clear count
    public void DeleteRow(int y)
    {
        //Put a score counter in and display the total number of lines cleared
        //Create a function that changes the speed of falling minos as lines get deleted
        for(int x=0; x < w; ++x)
        {
            //Removes that full row of minos
            Destroy(grid[x, y].gameObject);
            //Display this count after it incrememnts 
            ++clearCount;
            //creates a new empty set now that the full set is deleted
            grid[x, y] = null;
            SetText();
        }
    }

    //Moves rows down according to grid level shift
    public void DecreaseRow(int y)
    { 
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                //Reassigns new bottom row position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    
    //Moving rows Down after deletion
    public void DecreaseUp(int y)
    {
        for(int i = y; i < h; ++i)
        {
            DecreaseRow(i);
        }
    }
    //Checking if Row is full
    public bool IsRowFull(int y)
    {
        
        for (int x = 0; x < w; ++x)
        
            if (grid[x, y] == null)
            
                return false;
            
        return true;
        
    }
    //Deleting Rows
    public void DeleteFullRows()
    {
        for(int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseUp(y + 1);
                --y;
                ++lineCount;
                SetText();
            }
        }
    }
    void SetText()
    {
        SetScoreText();
        SetLevelText();
        SetLineText();
    }
    void SetLineText()
    {
        lineText.text = "Lines Cleared: " + lineCount;
    }
    void SetScoreText()
    {
        score = lineCount * 250;
        scoreText.text = "Score: " + score;
    }
    void SetLevelText()
    {
        level = (lineCount / 10) + 1;
        levelText.text = "Level " + level;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
