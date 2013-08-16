using UnityEngine;
using System.Collections;
using System;

public class WTComponent : FContainer {
	public string name {get; private set;}
	public WTEntity entity {get; private set;}
	public event Action<WTComponent> SignalComponentDestroyed;

	public WTComponent(string name) {
		this.name = name;
	}

	virtual public void HandleAddedToEntity(WTEntity entity) {
		this.entity = entity;
		entity.AddChild(this);
	}
	
	virtual public void Destroy() {
		if (SignalComponentDestroyed != null) SignalComponentDestroyed(this);

		RemoveFromContainer();
	}
}
