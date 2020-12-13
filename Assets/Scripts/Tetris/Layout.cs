using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layout : MonoBehaviour
{
    public Transform layout_sprite;
    public int layout_height = 30;
    public int layout_width = 10;

    public int layout_header = 8;   //space to spawn blocks

     //datastructure to store blocks in each grid
    Transform[,] layout_grid;  

    public int completed_rows = 0;  //total number of cleared rows counter
    public int num_rows = 0;

    //run before Start
    void Awake() {
        layout_grid = new Transform[layout_width, layout_height];
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //takes in two parameter and set as layout width boundary
    bool isInLayout(int x, int y) {
        return (x >= 0 && x < layout_width && y >= 0);
    }

    //checks if the block overlaps
    bool isOverlay(int x, int y, Blocks block)
    {
        return (layout_grid[x,y] != null && layout_grid[x,y].parent != block.transform);
    }

    /*
    check if each block inside the layout is inside the width boundary
    */
    public bool isValidPosition (Blocks block)
    {
        //loop to check if block is in boundary and if it overlaps
        foreach (Transform child in block.transform)
        {
            Vector2 position = Vectorf.Round(child.position);

            if (!isInLayout((int) position.x, (int) position.y))
            {
                return false;
            }

            if (isOverlay((int) position.x, (int) position.y, block))
            {
                return false;
            }
        }
        return true;
    }

    //use loops and square sprite to create cell table
    private void CreateCells() 
    {   
        if (layout_sprite != null)     //check if sprite field is null
        {
            for (int x = 0; x < layout_width; x++) 
                {   //loop for layer width
                for (int y = 0; y < layout_height - layout_header; y++) 
                    {   //loop for layer height
                    /*
                        inner loop handles creation of squares according to x and y var and set z to always be 0
                        Create clone Instantiate of square sprite and rename them according to their x and y positions
                        hide all the children clone under parent
                    */
                        Transform clone;
					    clone = Instantiate(layout_sprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
					    clone.name = "Board Space ( x = " + x.ToString() +  " , y =" + y.ToString() + " )"; 
					    clone.transform.parent = transform;
                    }
                }  
        }
        else
        {
            Debug.Log("Error! Assign sprite object!");
        }
    }

    /*
        store the block into the layout_grid array as a child class
    */
    public void StoreBlock(Blocks block)
    {
        if (block == null)
        {
            return;
        }

        foreach (Transform child in block.transform)
        {
            Vector2 position = Vectorf.Round(child.position);
            layout_grid[(int) position.x, (int)position.y] = child;
        }
    }

    /*
        is row complete
    */
    bool isComplete(int y)
    {
        //for loop to check each row and see if its filled out
        for (int x = 0; x < layout_width; x++)
        {
            if (layout_grid[x,y] == null)
            {
                return false;
            }
        }
        return true;
    }

    /*
        clear row method
    */
    void ClearRow(int y)
    {
        //for loop to destroy layout_grid with full row
        for (int x = 0; x < layout_width; x++)
        {
            if (layout_grid[x,y] != null)
            {
                Destroy(layout_grid[x,y].gameObject);
            }
            layout_grid[x,y] = null; //free up the row destroyed in game datastructure
        }
    }

    /*
        shift row down after row clear method
    */
    void ShiftDown(int y)
    {
        //for loop to check complete rows and copy it down
        for (int x = 0; x < layout_width; x++)
        {
            if (layout_grid[x,y] != null)
            {
                layout_grid[x,y - 1] = layout_grid[x,y];    //set row under = to row above
                layout_grid[x,y] = null; //free up the row destroyed in game datastructure
                layout_grid[x,y - 1].position += new Vector3(0 , -1, 0);    //allow the drop transform to continue
            }
            
        }
    }

    /*
        shift multiple row
    */
    void ShiftRowsDown(int y)
    {
        //for loop to copy rows down from wanted row, if its complete
        for (int i = y; i < layout_height; i++)
        {
            ShiftDown(i);    
        }
    }

    /*
        recursive method to clear multiple rows at once
    */
    public void ClearRows()
    {
        num_rows = 0;   //reset row clear counter to zero
        //for loop to copy rows down from wanted row, if its complete
        for (int i = 0; i < layout_height; i++)
        {
            if (isComplete(i))
            {
                completed_rows++;
                ClearRow(i);
                ShiftRowsDown(i + 1);
                i--;
                num_rows++;
            }
        }
    }

    /*
        method to check if block went over limit
        loops through and check if each child block passes layout height
    */
    public bool IsOverLimit(Blocks block)
	{
		foreach (Transform child in block.transform) 
		{
			if (child.transform.position.y >= layout_height - layout_header - 1)
			{
				return true;
			}
		}
		return false;
	}
}
