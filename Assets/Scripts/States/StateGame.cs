
using System.Collections.Generic;
using UnityEngine;

public class StateGame : ExaState {
	public StateGame(): base(GAME) {
		//Initialize
		m_Score 		= 0;
		m_Error			= 0;
		m_Health		= HEALTH_MAX;
		m_EnemyTimer	= 2.0f;
	
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
		m_Exa 			= new Exa();
		m_Coins			= new FContainer();
		m_Enemies		= new FContainer();
		m_PlayerBullets	= new FContainer();
		AddChild(m_Enemies);
		AddChild(m_Exa);
		AddChild(m_PlayerBullets);

		//Create coins
		processCoins(0);

		//Timer for coin
		m_SpawnCoinTime 	= 2.0f;
		m_CurrentCoinTime 	= m_SpawnCoinTime;
		
		//Create interface
		m_ScoreCounter 		= new FLabel("font", "");
		m_ErrorCounter 		= new FLabel("font", "");
		m_HealthCounter 	= new FLabel("font", "");
		m_CounterOverlay 	= new FSprite("rect") {
			x = 0, 
			y = 0, 
			width = m_ScoreCounter.textRect.width, 
			height = m_ScoreCounter.textRect.height, 
			color = new Color(255.0f, 255.0f, 255.0f, 0.35f) 
		};
		m_CounterOverlay.isVisible = false;
		
		//Prepare
		m_Error--;
		AddChild(m_ScoreCounter);
		AddChild(m_ErrorCounter);
		AddChild(m_HealthCounter);
		AddChild(m_CounterOverlay);
		incrementError();
		increaseScore(0);
		changeHealth(0);
	}

	public override void onUpdate(FTouch[] touches) {
		//Update
		m_Exa.update();
		processEnemies();
		
		//For each enemy
		Drawable Enemy = null;
		for (int i = 0; i < m_Enemies.GetChildCount() && Enemy == null; i++) {
			//Get enemy
			Enemy = m_Enemies.GetChildAt(i) as Drawable;
			if (Enemy != null && Enemy.x < m_Exa.x) Enemy = null;
		}
		
		//IF closest enemy exist
		if (Enemy != null) {
			//Move player
			if (Enemy.y < m_Exa.y - 8) 		m_Exa.y -= 8;
			else if (Enemy.y > m_Exa.y + 8) m_Exa.y += 8;
			else 							m_Exa.y = Enemy.y;
		}

		processPlayerBullets(Enemy);
		
		//Check input
		processCoinCounter(touches);
	}
	
	public void increaseScore(int amount) {
		//Add
		m_Score += amount;
		
		//Refresh
		m_ScoreCounter.text = "Score: " + m_Score;
		m_ScoreCounter.x 	= Futile.screen.width - 12 - (m_ScoreCounter.textRect.width * 0.5f);
		m_ScoreCounter.y 	= Futile.screen.height - 12 - (m_ScoreCounter.textRect.height * 0.5f);
		
		//Refresh overlay
		m_CounterOverlay.x 		= m_ScoreCounter.x;
		m_CounterOverlay.y 		= m_ScoreCounter.y;
		m_CounterOverlay.width	= m_ScoreCounter.textRect.width;
		m_CounterOverlay.height	= m_ScoreCounter.textRect.height;
	}
	
	public void changeHealth(float change) {
		//Change
		m_Health += change;
		
		//Refresh
		m_HealthCounter.text 	= "Health: " + (int)m_Health;
		m_HealthCounter.x 		= 12 + (m_HealthCounter.textRect.width * 0.5f);
		m_HealthCounter.y 		= Futile.screen.height - 12 - (m_HealthCounter.textRect.height * 0.5f);
	}
	
	public void incrementError() {
		//Increase
		m_Error++;
		
		//Refresh
		m_ErrorCounter.text 	= "Error: " + m_Error;
		m_ErrorCounter.x 		= 12 + (m_ErrorCounter.textRect.width * 0.5f);
		m_ErrorCounter.y 		= 4 + (m_ErrorCounter.textRect.height * 0.5f);
	}

	/*protected void processCoins(float offset) {
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
					incrementError();
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
						if (!ACoin.ShouldBeTouched()) incrementError();

						break;
					}
				}
			}	
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_Coins.RemoveChild(Deads[i]);
	}*/
	
