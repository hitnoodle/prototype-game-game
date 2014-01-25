
public class Enemy : Collidable {
	public Enemy(float xPosition, float yPosition) : base("exa-walk",0,0,0,0) {
		//Set position
		x = xPosition;
		y = yPosition;

		//Set attributes
		setColor(1, 0, 0, 1);
	}
}
