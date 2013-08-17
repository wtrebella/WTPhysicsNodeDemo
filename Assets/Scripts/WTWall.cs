using UnityEngine;
using System.Collections;

// just a stupid simple rigid wall for things to bounce off of

public class WTBasicWall : WTPhysicsNode {
	FSprite sprite;

	public WTBasicWall(float width, float height) : base("wall") {
		sprite = new FSprite("whiteSquare");
		sprite.width = width;
		sprite.height = height;
		sprite.color = new Color(0.1f, 0.1f, 0.1f);
		AddChild(sprite);

		physicsComponent.AddBoxCollider(sprite.width, sprite.height);
		physicsComponent.SetupPhysicMaterial(1.0f, 0.1f, 0.1f);
	}
}
