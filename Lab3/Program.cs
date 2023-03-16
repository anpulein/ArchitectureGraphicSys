using System;
using System.Collections.Generic;
using ArchitectureGraphicSys;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenGl = OpenTK.Graphics.OpenGL4;
using Data = lib.Data;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting process...");
            using (GraphicObjects graphicObject = new GraphicObjects($"Lab3"))
            {
                Data.Data data = new Data.Data(3);
                graphicObject.Run();
            }
        }
    }
}