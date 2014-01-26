using UnityEngine;
using System.Collections;

public class StateResult : ExaState {
	public StateResult(StateGame game, float health) : base(RESULT) {
		//Save game
		m_Game = game;
		
		//Get quote
		Debug.Log("health " + health + " / 10 " + (int)((100 - health) / 10));
		int Index       = (int)((int)(100 - health) / 5) + Random.Range(0,2);
		string Quote1	= '"' + "" + (int)((100 - health) / 10) + "/10 ... " + Quotes1[Index];
		string Quote2 	= Quotes2[Index] + '"' + " - " + TwitterHandles[Random.Range(0, 10)];
		if (Quotes2[Index].Length <= 0) Quote1 += Quote2;

		//Create labels
		FLabel Title		= new FLabel("font", health > 0 ? "THE GAME CRASHED!" : "THE GAME FINISHED!") { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CANVAS_BOTTOM + (Constants.UNITY_CANVAS_HEIGHT * 0.78f) };
		FLabel Status		= new FLabel("font", "Player is at " + (int)health + "% health") { x = Constants.UNITY_CENTER_X, scale = 0.9f };
		FLabel Tweet1		= new FLabel("font", Quote1) { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CENTER_Y, scale = 0.8f };
		FLabel Tweet2		= new FLabel("font", Quote2) { x = Constants.UNITY_CENTER_X, scale = 0.8f };
		FSprite Background	= new FSprite("rect") { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CENTER_Y, width = Constants.UNITY_CANVAS_WIDTH, height = Constants.UNITY_CANVAS_HEIGHT, color = new Color(0, 0, 0, 0.5f) };
		Status.y = Constants.UNITY_CANVAS_BOTTOM + (Constants.UNITY_CANVAS_HEIGHT * 0.78f) - Title.textRect.height + 4;
		Tweet2.y = Constants.UNITY_CENTER_Y - (Tweet1.textRect.height * 0.8f);
		
		//Add
		AddChild(Background);
		if (health > 0) AddChild(Status);
		AddChild(Tweet1);
		if (Quotes2[Index].Length > 0) AddChild(Tweet2);
		AddChild(Title);

		//Create buttons
		m_ButtonRetry	= new FButton("button", "button-hover", "button", "cursor") { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CANVAS_BOTTOM + (Constants.UNITY_CANVAS_HEIGHT * 0.15f)};
		m_ButtonRetry.AddLabel("font", "Restart Game", Color.white);
		m_ButtonRetry.SignalRelease += onPress;
		AddChild(m_ButtonRetry);
	}

	protected void onPress(FButton button) {
		//Check button
		if (button == m_ButtonRetry) {
			//Setup game
			if (m_Game != null) m_Game.setup();
			m_Active = false;
			
			//SFX
			FSoundManager.PlaySound("success");
		}
	}

	public override void onUpdate(FTouch[] touches) {
		//Do nothing
	}
 
	//Readonly quotes for array
	public readonly string[] Quotes1 = {
        "if you really HATE someone,",
        "you have better things to waste",
        "it is better to just tap the screen",
        "you would have more fun",
        "some videogames can be",
        "it crashes every time",
        "kids may never want to play",
        "outright terrifying",
        "I found myself craving lots of",
        "uniquely bad",
        "not worth the time",
        "still, it's free",
        "it treated me",
        "deeply predictable",
        "this could be",
        "delightful but",
        "chasing perfection in",
        "if you think you've seen it all,",
        "one of the smartest,",
        "breathtaking in scope and bitingly funny",
        "a staggering work",
        "prepare to lose yourself",
	};
	
	public readonly string[] Quotes2 = {
        "give them this game as a gift",
        "your money, like soap",
        "while the system is off",
        "playing backgammon with your mother",
        "outclassed by potatoes",
        "I try to quit",
        "games again after playing this",
        "",
        "fried chicken after a while",
        "",
        "or the money",
        "",
        "like a bad pet",
        "",
        "a real sleeper hit",
        "disinterested",
        "the right direction",
        "you haven't",
        "most engrossing games of 2014",
        "and bitingly funny",
        "of genius",
        "in this charming world",
	};
 
	public readonly string[] TwitterHandles = {
        "Kotaku",
        "@rukanishino",
        "@tornado91",
        "Game Informer",
        "@legacy99",
        "Eurogamer",
        "Touch Arcade",
        "PC Gamer",
        "Destructoid",
        "Polygon",
        "IGN",
	};

	public static int s_Highscore = 0;
	public static int s_Highdistance = 0;

	//Data
	protected FButton	m_ButtonRetry;
	protected StateGame m_Game;
}
