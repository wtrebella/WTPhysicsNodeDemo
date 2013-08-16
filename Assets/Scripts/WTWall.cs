using UnityEngine;
using System.Collections;

public class WTWall : WTPhysicsNode {
	FSprite sprite;

	public WTWall(float width, float height) : base("wall") {
		sprite = new FSprite("whiteSquare");
		sprite.width = width;
		sprite.height = height;
		sprite.color = new Color(0.1f, 0.1f, 0.1f);
		AddChild(sprite);

		physicsComponent.AddBoxCollider(sprite.width, sprite.height);
		physicsComponent.SetupPhysicMaterial();
		SetNewPosition(WTUtils.screenCenter);
	}
}
