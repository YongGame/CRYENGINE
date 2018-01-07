// Copyright 2001-2016 Crytek GmbH / Crytek Group. All rights reserved.

using CryEngine.Common;
using CryEngine.Rendering;

namespace CryEngine
{
	/// <summary>
	/// Static class exposing access to the current view camera
	/// </summary>
	public static class Camera
	{
		/// <summary>
		/// Set or get the position of the current view camera.
		/// </summary>
		public static Vector3 Position
		{
			get
			{
				return Engine.System.GetViewCamera().GetPosition();
			}
			set
			{
				Engine.System.GetViewCamera().SetPosition(value);
			}
		}

		/// <summary>
		/// Get or set the facing direction of the current view camera.
		/// </summary>
		public static Vector3 ForwardDirection
		{
			get
			{
				return Engine.System.GetViewCamera().GetMatrix().GetColumn1();
			}
			set
			{
				var camera = Engine.System.GetViewCamera();
				var newRotation = new Quaternion(value);

				camera.SetMatrix(new Matrix3x4(Vector3.One, newRotation, camera.GetPosition()));
			}
		}

		/// <summary>
		/// Get or set the transformation matrix of the current view camera.
		/// </summary>
		public static Matrix3x4 Transform
		{
			get
			{
				return Engine.System.GetViewCamera().GetMatrix();
			}
			set
			{
				Engine.System.GetViewCamera().SetMatrix(value);
			}
		}

		/// <summary>
		/// Get or set the rotation of the current view camera
		/// </summary>
		public static Quaternion Rotation
		{
			get
			{
				return new Quaternion(Engine.System.GetViewCamera().GetMatrix());
			}
			set
			{
				var camera = Engine.System.GetViewCamera();

				camera.SetMatrix(new Matrix3x4(Vector3.One, value, camera.GetPosition()));
			}
		}

		/// <summary>
		/// Gets or sets the field of view of the view camera in degrees.
		/// </summary>
		/// <value>The field of view.</value>
		public static float FieldOfView
		{
			get
			{
				return MathHelpers.RadiansToDegrees(Engine.System.GetViewCamera().GetFov());
			}
			set
			{
				var camera = Engine.System.GetViewCamera();

				camera.SetFrustum(Global.gEnv.pRenderer.GetWidth(), Global.gEnv.pRenderer.GetHeight(), MathHelpers.DegreesToRadians(value));
			}
		}

		/// <summary>
		/// Converts a screenpoint from screen-space to world-space.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Vector3 Unproject(int x, int y)
		{
			return Global.gEnv.pRenderer.UnprojectFromScreen(x, Renderer.ScreenHeight - y);
		}

		/// <summary>
		/// Converts a point in world-space to the camera's screen-space.
		/// </summary>
		/// <returns><c>true</c>, if the point is visible, <c>false</c> otherwise.</returns>
		/// <param name="position">Position of the point in world-space.</param>
		/// <param name="screenPosition">Position of the point in the camera's screen-space.</param>
		public static bool ProjectToScreen(Vector3 position, out Vector3 screenPosition)
		{
			var camera = Global.gEnv.pSystem.GetViewCamera();
			Vec3 result = new Vec3();
			var visible = camera.Project(position, result);
			screenPosition = result;
			return visible;
		}

		/// <summary>
		/// Converts a point in world-space to the camera's viewport-space.
		/// </summary>
		/// <returns><c>true</c>, if the point is visible, <c>false</c> otherwise.</returns>
		/// <param name="position">Position of the point in world-space.</param>
		/// <param name="viewportPosition">Position of the point in the camera's viewport-space.</param>
		public static bool ProjectToViewport(Vector3 position, out Vector3 viewportPosition)
		{
			var camera = Global.gEnv.pSystem.GetViewCamera();
			Vec3 result = new Vec3();
			var visible = camera.Project(position, result);
			viewportPosition = result;
			viewportPosition.x /= camera.GetViewSurfaceX();
			viewportPosition.y /= camera.GetViewSurfaceZ();
			return visible;
		}

		/// <summary>
		/// Transforms a direction from world space to local space of the camera.
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 TransformDirection(Vector3 direction)
		{
			return Rotation * direction;
		}
	}
}
