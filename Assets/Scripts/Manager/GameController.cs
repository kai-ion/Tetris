using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Layout layout;      //reference to layout script

    Spawner spawner;    //refernce spawner script

    ScoreManager score_manager;   //score manager script reference

    private SoundHandler sh;

    public GameObject gameover_panel;
    public bool game_over = false;

    PauseGame pause_game;

    Blocks current_block;   //currently active block
    float drop_interval = .5f;
    float dropInterval_new;
    float previous_time;
    
    float drop_timer;  

    float timeTo_NextKey_LeftRight;

	[Range(0.02f,1f)]
	public float keyRepeatRate_LeftRight = 0.2f;

	float timeToNextKey_Down;

	[Range(0.005f,0.5f)]
	public float keyRepeatRate_Down = 0.01f;

	float timeTo_NextKeyRotate;

    [Range(0.02f,1f)]
	public float keyRepeatRate_Rotate = 0.2f;

    bool hard_drop;

    // Start is called before the first frame update
    void Start()
    {
        //add references to layout and spawner component
        layout = GameObject.FindWithTag("Layout").GetComponent<Layout>();
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        score_manager = GameObject.FindObjectOfType<ScoreManager>();
        pause_game = GameObject.FindObjectOfType<PauseGame>();
        /**
        FindObjectOfType<s>, same function as above but slower
        **/
        //layout = GameObject.FindObjectOfType<Layout>();   //another find script method
        //spawner = GameObject.FindObjectOfType<Spawner>();   //another find script method

        timeToNextKey_Down = Time.time + keyRepeatRate_Down;
		timeTo_NextKey_LeftRight = Time.time + keyRepeatRate_LeftRight;
		timeTo_NextKeyRotate = Time.time + keyRepeatRate_Rotate;
        sh = GetComponent<SoundHandler>();

        hard_drop = false;

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
    	if (Input.GetKey(KeyCode.UpArrow) && (Time.time > timeTo_NextKeyRotate)) {	//change object direction
    		current_block.RotateRight();
            timeTo_NextKeyRotate = Time.time + keyRepeatRate_Rotate;
            sh.Playtransform();

            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.RotateLeft();
            }
    	}

        if (Input.GetKey(KeyCode.LeftControl) && (Time.time > timeTo_NextKeyRotate)) {	//change object direction
    		current_block.RotateLeft();
            timeTo_NextKeyRotate = Time.time + keyRepeatRate_Rotate;

            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.RotateRight();
            }
    	}
        
        if (Time.time - previous_time > (Input.GetKey(KeyCode.DownArrow) ? drop_interval / 10 : drop_interval)){	//change object direction
    		current_block.MoveDown();
            previous_time = Time.time;

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

    	if (Input.GetKeyDown(KeyCode.Space) ) {
            drop_timer = Time.time + dropInterval_new;
			timeToNextKey_Down = Time.time + keyRepeatRate_Down;
            
            while (layout.isValidPosition(current_block) && !hard_drop)
            {
                current_block.MoveDown();
            }
    		

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
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            hard_drop = false;
        }
        
    	if ((Input.GetKey (KeyCode.LeftArrow) && (Time.time > timeTo_NextKey_LeftRight)) || Input.GetKeyDown(KeyCode.LeftArrow)) {
    		current_block.MoveLeft();
            timeTo_NextKey_LeftRight = Time.time + keyRepeatRate_LeftRight;
            sh.PlayMove();
            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.MoveRight();
            }
    	}
    	if ((Input.GetKey (KeyCode.RightArrow) && (Time.time > timeTo_NextKey_LeftRight)) || Input.GetKeyDown(KeyCode.RightArrow)) {
    		current_block.MoveRight();
            timeTo_NextKey_LeftRight = Time.time + keyRepeatRate_LeftRight;
            sh.PlayMove();
            //make sure block dont over lap
            if (!layout.isValidPosition(current_block))
            {
                current_block.MoveLeft();
            }
    	}

        //press p to pause game
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            pause_game.PauseMenu();
        }

         /*
        if it past a certain time, start making the block fall
        allows player time to react
        */
   
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
                */
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
            
        }
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
        hard_drop = true;

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
				dropInterval_new = Mathf.Clamp(drop_interval - ((float)score_manager.level * 0.005f), 0.005f, 1f);
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
        sh.PlayDie();
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
        //set time scale back to 1 and load gamescene
        Time.timeScale = 1f;
		Debug.Log("Restart Game");
        SceneManager.LoadScene("GameScene");
	}

    // Return to main
    public void MainMenu()
	{
        //set time scale back to 1 and load gamescene
        Time.timeScale = 1f;
		Debug.Log("Return to MainMenu");
        SceneManager.LoadScene("StartScene");
	}

    // Return to main
    public void QuitGame()
	{
		Debug.Log("Exit Game");
        Application.Quit();
	}
}
