using System;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenGl = OpenTK.Graphics.OpenGL4;
using Data = lib.Data;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting process...");
            using (Graphics graphics = new Graphics($"Lab2"))
            {
                Data.Data data = new Data.Data(2);
                graphics.Run();
            }
        }
    }
}