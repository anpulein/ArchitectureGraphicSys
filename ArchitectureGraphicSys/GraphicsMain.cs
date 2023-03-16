using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GL = OpenTK.Graphics.OpenGL4.GL;
using Data = lib.Data;

namespace ArchitectureGraphicSys
{
    public class GraphicsMain : GameWindow
    {
        protected int _vertexBufferObject;
        protected int _vertexArrayObject;
        protected Shader _shader;
        protected Shader _light;
        protected Camera _camera;

        protected int Width;
        protected int Height;
        
        private float frameTime;
        private int fps;
        
        // Matrix projection
        protected Matrix4 projectionMatrix;
        // Matrix model
        protected Matrix4 modelMatrix;
        // Matrix view
        protected Matrix4 modelViewMatrix;

        protected double _time;
        protected bool _firstMove = true;
        protected Vector2 _lastPos;


        /// <summary>
        /// Setting the window size and title window
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        /// <param name="title"></param>
        public GraphicsMain(int weight, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (weight, height), Title = title })
        {
            VSync = VSyncMode.On;
        }

        /// <summary>
        /// Setting the title window
        /// </summary>
        /// <param name="title"></param>
        public GraphicsMain(string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (800, 600), Title = title })
        {
            VSync = VSyncMode.On;
        }


        /// <summary>
        /// Getting fps the current window
        /// </summary>
        /// <param name="time"></param>
        protected void GetFPS(float time)
        {
            frameTime += time;
            fps++;

            if (frameTime >= 1.0f)
            {
                Title = $"Lab{Data.Data.NUMBER_LAB.ToString()}, FPS: {fps.ToString()}";
                frameTime = 0.0f;
                fps = 0;
            }
        }


        /// <summary>
        /// Update Frame
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            // Code
        }

        /// <summary>
        /// Starting at the very beginning of the program
        /// Init code
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            // Code
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            // Code
        }

        /// <summary>
        /// Editing size window app
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            // Code
        }

        protected override void OnUnload()
        {
            // Code
            base.OnUnload();
        }
    }
}