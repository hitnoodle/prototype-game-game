
using UnityEngine;

public class Exa : Collidable {
	public Exa() : base(null, "chicken", 0, 0, 0, 0) {
		//Initialize
		m_SpeedX 	= DEFAULT_SPEED_X;
		m_SpeedY 	= DEFAULT_SPEED_Y;
		m_AccelX	= DEFAULT_ACCELERATION_X;
		m_AccelY	= DEFAULT_ACCELERATION_Y;
		m_OffsetX	= 0;
		m_Jump		= 0;
		
		//Set position
		x = Constants.UNITY_CANVAS_LEFT + (Constants.UNITY_CANVAS_WIDTH * 0.2f);
		y = (Constants.UNITY_CANVAS_BOTTOM + (Constants.UNITY_CANVAS_HEIGHT * 0.25f)) + (getHeight() * 0.5f);
		
		//Create animation
		m_Animated.addAnimation(new FAnimation(DEFAULT_ANIMATION, new int[] { 1, 2, 3 }, 100, true));
		m_Animated.play(DEFAULT_ANIMATION);
	}
	
	public float getOffset() { return m_OffsetX; }
	
	public void jump() {
		//Skip if maximum jump
		if (m_Jump >= 2) return;
	
		//Set jump speed
		m_SpeedY = JUMP_SPEED;
		m_Jump++;
	}
	
	public void dash() {
		//Skip if still dashing
		if (m_SpeedX > DEFAULT_SPEED_X) return;
		 
		//Set dash speed
		m_SpeedX += DASH_SPEED;
		m_AccelX  = DASH_ACCEL;
		
		//Reset jump
		m_Jump		= 0;
		m_SpeedY 	= DEFAULT_SPEED_Y;
		m_AccelX 	= DEFAULT_ACCELERATION_Y;
	}
	
	public void update() {
		//Manage speed
		m_SpeedX += m_AccelX * Time.deltaTime;
		if (m_SpeedX < DEFAULT_SPEED_X) {
			//Reset
			m_SpeedX = DEFAULT_SPEED_X;
			m_AccelX = DEFAULT_ACCELERATION_X;
		}
		
		//Calculate offset
		m_OffsetX = m_SpeedX * Time.deltaTime;
		
		//If not dashing
		if (m_SpeedX <= DEFAULT_SPEED_X) {
			//Manage height
			/*m_SpeedY 	+= m_AccelY * Time.deltaTime;
			y 			+= m_SpeedY * Time.deltaTime;
			
			//Check platforms
			/bool Landed	= checkPlatforms(platforms);
			if (y < (m_Height / 2) - 1) {
				//Limit
				y 			= (m_Height / 2) - 1;
				Landed		= true;
			}
			
			//If landed
			if (Landed) {
				//Reset
				m_SpeedY 	= DEFAULT_SPEED_Y;
				m_Jump		= 0;
			}*/
		}
	}
	
	protected bool checkPlatforms(Collidable[] platforms) {
		//Skip if no platforms
		bool Landed = false;
		if (platforms != null) {
			//For each platforms
			for (int i = 0; i < platforms.Length; i++) {
				//If collide
				if (doesCollide(platforms[i])) {
					//If going down and still above
					if (y - (m_Height / 4) > platforms[i].getTop() && m_SpeedY <= 0) {
						//Check left and right
						if (x < platforms[i].getRight()) {
							//Set Y
							y 		= platforms[i].getTop() + (m_Height / 2) - 1;
							Landed 	= true;
						}
					}
				}
			}
		}
		
		//Return
		return Landed;
	}
	
	//Constants
	protected const float JUMP_SPEED 				= 280;
	protected const float DASH_SPEED 				= 280;
	protected const float DASH_ACCEL 				= -200;
	protected const float DEFAULT_SPEED_X 			= 300;
	protected const float DEFAULT_SPEED_Y 			= -50;
	protected const float MINIMUM_SPEED_Y 			= -100;
	protected const float DEFAULT_ACCELERATION_Y 	= -500;
	protected const float DEFAULT_ACCELERATION_X 	= 0;
	protected const string DEFAULT_ANIMATION		= "default";
	
	//Data
	protected float m_AccelX;
	protected float m_AccelY;
	protected float m_SpeedX;
	protected float m_SpeedY;
	protected float m_OffsetX;
	protected int	m_Jump;
}
