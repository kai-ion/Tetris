using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int score = 0;
    int lines;
    public int level = 1;

    public int linesPer_level = 5;

	public Text lines_text;
	public Text level_text;
	public Text score_text;

    public int min_lines = 1;
    public int max_lines = 4;

	public bool didLevelUp = false;

    //calculate score per line
    public void ScoreCounter(int n)
    {
        // flag to GameController that we leveled up
		didLevelUp = false;

		// clamp value between min and max lines
		n = Mathf.Clamp(n, min_lines, max_lines);

		// adds to score depending on lines clear
		switch (n)
		{
			case 1:
				score += 40 * level;
				break;
			case 2:
				score += 100 * level;
				break;
			case 3:
				score += 300 * level;
				break;
			case 4:
				score += 1200 * level;
				break;
		}

		// reduce our current number of lines needed for the next level
		lines -= n;

		// if we finished our lines, then level up
		if (lines <= 0)
		{
			LevelUp();
		}

		// update the UI everytime we score
		UpdateUIText();
    }

	// reset game level
	public void Reset()
	{
		level = 1;
		score = 0;
		lines = linesPer_level * level;
		UpdateUIText();
	}

    // returns a string with num of zeros added to the fron
	string AddZero(int n, int digits)
	{
		string str = n.ToString();

		while (str.Length < digits)
		{
			str = "0" + str;
		}
		return str;
	}
	// level up method
	public void LevelUp()
	{
		level++;
		lines = linesPer_level * level;
		didLevelUp = true;
	}

	//start game with clear level and score
	void Start () 
	{
		Reset();
	}

    // Update the score text
    void UpdateUIText()
    {
        if (lines_text)
		{
			lines_text.text = lines.ToString();
		}

		if (level_text)
		{
			level_text.text = level.ToString();
		}

		if (score_text)
		{
			score_text.text = AddZero(score, 5);
		}
    }
}
