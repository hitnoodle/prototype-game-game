
using System.Collections.Generic;
using UnityEngine;

public class StateGame : ExaState {
	public StateGame(): base(GAME) {
		//Initialize
		m_Coin 		= 9;
		m_Health	= 3;
	
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
		m_Exa 		= new Exa();
		m_Coins		= new FContainer();
		AddChild(m_Coins);
		AddChild(m_Exa);

		//Create coins
		processCoins(0);

		//Timer for coin
		m_SpawnCoinTime = 3.0f;
		m_CurrentCoinTime = m_SpawnCoinTime;
		
		//Create interface
		m_Coin--;
		m_Health++;
		m_CoinCounter 	= new FLabel("font", "");
		m_HealthCounter = new FLabel("font", "");
		AddChild(m_HealthCounter);
		AddChild(m_CoinCounter);
		decrementHealth();
		incrementCoin();
	}

	public override void onUpdate(FTouch[] touches) {
		//Update
		m_Exa.update();
		processCoins(m_Exa.getOffset());
		
		//Check input
		checkTouchedCoins(touches);
	}
	
	public void incrementCoin() {
		//Add
		m_Coin++;
		
		//Refresh
		m_CoinCounter.text 	= m_Coin + " Coins";
		m_CoinCounter.x 	= Futile.screen.width - 12 - (m_CoinCounter.textRect.width * 0.5f);
		m_CoinCounter.y 	= Futile.screen.height - 12 - (m_CoinCounter.textRect.height * 0.5f);
	}
	
	public void decrementHealth() {
		//Decrease
		m_Health--;
		
		//Refresh
		m_HealthCounter.text 	= "Error: " + (3 - m_Health);
		m_HealthCounter.x 		= 12 + (m_HealthCounter.textRect.width * 0.5f);
		m_HealthCounter.y 		= 4 + (m_HealthCounter.textRect.height * 0.5f);
	}

	protected void processCoins(float offset) {
		//Initialize
		List<FNode> Deads = new List<FNode>();
		
		//For each child in the container
		for (int i = 0; i < m_Coins.GetChildCount(); i++) {
			//Get platform
			Coin ACoin = m_Coins.GetChildAt(i) as Coin;
			if (ACoin != null) {
				//Move
				ACoin.x 		-= offset;
				float CoinRight	 = ACoin.x + (ACoin.getWidth() / 2);
				if (CoinRight < 0) 	Deads.Add(ACoin);

				//Update duration
				ACoin.UpdateDuration(Time.deltaTime);
			}
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_Coins.RemoveChild(Deads[i]);
		
		//Spawn
		m_CurrentCoinTime -= Time.deltaTime;
		if (m_CurrentCoinTime <= 0f) {
			//Create
			float X 				= Futile.screen.width + (Random.Range(0, 6) * 40);
			float Y 				= Random.Range(0, ((int)(Futile.screen.height / 40) + 1)) * 40;
			Coin NewCoin			= new Coin(X, Y, 2f);
			
			//Add
			m_Coins.AddChild(NewCoin);
			
			//Reset
			m_CurrentCoinTime = m_SpawnCoinTime;
		}
	}

	protected void checkTouchedCoins(FTouch[] touches) {
		//Initialize
		List<FNode> Deads = new List<FNode>();
		
		//For each child in the container
		for (int i = 0; i < m_Coins.GetChildCount(); i++) {
			//Get coins
			Coin ACoin = m_Coins.GetChildAt(i) as Coin;
			if (ACoin != null) {
				foreach(FTouch touch in touches) {
					if (ACoin.IsTouched(touch.position)) {
						Deads.Add(ACoin);
						break;
					}
				}
			}	
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_Coins.RemoveChild(Deads[i]);
	}

	//Data
	protected int 	m_Coin;
	protected int 	m_Health;
	protected float m_SpawnCoinTime;
	protected float m_CurrentCoinTime;
	
	//Components
	protected Exa			m_Exa;
	protected FContainer	m_Coins;
	
	//Interface
	protected FLabel m_CoinCounter;
	protected FLabel m_HealthCounter;
}
