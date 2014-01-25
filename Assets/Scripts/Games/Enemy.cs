using UnityEngine;

public class Enemy : Collidable {
	public Enemy(float xPosition, float yPosition) : base("exa-walk",0,0,0,0) {
		//Set position
		x = xPosition;
		y = yPosition;

		//Set attributes
		setColor(1, 0, 0, 1);

		m_Overlay = new FSprite("rect") {
			x = 0, 
			y = 0, 
			width = m_Width, 
			height = m_Height, 
			color = new Color(255.0f, 255.0f, 255.0f, 0.35f) };
		AddChild(m_Overlay);
		m_Overlay.isVisible = false;
	}

	public bool IsTouched(Vector2 pos) {
		float posx = pos.x;
		float posy = pos.y;
		
		return (posx >= getLeft() && posx <= getRight() && posy >= getBottom() && posy <= getTop());
	}

	public void UpdateDuration(float duration) {
		if (m_Activated && !m_Overlay.isVisible) return;
		
		m_ActivatedDuration -= duration;
		if (m_ActivatedDuration <= 0) {
			m_Overlay.isVisible = false;
			m_Dead = true;
		}
	}

	public void EnableTouchChecking() {
		m_Overlay.isVisible = true;
		m_ActivatedDuration = OVERLAY_DURATION;
		m_Activated = true;
	}

	public bool ShouldBeTouched() {
		return m_Overlay.isVisible;
	}

	public bool ShouldBeDead() {
		bool retval = m_Dead && !m_Overlay.isVisible && m_Activated;
		if (m_Dead) m_Dead = false;
		return retval;
	}

	protected float 	m_ActivatedDuration;
	protected bool 		m_Activated;
	protected bool 		m_Dead;
	protected FSprite	m_Overlay;
	
	protected const float OVERLAY_DURATION = 0.75f;
}
