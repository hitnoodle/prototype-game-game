using UnityEngine;
using System.Collections;

public class Platform : Collidable {
	public Platform(float xPosition, float yPosition) : base("platform", 5, 0, 5, 0) {
		//Set position
		x = xPosition;
		y = yPosition;
	}
}
