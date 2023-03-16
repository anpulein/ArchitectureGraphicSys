using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Assimp;
using Data = lib.Data.Data;
using Meshes = lib.Data.Data.Meshes;
using TextureUnit = OpenTK.Graphics.OpenGL4.TextureUnit;


namespace ArchitectureGraphicSys
{
    public class Mesh
    {
        public struct Vertex
        {
            public Vector3 Vertices;
            public Vector2 Coord;
            public Vector3 Normal;
        }

        private string _pathTexture;

        private readonly List<Texture> _textures = new List<Texture>();
        private readonly List<int> _materialTextureIndex = new List<int>();

        private readonly List<Tuple<Vertex[], int[]>> _completeScene = new List<Tuple<Vertex[], int[]>>();
        private readonly List<Tuple<uint, uint>> _bufferList = new List<Tuple<uint, uint>>();
        private int _sizeVertex;
        private int _lenVertex;


        public void InitGlBuffer(Vertex[] vertices, int[] indices)
        {
            uint vertexBuffer;
            uint indexBuffer;

            _lenVertex = vertices.Length;
            _sizeVertex = System.Runtime.InteropServices.Marshal.SizeOf<Vertex>();

            GL.GenBuffers(1, out vertexBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * _sizeVertex,  vertices, BufferUsageHint.StaticDraw);
            
            GL.GenBuffers(1, out indexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int),  indices, BufferUsageHint.StaticDraw);
            
            _bufferList.Add(new Tuple<uint, uint>(vertexBuffer, indexBuffer));
        }

        public Meshes LoadMesh(Meshes meshes)
        {
            _pathTexture = GetTextures(meshes);
            
            var filename = GetMeshes(meshes);
            Console.WriteLine("Загрузка mesh");

            var assimpImporter = new AssimpContext();
            var scene = assimpImporter.ImportFile(filename,
                PostProcessSteps.Triangulate |
                PostProcessSteps.GenerateSmoothNormals |
                PostProcessSteps.FlipUVs |
                PostProcessSteps.JoinIdenticalVertices);

            InitMaterials(scene);

            var allMeshesScene = scene.Meshes;
            foreach (var mesh in allMeshesScene)
            {
                InitMesh(mesh);
            }

            return meshes;
        }

        private void InitMesh(Assimp.Mesh mesh)
        {
            _materialTextureIndex.Add(mesh.MaterialIndex);
            
            var vertices = new Vertex[mesh.Vertices.Count];
            var indices = new int[mesh.FaceCount * 3];

            var meshVertices = mesh.Vertices;
            var normals = mesh.Normals;
            var coords = mesh.TextureCoordinateChannels;
            var faces = mesh.Faces;

            for (int i = 0; i < meshVertices.Count; i++)
            {
                var coord = new Vector2(0f, 0f);
                var vertex = new Vector3(meshVertices[i].X, meshVertices[i].Y, meshVertices[i].Z);
                var normal = new Vector3(normals[i].X, normals[i].Y, normals[i].Z);
                if (coords[0].Count > 1)
                {
                    coord = new Vector2(coords[0][i].X, coords[0][i].Y);
                }

                var compiledVertex = new Vertex
                {
                    Vertices = vertex,
                    Normal = normal,
                    Coord = coord
                };

                vertices[i] = compiledVertex;
            }

            var count = 0;

            foreach (var face in faces)
            {
                indices[count] = face.Indices[0];
                indices[++count] = face.Indices[1];
                indices[++count] = face.Indices[2];
                ++count;
            }
            
            _completeScene.Add(new Tuple<Vertex[], int[]>(vertices, indices));
            
            InitGlBuffer(vertices, indices);
        }
        
        private void InitMaterials(Scene scene)
        {
            for (var i = 0; i < scene.MaterialCount; i++)
            {
                var material = scene.Materials[i];
        
                if (material.GetMaterialTextureCount(TextureType.Diffuse) > 0)
                {
                    TextureSlot foundTexture;
                    if (material.GetMaterialTexture(TextureType.Diffuse, 0, out foundTexture))
                    {
                        _textures.Add(new Texture(TextureTarget.Texture2D, foundTexture.FilePath));
                        if (!_textures[i].Load())
                        {
                            Console.WriteLine("Error Loading texture!");
                        }
                    }
                }
                else
                {
                    // FBO -> No textures
                     _textures.Add(new Texture(TextureTarget.Texture2D, _pathTexture));
                    _textures[i].Load();
                }
            }
        }

        public void Render()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            

            for (int i = 0; i < _completeScene.Count; i++)
            {
                var vertexBuffer = _bufferList[i].Item1;
                var indexBuffer = _bufferList[i].Item2;
                var indicesCount = _completeScene[i].Item2.Length;
                
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, _sizeVertex, IntPtr.Zero);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, _sizeVertex, (IntPtr)12);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, _sizeVertex, (IntPtr)20);
                
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
                if(_materialTextureIndex.Count >= 1)
                    _textures[_materialTextureIndex[i]].Bind(TextureUnit.Texture0);

                GL.DrawElements(BeginMode.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            }

            // clean
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
        }
        
        private string GetMeshes(Meshes mesh) => mesh switch
        {
            Meshes.HOUES_2 => String.Concat(Data.LOCAL_PATH_MESHES, "buildings\\house_2.obj"),
            Meshes.BIG_TREE => String.Concat(Data.LOCAL_PATH_MESHES, "natures\\big_tree.obj"),
            Meshes.POLICE_CAR => String.Concat(Data.LOCAL_PATH_MESHES, "vehicles\\police_car.obj"),
            Meshes.JEEP => String.Concat(Data.LOCAL_PATH_MESHES, "vehicles\\jeep.obj"),
            _ => String.Concat(Data.LOCAL_PATH_MESHES, "box.obj")
        };
        
        private string GetTextures(Meshes mesh) => mesh switch
        {
            Meshes.HOUES_2 => String.Concat(Data.LOCAL_PATH_TEXTURES, "buildings\\house_2_orange.png"),
            Meshes.BIG_TREE => String.Concat(Data.LOCAL_PATH_TEXTURES, "natures\\nature.png"),
            Meshes.POLICE_CAR => String.Concat(Data.LOCAL_PATH_TEXTURES, "vehicles\\police_car.png"),
            Meshes.JEEP => String.Concat(Data.LOCAL_PATH_TEXTURES, "vehicles\\jeep_green.png"),
        };
    }
}