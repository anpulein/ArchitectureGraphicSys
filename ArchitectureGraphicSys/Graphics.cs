using System.Collections.Generic;
using Assimp;
using lib.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchitectureGraphicSys
{
    public class Graphics
    {
        private Vector3 _position;
        private Vector4 _color;
        private Shader _shader;
        private float _rotation;
        private Mesh _mesh;
        private Matrix4 _modelMatrix;
        
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                _modelMatrix = recalculateModelMatrix(value);
            }
        }
        public Shader Shader
        {
            get => _shader;
            set => _shader = value;
        }

        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(value), out _modelMatrix);
                _modelMatrix.M41 = _position.X;
                _modelMatrix.M42 = _position.Y;
                _modelMatrix.M43 = _position.Z;

            }
        }

        public Mesh Mesh
        {
            get => _mesh;
            set => _mesh = value;
        }

        public Matrix4 ModelMatrix
        {
            get => _modelMatrix;
            set => _modelMatrix = value;
        }

        public Graphics () {}

        public Graphics(Vector3 position, Vector4 color, float rotation)
        {
            _modelMatrix = Matrix4.Identity;
            _position = position;
            _color = color;
            _rotation = rotation;
        }
        public Graphics(Vector3 position, Vector4 color)
        {
            _modelMatrix = Matrix4.Identity;
            _position = position;
            _color = color;
            _rotation = 0;
        }
        
        public Graphics(Vector3 position, Data.Colors color, float rotation)
        {
            _modelMatrix = Matrix4.Identity;
            _position = position;
            _color = new Vector4(GraphicObjectsMain.GetColors(color), 1.0f);
            _rotation = rotation;
        }
        public Graphics(Vector3 position, Data.Colors color)
        {
            _modelMatrix = Matrix4.Identity;
            _position = position;
            _color = new Vector4(GraphicObjectsMain.GetColors(color), 1.0f);
            _rotation = 0;
        }

        public Vector4 getColor() => _color;

        public void setColor(Data.Colors color)
        {
            _color = new Vector4(GraphicObjectsMain.GetColors(color), 1.0f);
        }
        public void setColor(Vector4 color) => _color = color;


        private static Matrix4 recalculateModelMatrix(Vector3 position)
        {
            Matrix4 mat4 = Matrix4.Identity;
            mat4.M41 = position.X;
            mat4.M42 = position.Y;
            mat4.M43 = position.Z;

            return mat4;
        }
    }
}