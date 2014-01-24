
public abstract class ExaState : FContainer {
	protected ExaState(int id) {
		//Save ID
		m_ID = id;
	}

	//Functions
	public void initialize() {
		//Set active
		m_Active = true;
	}

	//Accessors
	public int getID() 		{ return m_ID; 		}
	public bool isActive()	{ return m_Active;	}

	//Functions
	public virtual void onEnter() 					{}
	public virtual void onRemove()					{}
	public virtual void onExit()					{}
	public virtual void onResume()					{}
	public virtual void onUpdate(FTouch[] touches)	{}
	public virtual void onResize(bool orientation)	{}

	//Constants
	public const int TITLE 	= 1;
	public const int GAME 	= 2;
	public const int PAUSE 	= 3;
	public const int RESULT = 4;

	//Data
	protected int 	m_ID;
	protected bool 	m_Active;
}
