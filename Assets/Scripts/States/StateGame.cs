
using System.Collections.Generic;
using UnityEngine;

public class StateGame : ExaState {
	public StateGame(): base(GAME) {
		//Initialize
		m_Score 				= 0;
		m_Error					= 0;
		m_Health				= HEALTH_MAX;
		m_EnemyTimer			= 2.0f;
		m_PlayerBulletBorder 	= Futile.screen.width - 25;
		m_Score 				= 0;
		m_Error					= 0;
		m_EnemyTimer			= 2.0f;
		m_Health				= HEALTH_MAX;
		m_ScoreCounterTimers	= new List<float>();
		m_HealthCounterTimers	= new List<float>();
		m_HealthChanges			= new List<float>();
		m_Started				= false;
	
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
		m_Enemies		= new FContainer();
		m_PlayerBullets	= new FContainer();

		AddChild(m_Enemies);
		AddChild(m_Exa);
		AddChild(m_PlayerBullets);
		
		//Create interface
		m_ScoreCounter 		= new FLabel("font", "") { isVisible = false };
		m_ErrorCounter 		= new FLabel("font", "") { isVisible = false };
		m_HealthCounter 	= new FLabel("font", "") { isVisible = false };
		m_ScoreOverlay 		= new FSprite("rect") {
			x = 0, 
			y = 0, 
			width = m_ScoreCounter.textRect.width, 
			height = m_ScoreCounter.textRect.height, 
			color = new Color(1.0f, 1.0f, 1.0f, 0.35f),
			isVisible = false
		};
		m_HealthOverlay 	= new FSprite("rect") {
			x = 0, 
			y = 0, 
			width = m_HealthCounter.textRect.width, 
			height = m_HealthCounter.textRect.height, 
			color = new Color(1.0f, 1.0f, 1.0f, 0.35f),
			isVisible = false
		};
		
		//Prepare
		m_Error--;
		AddChild(m_ScoreCounter);
		AddChild(m_ScoreOverlay);
		AddChild(m_ErrorCounter);
		AddChild(m_HealthCounter);
		AddChild(m_HealthOverlay);
		incrementError();
		increaseScore(0);
		changeHealth();
	}
	
	public void start() {
		//Started
		m_Started = true;
		
		//Show interface
		m_ErrorCounter.isVisible = true;
		m_ScoreCounter.isVisible = true;
		m_HealthCounter.isVisible = true;
	}

	public override void onUpdate(FTouch[] touches) {
		//If not started
		if (!m_Started) StateManager.instance.goTo(TITLE, new object[] { this }, false);
		else {
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
	
			Enemy e = (Enemy)Enemy;
			processPlayerBullets(e);
	
			//Check input
			processCoinCounter(touches);
			checkTouchedObjects(touches);
		}
	}
	
	protected void addScoreChange(float duration) {
		//Add
		m_ScoreCounterTimers.Add(duration);
		m_ScoreOverlay.isVisible = true;
	}
	
	protected void addHealthChange(float change, float duration) {
		//Add
		m_HealthChanges.Add(change);
		m_HealthCounterTimers.Add(duration);

		//Show
		m_HealthOverlay.isVisible = true;
	}
	
	protected void increaseScore(int amount) {
		//Add
		m_Score += amount;
		if (m_ScoreCounterTimers.Count > 0) m_ScoreCounterTimers.RemoveAt(0);
		
		//Refresh
		m_ScoreCounter.text = "Score: " + m_Score;
		m_ScoreCounter.x 	= Futile.screen.width - 12 - (m_ScoreCounter.textRect.width * 0.5f);
		m_ScoreCounter.y 	= Futile.screen.height - 12 - (m_ScoreCounter.textRect.height * 0.5f);
		
		//Refresh overlay
		m_ScoreOverlay.x 		= m_ScoreCounter.x;
		m_ScoreOverlay.y 		= m_ScoreCounter.y;
		m_ScoreOverlay.width	= m_ScoreCounter.textRect.width;
		m_ScoreOverlay.height	= m_ScoreCounter.textRect.height;
	}
	
