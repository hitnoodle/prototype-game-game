
public class PlayerBullet : Collidable {
	public PlayerBullet(float xPosition, float yPosition) : base("exa-walk",0,0,0,0) {
		//Set position
		x = xPosition;
		y = yPosition;

		//Scale and set collision
		SetScaleX(0.25f);
		SetScaleY(0.25f);
		setSize(0,0,0,0);

		//Set attributes
		setColor(0, 0, 1, 1);
	}
}
