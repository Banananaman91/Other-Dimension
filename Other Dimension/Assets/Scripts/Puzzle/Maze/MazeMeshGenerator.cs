using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Maze
{
    public class MazeMeshGenerator
    {
        public float Width;
        public float Height;

        public MazeMeshGenerator(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Mesh FromData(int[,] data) // create quad
        {
            Mesh maze = new Mesh();
            
            List<Vector3> newVertices = new List<Vector3>();
            List<Vector2> newUVs = new List<Vector2>();

            maze.subMeshCount = 2;
            List<int> floorTriangles = new List<int>();
            List<int> wallTriangles = new List<int>();

            int rMax = data.GetUpperBound(0);
            int cMax = data.GetUpperBound(1);
            float halfH = Height * 0.5f;

            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    if (data[i, j] != 1)
                    {
                        AddQuad(Matrix4x4.TRS(new Vector3(j * Width, 0, i * Width), Quaternion.LookRotation(Vector3.up), new Vector3(Width, Width, 1)), ref newVertices, ref newUVs, ref floorTriangles);
                        //AddQuad(Matrix4x4.TRS(new Vector3(j * Width, Height, i * Width), Quaternion.LookRotation(Vector3.down), new Vector3(Width, Width, 1)), ref newVertices, ref newUVs, ref floorTriangles);
                        
                        if (i == 0 || j == 0 || i == rMax || j == cMax) continue;

                        if (i - 1 < 0 || data[i - 1, j] == 1)
                        {
                            AddQuad(Matrix4x4.TRS(new Vector3(j * Width, halfH, (i - 0.5f) * Width), Quaternion.LookRotation(Vector3.forward), new Vector3(Width, Height, 1)), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (j + 1 > cMax || data[i, j + 1] == 1)
                        {
                            AddQuad(Matrix4x4.TRS(new Vector3((j + 0.5f) * Width, halfH, i * Width), Quaternion.LookRotation(Vector3.left), new Vector3(Width, Height, 1)), ref newVertices, ref newUVs, ref wallTriangles);
                        }
                        
                        if (j - 1 < 0 || data[i, j - 1] == 1)
                        {
                            AddQuad(Matrix4x4.TRS(new Vector3((j - 0.5f) * Width, halfH, i * Width), Quaternion.LookRotation(Vector3.right), new Vector3(Width, Height, 1)), ref newVertices, ref newUVs, ref wallTriangles);
                        }

                        if (i + 1 > rMax || data[i + 1, j] == 1)
                        {
                            AddQuad(Matrix4x4.TRS(new Vector3(j * Width, halfH, (i + 0.5f) * Width), Quaternion.LookRotation(Vector3.back), new Vector3(Width, Height, 1)), ref newVertices, ref newUVs, ref wallTriangles);
                        }
                    }
                }
            }


            maze.vertices = newVertices.ToArray();
            maze.uv = newUVs.ToArray();
            maze.SetTriangles(floorTriangles.ToArray(), 0);
            maze.SetTriangles(wallTriangles.ToArray(), 1);
            
            maze.RecalculateNormals();

            return maze;
        }

        private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices, ref List<Vector2> newUVs,
            ref List<int> newTriangles)
        {
            int index = newVertices.Count;
            
            Vector3 vert1 = new Vector3(-0.5f, -0.5f, 0);
            Vector3 vert2 = new Vector3(-0.5f, 0.5f, 0);
            Vector3 vert3 = new Vector3(0.5f, 0.5f, 0);
            Vector3 vert4 = new Vector3(0.5f, -0.5f, 0);
            
            newVertices.Add(matrix.MultiplyPoint3x4(vert1));
            newVertices.Add(matrix.MultiplyPoint3x4(vert2));
            newVertices.Add(matrix.MultiplyPoint3x4(vert3));
            newVertices.Add(matrix.MultiplyPoint3x4(vert4));
            
            newUVs.Add(new Vector2(1, 0));
            newUVs.Add(new Vector2(1, 1));
            newUVs.Add(new Vector2(0, 1));
            newUVs.Add(new Vector2(0 ,0));
            
            newTriangles.Add(index+2);
            newTriangles.Add(index+1);
            newTriangles.Add(index);
            
            newTriangles.Add(index+3);
            newTriangles.Add(index+2);
            newTriangles.Add(index);
        }
    }
}