	protected void processCoinCounter(FTouch[] touches) {
		//If there's counter
		if (m_CoinCounterTime > 0) {
			//Manage time
			m_CoinCounterTime -= Time.deltaTime;
			if (m_CoinCounterTime <= 0 && !m_CounterOverlay.isVisible) incrementError();
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
				float HalfWidth		= m_ScoreCounter.textRect.width * 0.5f;
				float HalfHeight 	= m_ScoreCounter.textRect.height * 0.5f;
				if (TouchX >= m_ScoreCounter.x - HalfWidth && TouchX <= m_ScoreCounter.x + HalfWidth && TouchY >= m_ScoreCounter.y - HalfHeight && TouchY <= m_ScoreCounter.y + HalfHeight) Touched = true;
			}	
		}
		
		//If touched
		if (Touched) {
			//Decrease health if not time
			if (m_CoinCounterTime <= 0) incrementError();
			
			//Pressed
			increaseScore(500);
			m_CoinCounterTime = 0;
		}
	}
	
	protected void processEnemies() {
		//Initialize
		List<FNode> Deads = new List<FNode>();
		
		//For each child in the container
		for (int i = 0; i < m_Enemies.GetChildCount(); i++) {
			//Get enemy
			Drawable Enemy = m_Enemies.GetChildAt(i) as Drawable;
			if (Enemy != null) {
				//Move
				Enemy.x -= 7.0f;
				if (Enemy.x + (Enemy.getWidth() / 2) < 0) Deads.Add(Enemy);
			}
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_Enemies.RemoveChild(Deads[i]);
		
		//Spawn
		m_EnemyTimer -= Time.deltaTime;
		if (m_EnemyTimer <= 0f) {
			//Create
			float X 	= Futile.screen.width + 61.0f; //Hardcode width / 2
			float Y		= Futile.screen.height / 12.0f * (float)(Random.Range(3, 10));
			Enemy enemy = new Enemy(X,Y);
			
			//Add
			m_Enemies.AddChild(enemy);
			
			//Reset
			m_EnemyTimer = Random.Range(1, 9) / 2.0f;
			if (m_EnemyTimer > 1) m_EnemyTimer -= 1;
		}
	}
	
	//Constants
	protected const float HEALTH_MAX = 100;

	protected void processPlayerBullets(Drawable enemy) {
		//Initialize
		List<FNode> Deads = new List<FNode>();
		
		//For each child in the container
		for (int i = 0; i < m_PlayerBullets.GetChildCount(); i++) {
			//Get enemy
			Drawable Bullet = m_PlayerBullets.GetChildAt(i) as Drawable;
			if (Bullet != null) {
				//Move
				Bullet.x += 10.0f;
				if (Bullet.x + (Bullet.getWidth() / 2) > Futile.screen.width) Deads.Add(Bullet);
			}
		}
		
		//Remove dead coins
		for (int i = 0; i < Deads.Count; i++) m_PlayerBullets.RemoveChild(Deads[i]);
		
		//Spawn
		m_PlayerBulletTimer -= Time.deltaTime;
		if (m_PlayerBulletTimer <= 0f && enemy != null && enemy.y == m_Exa.y) {
			//Create
			Vector2 playerPos 	= m_Exa.GetPosition();
			float X 			= playerPos.x + 10.0f;
			float Y				= playerPos.y;
			PlayerBullet bullet = new PlayerBullet(X,Y);
			
			//Add
			m_PlayerBullets.AddChild(bullet);
			
			//Reset
			m_PlayerBulletTimer = 1.0f;
		}
	}

	//Data
	protected int 	m_Score;
	protected int	m_Error;
	protected float m_Health;
	protected float m_CoinCounterTime;
	protected float m_EnemyTimer;
	protected float m_PlayerBulletTimer;
	
	//Components
	protected Exa			m_Exa;
	protected FContainer	m_Enemies;
	protected FContainer	m_PlayerBullets;
	
	//Interface
	protected FLabel 	m_ScoreCounter;
	protected FLabel 	m_ErrorCounter;
	protected FLabel 	m_HealthCounter;
	protected FSprite	m_CounterOverlay;
}
