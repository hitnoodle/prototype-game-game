using UnityEngine;

public class EnemyBullet : Collidable {
	public EnemyBullet(float xPosition, float yPosition) : base("knife",0,0,0,0) {
		//Set position
		x = xPosition;
		y = yPosition;
	}

	public void SetTarget(Vector2 target) {
		Vector2 min = target - GetPosition();
		float rotate = Mathf.Atan2(min.x, min.y);

		m_Speed = new Vector2(6f * Mathf.Sin(rotate), 5f * Mathf.Cos(rotate));
	}

	public bool IsOutOfScreen() {
		return (x + (m_Width / 2) <= 0 || y - (m_Height / 2) >= Futile.screen.height || y +(m_Height / 2) <= 0);
	}
	
	public void Update(float duration) {
		x += m_Speed.x;
		y += m_Speed.y;
	}
	
	protected Vector2 m_Speed;
}
