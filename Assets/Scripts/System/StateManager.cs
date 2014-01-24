
public class StateManager : FContainer, FMultiTouchableInterface {
	protected StateManager() {
		//Initialize variable
		m_Factory		= null;
		m_Running		= true;
		m_Touches		= new FTouch[] {};
		m_Initialized	= false;
		m_RemovalDepth	= 0;

		//Listen
		ListenForUpdate(onUpdate);
		ListenForResize(onResize);
		EnableMultiTouch();
	}

	public static StateManager instance {
		get {
			//Create if null
			if (s_Instance == null) s_Instance = new StateManager();
			return s_Instance;
		}
	}

	public void setup(StateFactory factory) {
		//Set factory
		m_Factory = factory;
	}

	protected void initialize() {
		//Skip if initialized
		if (m_Initialized) return;
		m_Initialized = true;

		//Go to first state
		if (m_Factory != null) goTo(m_Factory.getFirstState(), null, false);
	}

	public void quit() {
		//Kill all states
		int ChildNumber = GetChildCount();
		for (int i = 0; i < ChildNumber; i++) removeState();

		//No longer running
		m_Running = false;
	}

	public void goTo(int id, object[] parameters, bool swap) {
		//Prepare variables
		int i		= 0;
		bool Exist	= false;

		//Find state
		while (!Exist && i < GetChildCount()) {
			//Is it exist?
			if (GetChildAt(i) is ExaState)
				if (((ExaState)GetChildAt(i)).getID() == id) Exist = true;

			//Next
			i++;
		}

		//If exist, return, otherwise, add a new one
		if (Exist) returnTo(id, parameters);
		else {
			//Create new state
			ExaState NewState = null;
			if (m_Factory != null) NewState = m_Factory.createState(id, parameters);
			if (NewState == null) return;

			//Add state
			addState(NewState);

			//If swapped and there's many states
			if (swap && GetChildCount() >= 2) {
				//Remove top
				m_RemovalDepth++;

				//Reorder
				AddChildAtIndex(NewState, GetChildCount() - 2);
			}
		}
	}

	protected void addState(ExaState state) {
		//Skip if no state
		if (state == null) return;

		//If there's a previous state
		if (GetChildCount() > 0) {
			//Get previous state
			ExaState Previous = GetChildAt(GetChildCount() - 1) as ExaState;
			if (Previous != null) Previous.onExit();
		}

		//Add
		AddChild(state);

		//Initialize
		state.initialize();
		state.onEnter();
	}

	protected void removeState() {
		//Validate
		if (GetChildCount() <= 0) return;

		//Get top state
		ExaState Top = GetChildAt(GetChildCount() - 1) as ExaState;
		if (Top != null) {
			//Remove
			Top.onExit();
			Top.onRemove();
			RemoveChild(Top);

			//If there's a child
			if (GetChildCount() > 0) {
				ExaState NewTop = GetChildAt(GetChildCount() - 1) as ExaState;
				if (NewTop != null) NewTop.onEnter();
			}
		}
	}

	protected void returnTo(int id, object[] parameters) {
		//Initialize
		m_RemovalDepth = 0;
		bool Found = false;
		int i = GetChildCount() - 1;

		//While not found
		while (i >= 0 & !Found) {
			//Get current state
			ExaState Current = GetChildAt(i) as ExaState;
			if (Current != null && Current.getID() == id) {
				//Found
				Found = true;
				Current.onEnter();
			} else m_RemovalDepth++;

			//Next
			i--;
		}
	}

	protected void onResize(bool orientation) {
		//Resize all child
		for (int i = 0; i < GetChildCount(); i++) {
			ExaState Child = GetChildAt(i) as ExaState;
			if (Child != null) Child.onResize(orientation);
		}
	}

	public void HandleMultiTouch(FTouch[] touches) {
		//Save touches
		m_Touches = touches;
	}

	protected void onUpdate() {
		//Initialize if not
		if (!m_Initialized) initialize();

		//Trim states
		for (int i = 0; i < m_RemovalDepth; i++) removeState();
		m_RemovalDepth = 0;

		//Top state exist?
		bool TopExist		= false;
		bool ActiveFound	= false;
		if (GetChildCount() > 0) TopExist = GetChildAt(GetChildCount() - 1) != null;

		//While not empty
		while (GetChildCount() > 0 && TopExist && !ActiveFound) {
			//If top state is active, found
			ExaState Top = GetChildAt(GetChildCount() - 1) as ExaState;
			if (Top != null && Top.isActive())	ActiveFound = true;
			else								removeState();

			//Check
			TopExist = GetChildCount() > 0 && GetChildAt(GetChildCount() - 1) != null;
		}

		//Quit if empty or no last state
		if (GetChildCount() <= 0 || !TopExist) m_Running = false;
		else {
			//Update last state
			ExaState Top = GetChildAt(GetChildCount() - 1) as ExaState;
			if (Top != null) {
				//Update and clear touches
				Top.onUpdate(m_Touches);
				m_Touches = new FTouch[] {};
			}
		}
	}

	//The only instance
	private static StateManager s_Instance;

	//Data
	protected FTouch[]		m_Touches;
	protected StateFactory	m_Factory;
	protected bool			m_Running;
	protected bool			m_Initialized;
	protected int			m_RemovalDepth;
}
