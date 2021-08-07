using Sandbox;
using System;
using System.Linq;

namespace TerryTheHotDog
{
	partial class MinimalPlayer : Player
	{
		public override void Respawn()
		{

			
			SetModel( "models/citizen/citizen.vmdl" );
			SetMaterialGroup( 4 );

			// Clothing 
			var model = Rand.FromArray( new[]
				{
				"models/citizen_clothes/shoes/trainers.vmdl",
			} );

				ModelEntity shoes;

				shoes = new ModelEntity();
				shoes.SetModel( model );
				shoes.SetParent( this, true );
				shoes.RenderColor = Color32.Red;
				shoes.EnableShadowInFirstPerson = true;
				shoes.EnableHideInFirstPerson = true;
				SetBodyGroup( "Feet", 1 );
			
			
			// Player
				



			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new SonicMovement();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new CustomCamera();



			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			
			//RenderColor = Color32.Blue;


			
			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>

		private Sound boostSound;
		private Particles boostParticle;

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );

			//
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//

			//
			


			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var ragdoll = new ModelEntity();
				ragdoll.SetModel( "models/citizen_props/hotdog01.vmdl_c" );
				ragdoll.Position = EyePos + EyeRot.Forward * 100;
				ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				ragdoll.Scale = 1;
				ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, true );
				ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
				ragdoll.DeleteAsync( 20 );

			}

			


			if ( Input.Pressed( InputButton.Jump ))
			{
				Sound.FromEntity( "jump", this );


			}

			if ( Input.Pressed( InputButton.Back ))
			{
				Sound.FromEntity( "brake", this );
			}

			
			if ( Input.Pressed( InputButton.Run ) )
			{
				boostSound = Sound.FromEntity( "boost", this );
				boostParticle = Particles.Create( "particles/boost.vpcf", this, "root_IK" );

			} else if ( Input.Released (InputButton.Run))
			{
				boostSound.Stop();
				boostParticle?.Destroy();
			}

			
			
		

		}			

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
