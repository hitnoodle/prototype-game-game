using UnityEngine;

public class Coin : Collidable {
	public Coin(float xPosition, float yPosition, float duration) : base("exa-walk", 0, 0, 0, 0) {
		//Set attributes
		m_Duration = duration;
		m_Activated = false;

		//Set position
		x = xPosition;
		y = yPosition;

		//Scale and set collision
		SetScaleX(0.5f);
		SetScaleY(0.5f);
		setSize(0,0,0,0);

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

		m_Duration -= duration;
		if (m_Duration <= 0) {
			if (!m_Activated) {
				m_Duration = OVERLAY_DURATION;
				m_Overlay.isVisible = true;
				m_Activated = true;
			} else {
				m_Overlay.isVisible = false;
			}
		}
	}

	public bool IsTouched(Vector2 pos) {
		float posx = pos.x;
		float posy = pos.y;
		Debug.Log(posx + " " + posy + ":" + getLeft() + " " + getRight());

		return (posx >= getLeft() && posx <= getRight() && posy >= getBottom() && posy <= getTop() );
	}

	public bool ShouldBeTouched() {
		return m_Overlay.isVisible;
	}

	protected float 	m_Duration;
	protected bool 		m_Activated;
	protected FSprite	m_Overlay;

	protected const float OVERLAY_DURATION = 0.75f;
}