using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GL = OpenTK.Graphics.OpenGL4.GL;

using ArchitectureGraphicSys;
using lib.Data;
using TextureUnit = OpenTK.Graphics.OpenGL.TextureUnit;


namespace Lab6
{
    public class GraphicObjects : GraphicObjectsMain
    {
        private readonly float[] vertices = new float[]
        {
            // front side (two triangles)
            -0.5f, +0.5f, +0.5f, -0.5f, -0.5f, +0.5f, +0.5f, +0.5f, +0.5f,
            +0.5f, +0.5f, +0.5f, -0.5f, -0.5f, +0.5f, +0.5f, -0.5f, +0.5f,
            // back side (two triangles)
            +0.5f, +0.5f, -0.5f, +0.5f, -0.5f, -0.5f, -0.5f, +0.5f, -0.5f,
            -0.5f, +0.5f,	-0.5f, +0.5f, -0.5f, -0.5f, -0.5f, -0.5f, -0.5f,
            // right side (two triangles) 
            +0.5f, -0.5f, +0.5f, +0.5f, -0.5f, -0.5f, +0.5f, +0.5f, +0.5f,
            +0.5f, +0.5f, +0.5f, +0.5f, -0.5f, -0.5f, +0.5f, +0.5f, -0.5f,
            // left side (two triangles)
            -0.5f, +0.5f, +0.5f, -0.5f, +0.5f, -0.5f, -0.5f, -0.5f, +0.5f,
            -0.5f, -0.5f,	+0.5f, -0.5f, +0.5f, -0.5f, -0.5f, -0.5f, -0.5f,
            // upper side (two triangles)
            -0.5f, +0.5f, -0.5f, -0.5f, +0.5f, +0.5f, +0.5f, +0.5f, -0.5f,
            +0.5f, +0.5f, -0.5f, -0.5f, +0.5f, +0.5f, +0.5f, +0.5f, +0.5f,
            // lower side (two triangles)
            -0.5f, -0.5f, +0.5f, -0.5f, -0.5f, -0.5f, +0.5f, -0.5f, +0.5f,
            +0.5f, -0.5f, +0.5f, -0.5f, -0.5f, -0.5f, +0.5f, -0.5f, -0.5f
        };

        private readonly Vector3 _lightPos = new Vector3(0.0f, 100.0f, 0.0f);
        
        public GraphicObjects(int weight, int height, string title) : base(weight, height, title) { Init(); }

        public GraphicObjects(string title) : base(title) { Init(); }
        
        
        /// <summary>
        /// Update Frame
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            GetFPS((float)e.Time);
            
            _time += 0.05f * e.Time;
            
            base.OnUpdateFrame(e);
            
            if (!IsFocused) return;
            
            // Esc - Exit current program
            KeyboardState input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape)) Close();

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W)) _camera.Position += _camera.Front * cameraSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.S)) _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.A)) _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.D)) _camera.Position += _camera.Right * cameraSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.Space)) _camera.Position += _camera.Up * cameraSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.LeftShift)) _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time;

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;

                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }
        
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }
        
        /// <summary>
        /// Starting at the very beginning of the program
        /// Init code
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(1f, 1f, 1f, 1.0f);
            
            // GL.Enable(EnableCap.Texture2D);
            //Basically enables the alpha channel to be used in the color buffer
            // GL.Enable(EnableCap.Blend);
            // //The operation/order to blend
            // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //Use for pixel depth comparing before storing in the depth buffer
            GL.Enable(EnableCap.DepthTest);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            _shader = new Shader(Data.LOCAL_PATH_SHADER_VERTICAL, Data.LOCAL_PATH_SHADER_FRAGMENT);
            _shader.Use();

            // var vertexLocation = _shader.GetAttribLocation("vPosition");
            // GL.EnableVertexAttribArray(vertexLocation);
            // GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //
            // // Next, we also setup texture coordinates. It works in much the same way.
            // // We add an offset of 3, since the texture coordinates comes after the position data.
            // // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            // var texCoordLocation = _shader.GetAttribLocation("vTexCoord");
            // GL.EnableVertexAttribArray(texCoordLocation);
            // GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorState = CursorState.Grabbed;
        }
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(_vertexArrayObject);

            var viewMatrix = _camera.GetViewMatrix();
            _shader.Use();
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            _shader.SetVector3("lPosition", _lightPos);
            _shader.SetVector4("lAmbient", new Vector4(1f, 1f, 1f, 1.0f));
            _shader.SetVector4("lDiffuse", new Vector4(1f, 1f, 1f, 1.0f));
            _shader.SetVector4("lSpecular", new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            

            foreach (var gr in _graphics)
            {
                _shader.SetMatrix4("modelViewMatrix", gr.ModelMatrix * viewMatrix);
                
                _shader.SetVector4("mAmbient", gr.Material.GetAmbient());
                _shader.SetVector4("mDiffuse", gr.Material.GetDiffuse());
                _shader.SetVector4("mSpecular", gr.Material.GetSpecular());
                _shader.SetFloat("mShininess", gr.Material.GetShininess());
                
                gr.Mesh?.Render();
            }
            
            SwapBuffers();
        }
        
        
        /// <summary>
        /// Editing size window app
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, e.Width, e.Height);

            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnUnload()
        {
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            
            
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_vertexBufferObject);

            
            GL.DeleteProgram(_shader.Handle);

                base.OnUnload();
        }

        private void Init()
        {
            LoadMeshPath(Data.LOCAL_PATH_JSON_MODELS);
            ParseJsonObjects(Data.LOCAL_PATH_JSON_SCENES + "demo_scene.json");
        }
    }
}