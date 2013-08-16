using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTMain : MonoBehaviour {
	WTPhysicsNode frog;

	void Start() {
		Go.defaultEaseType = EaseType.SineInOut;

		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(480f, 1.0f, 1.0f, "-res1");
		fp.AddResolutionLevel(1136f, 2.0f, 2.0f, "-res2");
		fp.AddResolutionLevel(2048f, 4.0f, 4.0f, "-res4");
		
		fp.backgroundColor = Color.black;
		fp.origin = Vector2.zero;

		Futile.instance.Init(fp);

		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadFont("franchise", "franchise", "Atlases/franchise", -7, -16);

		// futile done initing

		WTUtils.Init();

		FPWorld.Create(64.0f);

		FSprite s = new FSprite("whiteSquare");

		frog = new WTPhysicsNode("frog");
		frog.AddChild(s);
		frog.physicsComponent.AddRigidBody();
		frog.physicsComponent.AddBoxCollider(s.width, s.height);
		frog.physicsComponent.SetupPhysicMaterial();
		frog.SetPosition(WTUtils.screenCenter);
		frog.UpdatePositions();

		frog.physicsComponent.StartPhysics();
		frog.physicsComponent.AddForceAtPosition(100, 2000, frog.GetPosition().x, frog.GetPosition().y+50);

		Futile.stage.AddChild(frog);
	}
	
	void Update() {
//		if (frog.physicsNode.CanMoveInCode()) {
//			frog.rotation += 3;
//		}
	}
}