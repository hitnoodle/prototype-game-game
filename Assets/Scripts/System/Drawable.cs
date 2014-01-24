
using UnityEngine;

public class Drawable : FContainer {
	public Drawable(string name) {
		//Initialize
		m_Width		= 0;
		m_Height	= 0;
		m_Sprite	= null;

		//if name exist
		if (name != null) createSprite(new FSprite(name));
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

	//Accessor
	public float getWidth()		{ return m_Width;			}
	public float getHeight()	{ return m_Height;			}
	public float getScaleX()	{ return m_Sprite.scaleX;	}
	public float getScaleY()	{ return m_Sprite.scaleY;	}
	
	public void setColor(float r, float g, float b, float a) {
		//IF sprite exist
		if (m_Sprite != null) m_Sprite.color = new Color(r, g, b, a);
	}

	protected void SetScaleX(float scaleX) {
		m_Width *= scaleX / m_Sprite.scaleX;
		m_Sprite.scaleX = scaleX;
	}
	
	protected void SetScaleY(float scaleY) {
		m_Height *= scaleY / m_Sprite.scaleY;
		m_Sprite.scaleY = scaleY;
	}

	//Data
	protected float		m_Width;
	protected float		m_Height;
	protected FSprite	m_Sprite;
}
