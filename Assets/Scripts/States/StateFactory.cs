
public class StateFactory {
	public int getFirstState() { return ExaState.GAME; }
	public ExaState createState(int id, object[] parameters) {
		//Initialize
		ExaState NewState = null;

		//Check ID
		switch(id) {
		case ExaState.TITLE:
			if (parameters != null && parameters.Length >= 1) NewState = new StateTitle((StateGame)parameters[0]);
			break;

		case ExaState.GAME:
			NewState = new StateGame();
			break;

		case ExaState.PAUSE:
			break;

		case ExaState.RESULT:
			if (parameters != null && parameters.Length >= 2) NewState = new StateResult ((StateGame)parameters [0], (float)parameters[1]);
			break;
		}

		//Return
		return NewState;
	}
}
