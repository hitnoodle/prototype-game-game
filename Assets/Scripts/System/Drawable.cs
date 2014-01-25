
using UnityEngine;

public class Drawable : FContainer {
	public Drawable (string sprite) {
		//Initialize
		m_Width		= 0;
		m_Height	= 0;
		m_Sprite	= null;
		m_Animated	= null;
		
		//if name exist
		if (sprite != null) createSprite(new FSprite(sprite));
	}
	
	public Drawable(string sprite, string animated) : this(sprite) {
		//Check animated
		if (sprite == null && animated != null) createAnimatedSprite(new FAnimatedSprite(animated));
	}

	protected void createSprite(FSprite sprite) {
		//Skip if no sprite
		if (sprite == null) return;

		//Save sprite
		m_Sprite = sprite;
		AddChild(m_Sprite);

		//Set size
		m_Width = m_Sprite.textureRect.width;
		m_Height = m_Sprite.textureRect.height;
	}

	protected void createAnimatedSprite(FAnimatedSprite sprite) {
		//Skip if no sprite
		if (sprite == null) return;

		//Save sprite
		m_Animated = sprite;
		AddChild(m_Animated);

		//Set size
		m_Width 	= m_Animated.textureRect.width;
		m_Height 	= m_Animated.textureRect.height;
	}

	//Accessor
	public float getWidth()		{ return m_Width;			}
	public float getHeight()	{ return m_Height;			}

	//Data
	protected float				m_Width;
	protected float				m_Height;
	protected FSprite			m_Sprite;
	protected FAnimatedSprite	m_Animated;
}
