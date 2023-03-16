using System;
using System.Collections.Generic;

namespace lib.Data
{
    public class Data
    {
        public static string LOCAL_PATH_SHADER_FRAGMENT;
        public static string LOCAL_PATH_SHADER_VERTICAL;
        public static string LOCAL_PATH_LIGHT_FRAGMENT;
        public static string LOCAL_PATH_MESHES;
        public static string LOCAL_PATH_TEXTURES;
        public static string LOCAL_PATH_JSON_OBJECTS;
        public static int NUMBER_LAB;

        public enum Colors
        {
            Red,
            Green,
            Blue,
            Black,
            White
        }
        
        public enum Meshes
        {
            HOUES_2,
            BIG_TREE,
            POLICE_CAR,
            JEEP, 
            BOX
        }

        public static readonly List<Meshes> MeshesList = new List<Meshes>() { Meshes.HOUES_2, Meshes.BIG_TREE, Meshes.POLICE_CAR, Meshes.JEEP };

        public Data(int numberLab)
        {
            string pathLocal = System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\";
            LOCAL_PATH_SHADER_FRAGMENT = $"{pathLocal}lib.Data\\Lab{numberLab.ToString()}\\shaders\\shader.frag";
            LOCAL_PATH_LIGHT_FRAGMENT = $"{pathLocal}lib.Data\\Lab{numberLab.ToString()}\\shaders\\light.frag";
            LOCAL_PATH_SHADER_VERTICAL = $"{pathLocal}lib.Data\\Lab{numberLab.ToString()}\\shaders\\shader.vert";
            LOCAL_PATH_MESHES = $"{pathLocal}lib.Data\\meshes\\";
            LOCAL_PATH_TEXTURES = $"{pathLocal}lib.Data\\textures\\";
            LOCAL_PATH_JSON_OBJECTS = $"{pathLocal}lib.Data\\Lab{numberLab.ToString()}\\Objects\\Objects.json";
            NUMBER_LAB = numberLab;
            
        }
    }
}