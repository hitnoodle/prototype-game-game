using UnityEngine;

public class PlayerBullet : Collidable {
	public PlayerBullet(float xPosition, float yPosition) : base("exa-walk",0,0,0,0) {
		//Set position
		x = xPosition;
		y = yPosition;

		//Scale and set collision
		SetScaleX(0.25f);
		SetScaleY(0.25f);
		setSize(0,0,0,0);

		//Set attributes
		setColor(0, 0, 1, 1);

		m_Overlay = new FSprite("rect") {
			x = 0, 
			y = 0, 
			width = m_Width * 2, 
			height = m_Height * 2, 
			color = new Color(255.0f, 255.0f, 255.0f, 0.35f) };
		AddChild(m_Overlay);
		m_Overlay.isVisible = false;
	}

	public void SetBorder(float border) {
		m_Border = border;
	}

	public bool IsTouched(Vector2 pos) {
		float posx = pos.x;
		float posy = pos.y;

		float left 		= (x + m_Overlay.x) - (m_Overlay.width / 2);
		float right 	= (x + m_Overlay.x) + (m_Overlay.width / 2);
		float bottom 	= (y + m_Overlay.y) - (m_Overlay.height / 2);
		float top 		= (y + m_Overlay.y) + (m_Overlay.height / 2);

		bool retval = (posx >= left && posx <= right && posy >= bottom && posy <= top);
		return retval;
	}

	public void Update(float duration) {
		if (getRight() >= m_Border) {
			m_Overlay.x = m_Border - getRight();
		}

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
	protected float 	m_Border;
	
	protected const float OVERLAY_DURATION = 2f;
}
