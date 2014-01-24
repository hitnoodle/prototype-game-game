
using System.Collections.Generic;
using UnityEngine;

public class StateGame : ExaState {
	public StateGame(): base(GAME) {
		//Create background
		FSprite Background	= new FSprite("rect") { 
			x = Futile.screen.halfWidth, 
			y = Futile.screen.halfHeight, 
			width = Futile.screen.width, 
			height = Futile.screen.height, 
			color = new Color(86.0f / 256.0f, 181.0f / 256.0f, 1.0f, 1.0f) };
		AddChild(Background);
		
		//Create components
		m_Exa 		= new Exa();
		m_Platforms	= new FContainer();
		AddChild(m_Platforms);
		AddChild(m_Exa);

		//Create platforms
		processPlatforms(0);
		
		Debug.Log("Scale " + Futile.displayScale + " resource scale " + Futile.resourceScale);
		Debug.Log("width " + Futile.screen.width + " height " + Futile.screen.height);
	}

	public override void onUpdate(FTouch[] touches) {
		//Get platform arrays
		Collidable[] Platforms = new Collidable[m_Platforms.GetChildCount()];
		for (int i = 0; i < Platforms.Length; i++) Platforms[i] = m_Platforms.GetChildAt(i) as Collidable;
	
		//Update
		m_Exa.update(Platforms);
		processPlatforms(m_Exa.getOffset());
		
		//Check input
		InputManager.instance.update(touches);
		if (InputManager.instance.jumpDetected()) m_Exa.jump();
		if (InputManager.instance.dashDetected()) m_Exa.dash();
	}

	protected void processPlatforms(float offset) {
		//Initialize
		float Right			= -Futile.screen.halfWidth;
		List<FNode> Deads = new List<FNode>();
	
		//For each child in the container
		for (int i = 0; i < m_Platforms.GetChildCount(); i++) {
			//Get platform
			Drawable APlatform = m_Platforms.GetChildAt(i) as Drawable;
			if (APlatform != null) {
				//Move
				APlatform.x 		-= offset;
				float PlatformRight	 = APlatform.x + (APlatform.getWidth() / 2);
				if (PlatformRight < -Futile.screen.halfWidth) 	Deads.Add(APlatform);
				if (PlatformRight > Right) 						Right = PlatformRight;
			}
		}
		
		//Remove dead platforms
		for (int i = 0; i < Deads.Count; i++) m_Platforms.RemoveChild(Deads[i]);
		
		//While right is less
		while (Right < Futile.screen.width * 1.5f) {
			//Create
			float X 				= Right + (Random.Range(0, 6) * 40);
			float Y 				= Random.Range(0, ((int)(Futile.screen.height / 40) + 1)) * 40;
			Platform NewPlatform	= new Platform(X, Y);
			
			//Add
			m_Platforms.AddChild(NewPlatform);
			
			//Save
			Right = X + (NewPlatform.getWidth() / 2);
		}
	}
	
	//Data
	
	//Components
	protected Exa			m_Exa;
	protected FContainer	m_Platforms;
}
