﻿using Sandbox;

namespace TerryTheHotDog
{
	public class CustomCamera : Camera
	{
		[ConVar.Replicated]
		public static bool thirdperson_orbit { get; set; } = false;

		[ConVar.Replicated]
		public static bool thirdperson_collision { get; set; } = true;

		private Angles orbitAngles;
		private float orbitDistance = 150;


		public void setFOV(float i)
		{
			FieldOfView = i;
		}


		 static public float minFOV = 90;
		 static public float maxFOV = 150;

		public override void Update()
		{
			var pawn = Local.Pawn as AnimEntity;
			var client = Local.Client;

			if ( pawn == null )
				return;

			Pos = pawn.Position;
			Vector3 targetPos;

			var center = pawn.Position + Vector3.Up * 64;

			if ( thirdperson_orbit )
			{
				Pos += Vector3.Up * (pawn.CollisionBounds.Center.z * pawn.Scale);
				Rot = Rotation.From( orbitAngles );

				targetPos = Pos + Rot.Backward * orbitDistance;
			}
			else
			{
				Pos = center;
				Rot = Rotation.FromAxis( Vector3.Up, 4 ) * Input.Rotation;

				float distance = 130.0f * pawn.Scale;
				targetPos = Pos + Input.Rotation.Right * ((pawn.CollisionBounds.Maxs.x + 15) * pawn.Scale);
				targetPos += Input.Rotation.Forward * -distance;
			}

			if ( thirdperson_collision )
			{
				var tr = Trace.Ray( Pos, targetPos )
					.Ignore( pawn )
					.Radius( 8 )
					.Run();

				Pos = tr.EndPos;
			}
			else
			{
				Pos = targetPos;
			}


			// FOV SMOOTHING POG dog
			FieldOfView = Input.Down( InputButton.Run ) ? FieldOfView.LerpTo( maxFOV, (Time.Delta * 15) , false ) : FieldOfView.LerpTo( minFOV, Time.Delta, false );
			

			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( thirdperson_orbit && input.Down( InputButton.Walk ) )
			{
				if ( input.Down( InputButton.Attack1 ) )
				{
					orbitDistance += input.AnalogLook.pitch;
					orbitDistance = orbitDistance.Clamp( 0, 1000 );
				}
				else
				{
					orbitAngles.yaw += input.AnalogLook.yaw;
					orbitAngles.pitch += input.AnalogLook.pitch;
					orbitAngles = orbitAngles.Normal;
					orbitAngles.pitch = orbitAngles.pitch.Clamp( -89, 89 );
				}

				input.AnalogLook = Angles.Zero;

				input.Clear();
				input.StopProcessing = true;
			}

			base.BuildInput( input );
		}
	}
}
