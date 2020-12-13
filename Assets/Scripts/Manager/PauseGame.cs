using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool is_paused = false;

    public GameObject pause_panel;

    GameController game;


    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindObjectOfType<GameController>();
        //set pause to be off when game starts
        if (pause_panel)
        {
            pause_panel.SetActive(false);
        }
    }

    public void PauseMenu()
    {
        if (game.game_over == true)
        {
            return;
        }

        is_paused = !is_paused;

        if (pause_panel)
        {
            pause_panel.SetActive(is_paused);
        /*
            if (sound_manager)
            {
                sound_manager.music_source.volume = (is_paused) ? sound_manager.music_volume * 0.25f : sound_manager.music_volume;
            }
            */
            //set time to zero to pause game
            Time.timeScale = (is_paused) ? 0 : 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
