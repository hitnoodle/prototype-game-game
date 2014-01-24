using UnityEngine;

public class Coin : Collidable {
	public Coin(float xPosition, float yPosition, float duration) : base("exa-walk", 0, 0, 0, 0) {
		//Set attributes
		m_Duration = duration;

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
		if (!m_Overlay.isVisible) {
			m_Duration -= duration;
			if (m_Duration <= 0) {
				m_Overlay.isVisible = true;
			}
		}
	}

	public bool IsTouched(Vector2 pos) {
		if (!m_Overlay.isVisible) return false;

		float posx = pos.x;
		float posy = pos.y;
		Debug.Log(posx + " " + posy + ":" + getLeft() + " " + getRight());

		return (posx >= getLeft() && posx <= getRight() && posy >= getBottom() && posy <= getTop() );
	}

	protected float 	m_Duration;
	protected FSprite	m_Overlay;
}