	protected void changeHealth() {
		//If there's change
		if (m_HealthCounterTimers.Count > 0) m_HealthCounterTimers.RemoveAt(0);
		if (m_HealthChanges.Count > 0) {
			//Change health
			m_Health += m_HealthChanges[0];
			m_HealthChanges.RemoveAt(0);
		}
		
		//Refresh
		m_HealthCounter.text 	= "Health: " + (int)m_Health;
		m_HealthCounter.x 		= 12 + (m_HealthCounter.textRect.width * 0.5f);
		m_HealthCounter.y 		= Futile.screen.height - 12 - (m_HealthCounter.textRect.height * 0.5f);
		
		//Refresh overlay
		m_HealthOverlay.x 		= m_HealthCounter.x;
		m_HealthOverlay.y 		= m_HealthCounter.y;
		m_HealthOverlay.width	= m_HealthCounter.textRect.width;
		m_HealthOverlay.height	= m_HealthCounter.textRect.height;
	}
	
	protected void incrementError() {
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
		//While not all timer
		int Index = 0;
		while (Index < m_ScoreCounterTimers.Count) {
			//Manage time
			m_ScoreCounterTimers[Index] -= Time.deltaTime;
			if (m_ScoreCounterTimers[Index] > 0) Index++;
			else {
				//Remove
				if (!m_ScoreOverlay.isVisible) incrementError();
				m_ScoreCounterTimers.RemoveAt(Index);
			}
		}
		
		//Check score overlay
		m_ScoreOverlay.isVisible = m_ScoreCounterTimers.Count > 0;
		if (m_ScoreOverlay.isVisible) {
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
			if (Touched) increaseScore(500);
		}
	}
	
	protected void processHealthCounter(FTouch[] touches) {
		//While not all timer
		int Index = 0;
		while (Index < m_HealthCounterTimers.Count) {
			//Manage time
			m_HealthCounterTimers[Index] -= Time.deltaTime;
			if (m_HealthCounterTimers[Index] > 0) Index++;
			else {
				//Remove
				if (!m_HealthOverlay.isVisible) incrementError();
				m_HealthCounterTimers.RemoveAt(Index);
				m_HealthChanges.RemoveAt(Index);
			}
		}
		
		//Check health overlay
		m_HealthOverlay.isVisible = m_HealthCounterTimers.Count > 0;
		if (m_HealthOverlay.isVisible) {
			//For each touch
			bool Touched = false;
			for (int i = 0; i < touches.Length && !Touched; i++) {
				//If done
				if (touches[i].phase == TouchPhase.Ended) {
					//Check position
					float TouchX 		= touches[i].position.x;
					float TouchY 		= touches[i].position.y;
					float HalfWidth		= m_HealthCounter.textRect.width * 0.5f;
					float HalfHeight 	= m_HealthCounter.textRect.height * 0.5f;
					if (TouchX >= m_HealthCounter.x - HalfWidth && TouchX <= m_HealthCounter.x + HalfWidth && TouchY >= m_HealthCounter.y - HalfHeight && TouchY <= m_HealthCounter.y + HalfHeight) Touched = true;
				}	
			}
			
			//If touched
			if (Touched) changeHealth();
		}
	}

	protected void checkTouchedObjects(FTouch[] touches) {
		foreach(FTouch touch in touches) {
			//Enemy
			List<Enemy> deadEnemies = new List<Enemy>();
			for (int i = 0; i < m_Enemies.GetChildCount(); i++) {
				//Get enemy
				Enemy enemy = m_Enemies.GetChildAt(i) as Enemy;
				if (enemy != null && enemy.ShouldBeTouched() && enemy.IsTouched(touch.position)) {
					deadEnemies.Add(enemy);
					increaseScore(50);
					break;
				}
					
			}
			foreach(Enemy enemy in deadEnemies) m_Enemies.RemoveChild(enemy);

			//Player bullets
			List<PlayerBullet> deadPlayerBullets = new List<PlayerBullet>();
			for (int i = 0; i < m_PlayerBullets.GetChildCount(); i++) {
				//Get player bullets
				PlayerBullet bullet = m_PlayerBullets.GetChildAt(i) as PlayerBullet;
				if (bullet != null && bullet.ShouldBeTouched() && bullet.IsTouched(touch.position)) {
					deadPlayerBullets.Add(bullet);
					break;
				}
					
			}
			foreach(PlayerBullet bullet in deadPlayerBullets) m_PlayerBullets.RemoveChild(bullet);
		}
	}
	
