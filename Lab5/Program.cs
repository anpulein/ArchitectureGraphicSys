using System;
using System.Collections.Generic;
using ArchitectureGraphicSys;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenGl = OpenTK.Graphics.OpenGL4;
using Data = lib.Data;
namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting process...");
            Data.Data data = new Data.Data(5);
            using (GraphicObjects graphicObject = new GraphicObjects($"Lab5"))
            {
                graphicObject.Run();
            }
        }
    }
}