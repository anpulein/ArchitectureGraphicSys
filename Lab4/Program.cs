using System;
using System.Collections.Generic;
using ArchitectureGraphicSys;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenGl = OpenTK.Graphics.OpenGL4;
using Data = lib.Data;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting process...");
            Data.Data data = new Data.Data(4);
            using (GraphicObjects graphicObject = new GraphicObjects($"Lab4"))
            {
                graphicObject.Run();
            }
        }
    }
}