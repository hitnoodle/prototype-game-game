using UnityEngine;
using System.Collections;

public class StateTitle : ExaState {
	public StateTitle(StateGame game) : base(TITLE) {
		//Initialize
		m_Game		= game;
		m_Time		= BLINK_DURATION * 2;
		m_StartTime = START_DURATION * 2;
		
		//Create background
		FSprite Background	= new FSprite("rect") { x = Futile.screen.halfWidth, y = Futile.screen.halfHeight, width = Futile.screen.width, height = Futile.screen.height, color = new Color(0, 0, 0, 0.5f) };
		AddChild(Background);
		
		//Create stuff
		m_Title			= new FSprite("logo") 						{ x = Futile.screen.halfWidth, y = Futile.screen.height * 0.6f };
		m_Instruction1	= new FLabel("font", "Tap screen to play") 	{ x = Futile.screen.halfWidth, y = Futile.screen.height * 0.2f };
		AddChild(m_Instruction1);
		AddChild(m_Title);

		//Credit
		/*FLabel Credit1	= new FLabel("visitor-small", "A game by Karunia Ramadhan, Namira Chaldea, and Raka Mahesa") { x = Futile.screen.halfWidth };
		Credit1.y = 4 + (Credit1.textRect.height * 0.5f);
		AddChild(Credit1);*/

		//Play music
		FSoundManager.PlayMusic("bgm", 1f);
	}

	public override void onUpdate(FTouch[] touches) {
		//If not starting
		if (m_StartTime > START_DURATION) {
			//Manage blinking
			m_Time -= Time.deltaTime;
			m_Instruction1.isVisible = m_Time > BLINK_DURATION;
			if (m_Time < 0) m_Time += BLINK_DURATION * 2;

			//if game exist
			if (m_Game != null) {
				//For each touch,
				for (int i = 0; i < touches.Length; i++) {
					//If released
					if (touches[i].phase == TouchPhase.Ended) {
						//SFX
						FSoundManager.PlaySound("success");

						//Start
						m_StartTime = START_DURATION;
						RemoveAllChildren();
						AddChild(m_Title);
					}
				}
			}
		} else {
			//Reduce start duration
			m_StartTime -= Time.deltaTime;
			if (m_StartTime < 0) {
				//Start game
				m_Active = false;
				m_Game.start();
			} else {
				//Move
				m_Title.y = Futile.screen.height * 0.6f;
				m_Title.y += ((Futile.screen.height * 0.4f) + (m_Title.textureRect.height / 2)) * (START_DURATION - m_StartTime) / START_DURATION;
			}
		}
	}

	//Constnats
	protected const float BLINK_DURATION	= 0.5f;
	public const float START_DURATION		= 0.5f;

	//Data
	protected float 	m_Time;
	protected float 	m_StartTime;
	protected FLabel 	m_Instruction1;
	protected FSprite	m_Title;
	protected StateGame m_Game;
}
