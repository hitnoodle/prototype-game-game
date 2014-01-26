using UnityEngine;

public class Enemy : Collidable {
	public Enemy(float xPosition, float yPosition) : base(null, "enemy",0,0,0,0) {
		//Set position
		x = xPosition;
		y = yPosition;
		m_Overlay = new FSprite("target") { isVisible = false };
		AddChild(m_Overlay);
		
		//Create animation
		m_Animated.addAnimation(new FAnimation(DEFAULT_ANIMATION, new int[] { 1, 2 }, 180, true));
		m_Animated.addAnimation(new FAnimation(ATTACK_ANIMATION, new int[] 	{ 3, 4 }, 150, false));
		m_Animated.play(DEFAULT_ANIMATION);
	}

	public bool IsTouched(Vector2 pos) {
		float posx = pos.x;
		float posy = pos.y;
		
		return (posx >= getLeft() && posx <= getRight() && posy >= getBottom() && posy <= getTop());
	}

	public void UpdateDuration(float duration) {
		//What the fuck is this shit
		if (m_Animated.currentAnim.name == ATTACK_ANIMATION) {
			if (m_AttackTimer <= 0) {
				m_Animated.play(DEFAULT_ANIMATION);
				m_AttackTimer = 0;
			} else m_AttackTimer -= duration;
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

	public void Attack() {
		m_Animated.play(ATTACK_ANIMATION);
		m_AttackTimer = 0.3f;
	}

	protected float 		m_ActivatedDuration;
	protected bool 			m_Activated;
	protected bool 			m_Dead;
	protected FSprite		m_Overlay;
	protected float 		m_AttackTimer;

	protected const float OVERLAY_DURATION 		= 0.75f;
	protected const string DEFAULT_ANIMATION	= "default";
	protected const string ATTACK_ANIMATION		= "attack";
}