	protected void processEnemies() {
		//Initialize
		List<FNode> Deads = new List<FNode>();
		
		//For each child in the container
		for (int i = 0; i < m_Enemies.GetChildCount(); i++) {
			//Get enemy
			Enemy enemy = m_Enemies.GetChildAt(i) as Enemy;
			if (enemy != null) {
				//Move
				enemy.x -= 5.0f;
				enemy.UpdateDuration(Time.deltaTime);

				if (enemy.ShouldBeDead()) incrementError();
				if (enemy.x + (enemy.getWidth() / 2) < 0) Deads.Add(enemy);
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

	protected void processPlayerBullets(Enemy enemy) {
		//Initialize
		List<FNode> Deads 			= new List<FNode>();
		List<FNode> HittedEnemies 	= new List<FNode>();
		
		//For each child in the container
		for (int i = 0; i < m_PlayerBullets.GetChildCount(); i++) {
			//Get enemy
			PlayerBullet Bullet = m_PlayerBullets.GetChildAt(i) as PlayerBullet;
			if (Bullet != null) {
				//Move
				Bullet.x += 8.0f;
				Bullet.Update(Time.deltaTime);

				if (Bullet.ShouldBeDead()) {
					Deads.Add(Bullet);
					incrementError();
				}

				if (enemy != null && !Bullet.ShouldBeTouched() && Bullet.doesCollide(enemy)) {
					HittedEnemies.Add(enemy);
					Bullet.EnableTouchChecking();
				}			
			}
		}
		
		//Remove deads
		for (int i = 0; i < Deads.Count; i++) m_PlayerBullets.RemoveChild(Deads[i]);

		//Activate hitted enemies
		foreach(Enemy hitted in HittedEnemies) {
			hitted.EnableTouchChecking();
		}
		
		//Spawn
		m_PlayerBulletTimer -= Time.deltaTime;
		if (m_PlayerBulletTimer <= 0f && enemy != null && enemy.y == m_Exa.y) {
			//Create
			Vector2 playerPos 	= m_Exa.GetPosition();
			float X 			= playerPos.x + 10.0f;
			float Y				= playerPos.y;
			PlayerBullet bullet = new PlayerBullet(X,Y);
			bullet.SetBorder(m_PlayerBulletBorder);

			//Add
			m_PlayerBullets.AddChild(bullet);

			//Reset
			m_PlayerBulletTimer = 1.0f;
		}
	}

	//Data
	protected int 			m_Score;
	protected int			m_Error;
	protected float 		m_Health;
	protected float 		m_CoinCounterTime;
	protected float 		m_EnemyTimer;
	protected float 		m_PlayerBulletTimer;
	protected float 		m_PlayerBulletBorder;
	protected bool			m_Started;
	protected List<float> 	m_HealthChanges;
	protected List<float> 	m_ScoreCounterTimers;
	protected List<float> 	m_HealthCounterTimers;
	
	//Components
	protected Exa			m_Exa;
	protected FContainer	m_Enemies;
	protected FContainer	m_PlayerBullets;
	
	//Interface
	protected FLabel 	m_ScoreCounter;
	protected FLabel 	m_ErrorCounter;
	protected FLabel 	m_HealthCounter;
	protected FSprite	m_HealthOverlay;
	protected FSprite	m_ScoreOverlay;
}
