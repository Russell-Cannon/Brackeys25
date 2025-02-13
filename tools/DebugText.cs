using Godot;
using System;
using System.Collections.Generic;

class Message {
	//Use key to edit an existing message
	public string text = "", key = "";
	//When the message was posted to detect old messages
	public double creationTimeCode = 0;
	//sticky keeps the message around permentantly.
	public bool sticky = false;
}

public partial class DebugText
{
	List<Message> messages = new();
	Label text = new();
	//Needlessly expensive to use unittime codes as we don't need their precision
    long secondsPassed;
    public void Ready()
    {
		Timer timer = new() {
			OneShot = false,
			Autostart = true,
			WaitTime = 1.0
		};
		timer.Timeout += checkMessagesAge;
		FT.Instance.AddChild(timer); //Adds a timer that triggers every second

		FT.Instance.AddChild(text);
    }

	void updateMessages() {
		text.Text = "";
		foreach (Message m in messages) {
			text.Text += m.text + "\n";			
		}
	}

	public void checkMessagesAge() {
		secondsPassed++;
		for (int i = messages.Count - 1; i >= 0; i--) {
			if (secondsPassed - messages[i].creationTimeCode > 1.0 && !messages[i].sticky)//remove old but not sticky messages
				messages.Remove(messages[i]);
		}
		updateMessages();
	}
    public void post(string message) {
		post(message, "", false);
	}
    public void post(string message, string key) {
		post(message, key, false);
	}
    public void post(string message, bool sticky) {
		post(message, "", sticky);
	}
    public void post(string message, string key, bool sticky) {
		if (key != "") { //Update message with the same key
			for (int i = 0; i < messages.Count; i++) {
				if (messages[i].key == key) {//update keyed message if possible
					messages[i].text = message;
					messages[i].creationTimeCode = secondsPassed;//reset timer
					updateMessages();
					return;
				}
			}
		}

		Message m = new() { //Add new message
			text = message,
			creationTimeCode = secondsPassed,
			key = key,
			sticky = sticky
		};
		messages.Add(m);
		updateMessages();
	}
}
