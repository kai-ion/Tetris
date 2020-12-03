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

    //run before Start
    void Awake() {
        layout_grid = new Transform[layout_height, layout_width];
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
}
