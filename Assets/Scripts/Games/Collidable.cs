
public abstract class Collidable : Drawable {
	protected Collidable(string name, string animated, float left, float top, float right, float bottom) : base(name, animated) {
		//Initialize
		m_CollideTop	= 0;
		m_CollideLeft	= 0;
		m_CollideRight	= 0;
		m_CollideBottom	= 0;

		//If sprite exist
		if (m_Sprite != null || m_Animated != null) setSize(left, top, right, bottom);
	}

	protected void setSize(float left, float top, float right, float bottom) {
		//Calculate
		m_CollideTop	= (m_Height / 2) - top;
		m_CollideLeft	= (m_Width / 2) - left;
		m_CollideRight	= (m_Width / 2) - right;
		m_CollideBottom = (m_Height / 2) - bottom;
	}

	//Accessors
	public float getTop()		{ return y + m_CollideTop;		}
	public float getLeft()		{ return x - m_CollideLeft;		}
	public float getRight()		{ return x + m_CollideRight;	}
	public float getBottom()	{ return y - m_CollideBottom;	}

	public bool doesCollide(Collidable other) {
		//Validate
		if (other == null) return false;

		//Check
		if (getTop() < other.getBottom())	return false;
		if (getLeft() > other.getRight())	return false;
		if (getRight() < other.getLeft())	return false;
		if (getBottom() > other.getTop())	return false;

		//Otherwise, colliding
		return true;
	}

	//Data
	protected float		m_CollideTop;
	protected float		m_CollideLeft;
	protected float		m_CollideRight;
	protected float		m_CollideBottom;
}
