﻿using Sandbox.UI;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace TerryTheHotDog
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class MinimalHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public MinimalHudEntity()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/minimalhud.html" );
				RootPanel.AddChild<NameTags>();
				RootPanel.AddChild<CrosshairCanvas>();
				RootPanel.AddChild<ChatBox>();
				RootPanel.AddChild<VoiceList>();
				RootPanel.AddChild<KillFeed>();
				RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			}
		}
	}

}
