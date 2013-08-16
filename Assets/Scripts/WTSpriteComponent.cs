using UnityEngine;
using System.Collections;

public class WTSpriteComponent : WTComponent {
	public FSprite sprite;

	public WTSpriteComponent(string name, FSprite sprite) : base(name) {
		this.sprite = sprite;
		AddChild(sprite);
	}

	override public void Destroy() {
		base.Destroy();

		sprite.RemoveFromContainer();
	}
}
