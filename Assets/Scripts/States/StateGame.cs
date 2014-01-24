
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
		m_SpawnCoinTime 	= 2.0f;
		m_CurrentCoinTime 	= m_SpawnCoinTime;
		
		//Create interface
		m_HealthCounter 	= new FLabel("font", "");
		m_CoinCounter 		= new FLabel("font", "");
		m_CounterOverlay 	= new FSprite("rect") {
			x = 0, 
			y = 0, 
			width = m_CoinCounter.textRect.width, 
			height = m_CoinCounter.textRect.height, 
			color = new Color(255.0f, 255.0f, 255.0f, 0.35f) 
		};
		m_CounterOverlay.isVisible = false;
		
		//Prepare
		m_Coin--;
		m_Health++;
		AddChild(m_CoinCounter);
		AddChild(m_HealthCounter);
		AddChild(m_CounterOverlay);
		decrementHealth();
		incrementCoin();
	}

	public override void onUpdate(FTouch[] touches) {
		//Update
		m_Exa.update();
		processCoins(m_Exa.getOffset());
		
		//Check input
		checkTouchedCoins(touches);
		processCoinCounter(touches);
	}
	
	public void incrementCoin() {
		//Add
		m_Coin++;
		
		//Refresh
		m_CoinCounter.text 	= m_Coin + " Coins";
		m_CoinCounter.x 	= Futile.screen.width - 12 - (m_CoinCounter.textRect.width * 0.5f);
		m_CoinCounter.y 	= Futile.screen.height - 12 - (m_CoinCounter.textRect.height * 0.5f);
		
		//Refresh overlay
		m_CounterOverlay.x 		= m_CoinCounter.x;
		m_CounterOverlay.y 		= m_CoinCounter.y;
		m_CounterOverlay.width	= m_CoinCounter.textRect.width;
		m_CounterOverlay.height	= m_CoinCounter.textRect.height;
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
				ACoin.x -= offset;

				//Coin dead, assumed player not clicking
				float CoinRight	 = ACoin.x + (ACoin.getWidth() / 2);
				if (CoinRight < 0) {
					Deads.Add(ACoin);
					decrementHealth();
				}

				//Update duration
				bool ShouldTouch = ACoin.ShouldBeTouched();
				ACoin.UpdateDuration(Time.deltaTime);
				if (!ShouldTouch && ACoin.ShouldBeTouched()) m_CoinCounterTime = 2.0f;
			}
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_Coins.RemoveChild(Deads[i]);
		
		//Spawn
		m_CurrentCoinTime -= Time.deltaTime;
		if (m_CurrentCoinTime <= 0f) {
			//Create
			float X 		= Futile.screen.width + (Random.Range(0, 6) * 40);
			float Y			= Futile.screen.height / 16.0f * (float)(Random.Range(6, 14));
			Coin NewCoin	= new Coin(X, Y, 2f);
			
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
						if (!ACoin.ShouldBeTouched()) decrementHealth();

						break;
					}
				}
			}	
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_Coins.RemoveChild(Deads[i]);
	}
	
	protected void processCoinCounter(FTouch[] touches) {
		//If there's counter
		if (m_CoinCounterTime > 0) {
			//Manage time
			m_CoinCounterTime -= Time.deltaTime;
			if (m_CoinCounterTime <= 0 && !m_CounterOverlay.isVisible) decrementHealth();
		}
		m_CounterOverlay.isVisible = m_CoinCounterTime > 0;
	
		//For each touch
		bool Touched = false;
		for (int i = 0; i < touches.Length && !Touched; i++) {
			//If done
			if (touches[i].phase == TouchPhase.Ended) {
				//Check position
				float TouchX 		= touches[i].position.x;
				float TouchY 		= touches[i].position.y;
				float HalfWidth		= m_CoinCounter.textRect.width * 0.5f;
				float HalfHeight 	= m_CoinCounter.textRect.height * 0.5f;
				if (TouchX >= m_CoinCounter.x - HalfWidth && TouchX <= m_CoinCounter.x + HalfWidth && TouchY >= m_CoinCounter.y - HalfHeight && TouchY <= m_CoinCounter.y + HalfHeight) Touched = true;
			}	
		}
		
		//If touched
		if (Touched) {
			//Decrease health if not time
			if (m_CoinCounterTime <= 0) decrementHealth();
			
			//Pressed
			incrementCoin();
			m_CoinCounterTime = 0;
		}
	}

	//Data
	protected int 	m_Coin;
	protected int 	m_Health;
	protected float m_SpawnCoinTime;
	protected float m_CurrentCoinTime;
	protected float m_CoinCounterTime;
	
	//Components
	protected Exa			m_Exa;
	protected FContainer	m_Coins;
	
	//Interface
	protected FLabel 	m_CoinCounter;
	protected FLabel 	m_HealthCounter;
	protected FSprite	m_CounterOverlay;
}
