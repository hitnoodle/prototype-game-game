
using System.Collections.Generic;
using UnityEngine;

public class StateGame : ExaState {
	public StateGame(): base(GAME) {
		//Initialize
		m_Coin = 9;
	
		//Create background
		FSprite Background = new FSprite("rect") { 
			x = Futile.screen.halfWidth, 
			y = Futile.screen.halfHeight, 
			width = Futile.screen.width, 
			height = Futile.screen.height, 
			color = new Color(86.0f / 256.0f, 181.0f / 256.0f, 1.0f, 1.0f) };
		AddChild(Background);
		
		//Create platform
		FSprite Base = new FSprite("rect") { 
			x = Futile.screen.halfWidth, 
			y = Futile.screen.halfHeight * 0.25f, 
			width = Futile.screen.width, 
			height = Futile.screen.halfHeight * 0.5f, 
			color = new Color(0.0f, 1.0f, 0.0f, 1.0f) };
		AddChild(Base);
		
		//Create components
		m_Exa = new Exa();
		AddChild(m_Exa);
		
		//Create interface
		m_CoinCounter = new FLabel("font", "");
		AddChild(m_CoinCounter);
		incrementCoin();
	}

	public override void onUpdate(FTouch[] touches) {
		//Update
		m_Exa.update();
	}
	
	public void incrementCoin() {
		//Add
		m_Coin++;
		
		//Refresh
		m_CoinCounter.text 	= m_Coin + " Coins";
		m_CoinCounter.x 	= Futile.screen.width - 12 - (m_CoinCounter.textRect.width * 0.5f);
		m_CoinCounter.y 	= Futile.screen.height - 12 - (m_CoinCounter.textRect.height * 0.5f);
	}
	
	//Data
	protected int m_Coin;
	
	//Components
	protected Exa m_Exa;
	
	//Interface
	protected FLabel m_CoinCounter;
}
