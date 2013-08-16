using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTEntity : FContainer {
	List<WTComponent> components;

	public WTEntity() {
		components = new List<WTComponent>();
	}

	public void AddComponent(WTComponent newComponent) {
		if (GetComponentWithName(newComponent.name) != null) throw new FutileException("can't add component since there's already one called " + newComponent.name);

		newComponent.HandleAddedToEntity(this);
		newComponent.SignalComponentDestroyed += HandleComponentDestroyed;
		components.Add(newComponent);
	}

	public T GetFirstComponentOfType<T>() where T : WTComponent {
		foreach (WTComponent c in components) {
			if (c.GetType() == typeof(T)) return (T)c;
		}

		return default(T);
	}

	public T GetAllComponentsOfType<T>() where T : List<WTComponent> {
		List<WTComponent> cs = new List<WTComponent>();

		foreach (WTComponent c in components) {
			if (c.GetType() == typeof(T)) cs.Add(c);
		}

		if (cs.Count > 0) return (T)cs;
		else return default(T);
	}

	public WTComponent GetComponentWithName(string name) {
		foreach (WTComponent c in components) {
			if (c.name == name) return c;
		}
		
		return null;
	}

	private void HandleComponentDestroyed(WTComponent component) {
		int componentIndex = -1;

		for (int i = 0; i < components.Count; i++) {
			if (components[i] == component) {
				componentIndex = i;
				break;
			}
		}

		if (componentIndex == -1) throw new FutileException("component not found on entity");

		components.RemoveAt(componentIndex);
	}
}
