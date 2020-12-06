using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public Blocks[] blocks;       //make array element for the 7 blocks

    /**
        Method to get random block
    **/
    Blocks GetRandomeBlock()
    {
        int i = Random.Range(0, blocks.Length); //get random int range to num of blocks
        
        /*
            Check null case
        */
        if (blocks[i])
        {
            return blocks[i];
        }
        else
        {
            Debug.LogWarning("Invalid!");
            return null;
        }
    }

    /*
        Shape Spawn method
    */
    public Blocks SpawnBlock()
    {
        Blocks block = null;
        block = Instantiate(GetRandomeBlock(), transform.position, Quaternion.identity) as Blocks;

        /*
        null case
        */
        if (block)
        {
            return block;
        }
        else
        {
            Debug.LogWarning("Invalid!");
            return null;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //call helper script to make sure block be on whole number location
        Vector2 originalVector = new Vector2(4.3f, 1.3f);
        Vector2 newVector = Vectorf.Round(originalVector);

        Debug.Log(newVector.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
