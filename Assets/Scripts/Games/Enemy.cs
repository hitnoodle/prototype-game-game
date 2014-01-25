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

	public void UpdateDuration(float duration) {
		if (m_Activated && !m_Overlay.isVisible) return;
		
		m_ActivatedDuration -= duration;
		if (m_ActivatedDuration <= 0) 
			m_Overlay.isVisible = false;
	}

	public void EnableTouchChecking() {
		m_Overlay.isVisible = true;
		m_ActivatedDuration = OVERLAY_DURATION;
	}

	public bool ShouldBeTouched() {
		return m_Overlay.isVisible;
	}

	protected float 	m_ActivatedDuration;
	protected bool 		m_Activated;
	protected FSprite	m_Overlay;
	
	protected const float OVERLAY_DURATION = 0.75f;
}
