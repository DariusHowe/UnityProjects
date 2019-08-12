using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groups : MonoBehaviour
{
    
    float lastFall = 0;
    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = PlayField.roundVec2(child.position);
            //
            if (!PlayField.InsideBorder(v))
                return false;

            if (PlayField.grid[(int)v.x, (int)v.y] != null &&
                PlayField.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }
    void UpdateGrid()
    {
        for (int y = 0; y < PlayField.h; ++y)
            for (int x = 0; x < PlayField.w; ++x)
                if (PlayField.grid[x, y] != null)
                    if (PlayField.grid[x, y].parent == transform)
                        PlayField.grid[x, y] = null;
        foreach (Transform child in transform)
        {
            Vector2 v = PlayField.roundVec2(child.position);
            PlayField.grid[(int)v.x, (int)v.y] = child;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!IsValidGridPos())
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Each argument checks the input on the keyboard and moves the tetrimino accordingly
        //Left movements
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidGridPos())
                UpdateGrid();
            else
                transform.position += new Vector3(1, 0, 0);
        }
        //Right Movements
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidGridPos())
                UpdateGrid();
            else
                transform.position += new Vector3(-1, 0, 0);
        }
        //Rotating
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Spins the tetrimino 90 degrees on the Z Axis (in 2D changes orientation, in 3D spins the cube on that axis)
            transform.Rotate(0, 0, -90);
            if (IsValidGridPos())
                UpdateGrid();
            else
                transform.Rotate(0, 0, 90);

        }
        //Down Direction
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            transform.position += new Vector3(0, -1, 0);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                GameObject.Find("GameManager").GetComponent<PlayField>().DeleteFullRows();
                FindObjectOfType<Spawner>().spawnNext();
                enabled = false;
            }


            lastFall = Time.time;
        }
    }
}
