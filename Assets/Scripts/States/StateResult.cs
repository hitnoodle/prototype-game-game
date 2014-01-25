using UnityEngine;
using System.Collections;

public class StateResult : ExaState {
	public StateResult(StateGame game) : base(RESULT) {
		//Save game
		m_Game = game;

		//Create labels
		FLabel Title		= new FLabel("font", "Crashed!") { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CANVAS_BOTTOM + (Constants.UNITY_CANVAS_HEIGHT * 0.75f) };
		FLabel Result		= new FLabel("font", "Metacritic: 6.5/10") { x = Constants.UNITY_CENTER_X, y =  Constants.UNITY_CENTER_Y };
		FSprite Background	= new FSprite("rect") { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CENTER_Y, width = Constants.UNITY_CANVAS_WIDTH, height = Constants.UNITY_CANVAS_HEIGHT, color = new Color(0, 0, 0, 0.5f) };
		AddChild(Background);
		AddChild(Result);
		AddChild(Title);

		//Create buttons
		m_ButtonRetry	= new FButton("button-pause", "button-pause", "button-pause-hover", "cursor") { x = Constants.UNITY_CENTER_X, y = Constants.UNITY_CANVAS_BOTTOM + (Constants.UNITY_CANVAS_HEIGHT * 0.25f)};
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

	public static int s_Highscore = 0;
	public static int s_Highdistance = 0;

	//Data
	protected FButton	m_ButtonRetry;
	protected StateGame m_Game;
}
