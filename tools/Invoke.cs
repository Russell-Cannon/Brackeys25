using Godot;
using System;
using System.Collections.Generic;

public partial class Invoke
{
	List<Action> actions = new();
	List<Action<double>> actionsDelta = new();
    public void Process(double delta) {
		for (int i = 0; i < actions.Count; i++) {
			actions[i]();
		}
		for (int i = 0; i < actionsDelta.Count; i++) {
			actionsDelta[i](delta);
		}
	}
    public void Do(Action action, float time) {
		Timer t = new() {
			Autostart = true,
			OneShot = true,
			WaitTime = time
		};

		FT.Instance.AddChild(t);
		t.Timeout += action += () => {t.QueueFree();};
	}
	public void DoFor(Action<double> action, float time) {
		Timer t = new() {
			Autostart = true,
			OneShot = true,
			WaitTime = time
		};

		actionsDelta.Add(action);

		FT.Instance.AddChild(t);
		t.Timeout += () => {
			actionsDelta.Remove(action);
			t.QueueFree();
		};
	}
}
