using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using lib.Data;
using Newtonsoft.Json;
using Colors = lib.Data.Data.Colors;
using Meshes = lib.Data.Data.Meshes;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ArchitectureGraphicSys
{
    public class GraphicObjectsMain : GraphicsMain
    {
        protected List<Graphics> _graphics;
        protected Dictionary<Meshes, Mesh> _meshes;

        public GraphicObjectsMain(int weight, int height, string title) : base(weight, height, title)
        {
            _graphics = new List<Graphics>();
            _meshes = new Dictionary<Meshes, Mesh>();
        }

        public GraphicObjectsMain(string title) : base(title)
        {
            _graphics = new List<Graphics>();
            _meshes = new Dictionary<Meshes, Mesh>();
        }
        
        public static Vector3 GetColors(Colors color) => color switch
        {
            Colors.Black => new Vector3(0.0f, 0.0f, 0.0f),
            Colors.Red => new Vector3(1.0f, 0.0f, 0.0f),
            Colors.Green => new Vector3(0.0f, 1.0f, 0.0f),
            Colors.Blue => new Vector3(0.0f, 0.0f, 1.0f),
            Colors.White => new Vector3(1.0f, 1.0f, 1.0f),
            _ => new Vector3(0.0f, 0.0f, 0.0f)
        };
        
        public static Meshes GetMesh(string mesh) => mesh switch
        {
            "HOUSE_2" => Meshes.HOUES_2,
            "BIG_TREE" => Meshes.BIG_TREE,
            "POLICE_CAR" => Meshes.POLICE_CAR,
            "JEEP" => Meshes.JEEP,
            "BOX" => Meshes.BOX,
        };

        public void LoadMesh(List<Meshes> meshesList)
        {
            foreach (var mesh in meshesList)
            {
                if (_meshes.ContainsKey(mesh))
                {
                    Console.WriteLine("Данные Mesh уже загружен!");
                    continue;
                }

                var meshObject = new Mesh();
                meshObject.LoadMesh(mesh);
                _meshes.Add(mesh, meshObject);
            }
        }

        public void ParseJsonObjects(string path)
        {

            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<Root>>(json);

                foreach (var item in items)
                {
                    Graphics graphics = new Graphics();
                    graphics.setColor(new Vector4(item.color[0], item.color[1], item.color[2], item.color[3]));
                    graphics.Position = new Vector3(item.position[0], item.position[1], item.position[2]);
                    graphics.Rotation = item.rotation;
                    graphics.Mesh = _meshes[GetMesh(item.mesh)];
                    _graphics.Add(graphics);
                }
            }
            
        }
        
        public class Root
        {
            public List<float> color { get; set; }
            public List<float> position { get; set; }
            public float rotation { get; set; }
            public string mesh { get; set; }
        }
    }
}