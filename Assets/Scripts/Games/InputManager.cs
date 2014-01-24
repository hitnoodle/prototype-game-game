
using UnityEngine;

public class InputManager {
	protected InputManager() {
		//Reset
		reset();
	}

	public static InputManager instance {
		get {
			//Create if null
			if (s_Instance == null) s_Instance = new InputManager();
			return s_Instance;
		}
	}
	
	public void reset() {
		//Reset
		m_Jump 		= false;
		m_Dash		= false;
		m_JumpPress	= false;
		m_DashPress	= false;
	}
	
	//Accessor
	public bool jumpDetected() { return m_Jump; }
	public bool dashDetected() { return m_Dash; }
	
	public void update(FTouch[] touches) {
		//Clear
		if (m_Jump && m_JumpPress) m_Jump = false;
		if (m_Dash && m_DashPress) m_Dash = false;
		
		//If no jump input
		if (!m_JumpPress) {
			//Check key
			if (isKeyPressed(JUMP_KEYS)) m_JumpPress = true;
			else if (touches != null) {
				//For each touch
				for (int i = 0; i < touches.Length && !m_JumpPress; i++) {
					//If on the left, jump
					if (touches[i].position.x < Futile.screen.halfWidth) m_JumpPress = true;
				}
			}
			
			//If pressed, jump
			if (m_JumpPress) m_Jump = true;
		} else {
			//Check key
			if (isKeyReleased(JUMP_KEYS)) m_JumpPress = false;
			else {
				//No press if no touch
				if (touches == null) m_JumpPress = false;
				else if (touches != null) {
					//Any valid touch?
					m_JumpPress = false;
					for (int i = 0; i < touches.Length && !m_JumpPress; i++) {
						//If on the left, jump
						if (touches[i].position.x < Futile.screen.halfWidth) m_JumpPress = true;
					}
				}
			}
		}
		
		//If no dash input
		if (!m_DashPress) {
			//Check key
			if (isKeyPressed(DASH_KEYS)) m_DashPress = true;
			else if (touches != null) {
				//For each touch
				for (int i = 0; i < touches.Length && !m_DashPress; i++) {
					//If on the left, dash
					if (touches[i].position.x >= Futile.screen.halfWidth) m_DashPress = true;
				}
			}
			
			//If pressed, dash
			if (m_DashPress) m_Dash = true;
		} else {
			//Check key
			if (isKeyReleased(DASH_KEYS)) m_DashPress = false;
			else {
				//No press if no touch
				if (touches == null) m_DashPress = false;
				else if (touches != null) {
					//Any valid touch?
					m_DashPress = false;
					for (int i = 0; i < touches.Length && !m_DashPress; i++) {
						//If on the left, dash
						if (touches[i].position.x >= Futile.screen.halfWidth) m_DashPress = true;
					}
				}
			}
		}
	}
	
	protected bool isKeyPressed(KeyCode[] keys) {
		//Initialize
		bool Pressed = false;
		if (keys != null) {
			//Check each key
			for (int i = 0; i < keys.Length && !Pressed; i++) if (Input.GetKeyDown(keys[i])) Pressed = true;
		}
		
		//Return
		return Pressed;
	}
	
	protected bool isKeyReleased(KeyCode[] keys) {
		//Initialize
		bool Released = false;
		if (keys != null) {
			//Check each key
			for (int i = 0; i < keys.Length && !Released; i++) if (Input.GetKeyUp(keys[i])) Released = true;
		}
		
		//Return
		return Released;
	}
	
	//The only instance
	private static InputManager s_Instance;
	
	//Constants
	protected static readonly KeyCode[] DASH_KEYS = { KeyCode.Z		};
	protected static readonly KeyCode[] JUMP_KEYS = { KeyCode.Space	};
	
	//Data
	protected bool m_Jump;
	protected bool m_Dash;
	protected bool m_JumpPress;
	protected bool m_DashPress;
}
