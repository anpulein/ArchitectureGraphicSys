using System;
using OpenTK.Mathematics;

namespace ArchitectureGraphicSys
{
    public class Camera
    {

        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;

        private float _pitch;

        private float _yaw = -MathHelper.PiOver2;

        private float _fow = MathHelper.PiOver2;
        
        public Vector3 Position { get; set; }
        public float AspectRatio { private get; set; }

        public Vector3 Front => _front;
        public Vector3 Up => _up;
        public Vector3 Right => _right;

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -11f, 11f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors(); 
            }
        }

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fow);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 89f);
                _fow = MathHelper.DegreesToRadians(angle);
            }
        }
        
        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }

        public Matrix4 GetViewMatrix() => Matrix4.LookAt(Position, Position + _front, _up);

        public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(_fow, AspectRatio, 0.01f, 100f);

        public void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}