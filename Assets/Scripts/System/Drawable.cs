
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
	public float getWidth()		{ return m_Width;	}
	public float getHeight()	{ return m_Height;	}

	//Data
	protected float		m_Width;
	protected float		m_Height;
	protected FSprite	m_Sprite;
}
