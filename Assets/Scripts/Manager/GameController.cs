using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Layout layout;      //reference to layout script

    Spawner spawner;    //refernce spawner script

    ScoreManager score_manager;   //score manager script reference

    public GameObject gameover_panel;
    bool game_over = false;

    Blocks current_block;   //currently active block
    float drop_interval = .25f;
    float dropInterval_new;

    float drop_timer;  

    float timeTo_NextKey_LeftRight;

	[Range(0.02f,1f)]
	public float key_RepeatRate_LeftRight = 0.25f;

	float timeToNextKey_Down;

	[Range(0.01f,0.5f)]
	public float keyRepeatRate_Down = 0.02f;

	float timeTo_NextKeyRotate;

    [Range(0.02f,1f)]
	public float keyRepeatRate_Rotate = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        //add references to layout and spawner component
        layout = GameObject.FindWithTag("Layout").GetComponent<Layout>();
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        score_manager = GameObject.FindObjectOfType<ScoreManager>();
        /**
        FindObjectOfType<s>, same function as above but slower
        **/
        //layout = GameObject.FindObjectOfType<Layout>();   //another find script method
        //spawner = GameObject.FindObjectOfType<Spawner>();   //another find script method

        timeToNextKey_Down = Time.time + keyRepeatRate_Down;
		timeTo_NextKey_LeftRight = Time.time + key_RepeatRate_LeftRight;
		timeTo_NextKeyRotate = Time.time + keyRepeatRate_Rotate;

        

        /**
        check if spawner is assigned
        **/
        if (spawner)
        {
            /*
            check if there is a block spawned
            if there isnt spawn a random block
            */
            if(current_block == null)
            {
                current_block = spawner.SpawnBlock();
            }

            //make sure block spawns on a whole number square
            spawner.transform.position = Vectorf.Round(spawner.transform.position);
        }
        else
        {
            Debug.LogWarning("No Spawner Defined!");
        }
        

        if(!layout)
        {
            Debug.LogWarning("No Layout Defined!");
        }

        if(!score_manager)
        {
            Debug.LogWarning("No score manager Defined!");
        }

        if (gameover_panel)
		{
			gameover_panel.SetActive(false);
		}

        dropInterval_new =  Mathf.Clamp(drop_interval - ((float)score_manager.level * 0.1f), 0.05f, 1f);
    }
    
    private void Controll() 
    {
    	if (Input.GetKeyDown(KeyCode.UpArrow) && (Time.time > timeTo_NextKeyRotate)) {	//change object direction
    		current_block.RotateRight();
            timeTo_NextKeyRotate = Time.time + keyRepeatRate_Rotate;

            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.RotateLeft();
            }
    	}
        
        if (Input.GetKeyDown(KeyCode.DownArrow) && (Time.time > timeToNextKey_Down)) {	//change object direction
    		current_block.MoveDown();
            timeToNextKey_Down = Time.time + keyRepeatRate_Down;

            //make sure block dont over lap
            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                if (layout.IsOverLimit(current_block))
				{
                    GameOver();
				}
				else
				{
					//call method to handle block landing
                    LandBlock();
				}  
            }
    	}

    	if (Input.GetKey(KeyCode.Space) && (Time.time > timeToNextKey_Down) ||  (Time.time > drop_timer)) {
            drop_timer = Time.time + dropInterval_new;
			timeToNextKey_Down = Time.time + keyRepeatRate_Down;

    		current_block.MoveDown();

            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                if (layout.IsOverLimit(current_block))
				{
                    GameOver();
				}
				else
				{
					//call method to handle block landing
                    LandBlock();
				}  
            }
    	}
        
    	if ((Input.GetKey (KeyCode.LeftArrow) && (Time.time > timeTo_NextKey_LeftRight)) || Input.GetKeyDown(KeyCode.LeftArrow)) {
    		current_block.MoveLeft();
            timeTo_NextKey_LeftRight = Time.time + key_RepeatRate_LeftRight;

            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.MoveRight();
            }
    	}
    	if ((Input.GetKey (KeyCode.RightArrow) && (Time.time > timeTo_NextKey_LeftRight)) || Input.GetKeyDown(KeyCode.RightArrow)) {
    		current_block.MoveRight();
            timeTo_NextKey_LeftRight = Time.time + key_RepeatRate_LeftRight;

            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.MoveLeft();
            }
    	}

         /*
        if it past a certain time, start making the block fall
        allows player time to react
        */
        /*
        if (Time.time > drop_timer)
        {
            drop_timer = Time.time + drop_interval;
            //if there is a block, set it to fall down on default
            if (current_block)
            {
                current_block.MoveDown();

                /*
                    once block falls to bottom, 
                    we stop its MoveDown method and push it back up one
                    we have to push back up because our script only detects
                    that its out of boundary when it actually goes out of boundary
                    afterwards set current_block to a new block, to spawn new block
                *//*
                if (!layout.isValidPosition (current_block))
                {
                    //block lands
                    current_block.MoveUp();
                    layout.StoreBlock(current_block);   //store the block
                    if (spawner)
                    {
                        current_block = spawner.SpawnBlock();
                    }
                }
                
            }
            
        }*/
    }

    /*
        block lands method
    */
	void LandBlock ()
	{
		// move the block up, store it in datastructure
		current_block.MoveUp ();
		layout.StoreBlock (current_block);

		// spawn a new block
		current_block = spawner.SpawnBlock();

		// set all of the time_ToNextKey variables to current time, 
        // so no input delay for the next spawned block
		timeTo_NextKey_LeftRight = Time.time;
		timeToNextKey_Down = Time.time;
		timeTo_NextKeyRotate = Time.time;

		// remove completed rows from the board if we have any 
		layout.ClearRows();

        if (layout.num_rows > 0)
		{
			score_manager.ScoreCounter(layout.num_rows);

            //increase drop speed
			if (score_manager.didLevelUp)
			{
				dropInterval_new = Mathf.Clamp(drop_interval - ((float)score_manager.level * 0.05f), 0.05f, 1f);
			}
			
		}
    }

    // Update is called once per frame
    void Update()
    {
        //if spawner or layout not set dont update the game
        //also dont call player control when game is over
        //also case for score_manager
        if (!layout || !spawner || !current_block || game_over || !score_manager)
        {
            return;
        }

        /*
        player control methods
        */
        Controll();

       
    }

    // triggered when we are over the board's limit
	void GameOver ()
	{
		// move the shape one row up
		current_block.MoveUp ();

		// turn on the Game Over Panel
		if (gameover_panel) {
			gameover_panel.SetActive (true);
		}

		// set the game over condition to true
		game_over = true;
	}

    // reload the level
	public void Restart()
	{
        Time.timeScale = 1f;
		Debug.Log("Restart");
		//Application.LoadLevel(Application.loadedLevel);
	}
}
