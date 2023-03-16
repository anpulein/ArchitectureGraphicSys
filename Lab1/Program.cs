using System;
using lib.Data;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenGl = OpenTK.Graphics.OpenGL4;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting process...");
            using (Graphics graphics = new Graphics($"Lab1, Version OpenGL: "))
            {
                Data data = new Data(1);
                graphics.Title += GL.GetString(OpenGl.StringName.Version);
                graphics.Run();
            }
        }
    }
}