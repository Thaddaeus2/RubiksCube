using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace RubiksCube
{
    public class RubiksCube
    {
        public int dimension { get; private set; }
        private float cubeSize;
        private Face[] faces;

        public RubiksCube(int dim = 3, float size = 10.0f)
        {
            this.dimension = dim;
            this.cubeSize = size;

            this.faces = new Face[6];
            for (int i = 0; i < 6; i++)
            {
                this.faces[i] = new Face(dimension, i, cubeSize);
            }

            // Map directions of other faces, and order to rotate them

            // Front face
            this.faces[0].AddIndex(this.faces[4].direction, 2);
            this.faces[0].AddIndex(this.faces[2].direction, 3);
            this.faces[0].AddIndex(this.faces[5].direction, 0);
            this.faces[0].AddIndex(this.faces[3].direction, 1);
            this.faces[0].SetEdgeOrder(4, 2, 5, 3);

            // Back face
            this.faces[1].AddIndex(this.faces[4].direction, 2);
            this.faces[1].AddIndex(this.faces[3].direction, 3);
            this.faces[1].AddIndex(this.faces[5].direction, 0);
            this.faces[1].AddIndex(this.faces[2].direction, 1);
            this.faces[1].SetEdgeOrder(4, 3, 5, 2);

            // Right face
            this.faces[2].AddIndex(this.faces[5].direction, 0);
            this.faces[2].AddIndex(this.faces[0].direction, 1);
            this.faces[2].AddIndex(this.faces[4].direction, 2);
            this.faces[2].AddIndex(this.faces[1].direction, 3);
            this.faces[2].SetEdgeOrder(4, 1, 5, 0);

            // Left face
            this.faces[3].AddIndex(this.faces[5].direction, 0);
            this.faces[3].AddIndex(this.faces[1].direction, 1);
            this.faces[3].AddIndex(this.faces[4].direction, 2);
            this.faces[3].AddIndex(this.faces[0].direction, 3);
            this.faces[3].SetEdgeOrder(4, 0, 5, 1);

            // Top face
            this.faces[4].AddIndex(this.faces[2].direction, 2);
            this.faces[4].AddIndex(this.faces[1].direction, 1);
            this.faces[4].AddIndex(this.faces[3].direction, 0);
            this.faces[4].AddIndex(this.faces[0].direction, 3);
            this.faces[4].SetEdgeOrder(2, 1, 3, 0);

            // Bottom face
            this.faces[5].AddIndex(this.faces[2].direction, 2);
            this.faces[5].AddIndex(this.faces[0].direction, 1);
            this.faces[5].AddIndex(this.faces[3].direction, 0);
            this.faces[5].AddIndex(this.faces[1].direction, 3);
            this.faces[5].SetEdgeOrder(2, 0, 3, 1);
        }

        public void Draw()
        {
            DrawCubeV(new Vector3(0.0f, 0.0f, 0.0f), // Position
                    this.cubeSize * new Vector3(0.9f, 0.9f, 0.9f), // Size
                    BLACK); // Color

            for (int i = 0; i < 6; i++)
            {
                this.faces[i].Draw();
            }

            DrawCubeWiresV(new Vector3(0.0f, 0.0f, 0.0f), // Position
                    this.cubeSize * new Vector3(1.1f, 1.1f, 1.1f), // Size
                    BLACK); // Color
        }

        public void Draw2D(Vector2 pos, float size)
        {
            Vector2 facePos = new Vector2();
            for (int i = 0; i < 7; i++)
            {
                switch (i)
                {
                    case 0:
                        facePos = pos + new Vector2(0.0f, 0.0f);
                        break;
                    case 1:
                        facePos = pos + new Vector2(2.0f * size, 0.0f);
                        break;
                    case 2:
                        facePos = pos + new Vector2(1.0f * size, 0.0f);
                        break;
                    case 3:
                        facePos = pos + new Vector2(-1.0f * size, 0.0f);
                        break;
                    case 4:
                        facePos = pos + new Vector2(0.0f, -1.0f * size);
                        break;
                    case 5:
                        facePos = pos + new Vector2(0.0f, 1.0f * size);
                        break;
                    case 6:
                        facePos = pos + new Vector2(0.0f, -2.0f * size);
                        break;
                }
                if (i < 6)
                {
                    this.faces[i].Draw2D(facePos, size, (i == 0));
                }
                else
                {
                    this.faces[1].direction2D *= -1.0f;
                    this.faces[1].Draw2D(facePos, size, (i == 0));
                    this.faces[1].direction2D *= -1.0f;
                }
            }
        }

        public void Randomize()
        {
            var rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < this.dimension; j++)
                {
                    for (int k = 0; k < this.dimension; k++)
                    {
                        this.faces[i][j, k] = new Color(
                            rand.Next(0, 255),
                            rand.Next(0, 255),
                            rand.Next(0, 255),
                            255);
                    }
                }
            }
        }

        public void RotateFace(int face = 0, bool reversed = false)
        {
            if (!reversed)
            {
                for (int i = 0; i < this.dimension / 2; i++)
                {
                    for (int j = i; j < this.dimension - i - 1; j++)
                    {
                        Color temp = this.faces[face][i, j];
                        this.faces[face][i, j] = this.faces[face][j, this.dimension - i - 1];
                        this.faces[face][j, this.dimension - i - 1] = this.faces[face][this.dimension - i - 1, this.dimension - j - 1];
                        this.faces[face][this.dimension - i - 1, this.dimension - j - 1] = this.faces[face][this.dimension - j - 1, i];
                        this.faces[face][this.dimension - j - 1, i] = temp;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.dimension / 2; i++)
                {
                    for (int j = i; j < this.dimension - i - 1; j++)
                    {
                        Color temp = this.faces[face][i, j];
                        this.faces[face][i, j] = this.faces[face][this.dimension - j - 1, i];
                        this.faces[face][this.dimension - j - 1, i] = this.faces[face][this.dimension - i - 1, this.dimension - j - 1];
                        this.faces[face][this.dimension - i - 1, this.dimension - j - 1] = this.faces[face][j, this.dimension - i - 1];
                        this.faces[face][j, this.dimension - i - 1] = temp;
                    }
                }
            }
        }

        public void RotateEdge(int faceNum, int num, bool reversed = false)
        {
            // Check that the edge is valid
            if (num < 0 || num > this.dimension) { return; }

            Tile[][] edges = new Tile[4][];

            // Get edges from other faces
            for (int i = 0; i < 4; i++)
            {
                int edgeNum = this.faces[faceNum].indexOrder[i];
                bool arrayReverse = (faceNum == 2 || faceNum == 3) && (edgeNum == 4 || edgeNum == 5);
                edges[i] = this.faces[edgeNum].GetEdge(
                        this.faces[faceNum].direction, // Direction of face, used to determine row vs edge
                        num, // Which row or column of edge
                        arrayReverse); // Whether to reverse the edge array (when left or right request top or bottom)
            }

            // Rotate edges
            if (!reversed)
            {
                Color[] edgeCopy = new Color[this.dimension];
                for (int i = 0; i < this.dimension; i++)
                {
                    edgeCopy[i] = edges[0][i].color;
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < this.dimension; j++)
                    {
                        if (i != 3)
                        {
                            edges[i][j].color = edges[i + 1][j].color;
                        }
                        else
                        {
                            edges[i][j].color = edgeCopy[j];
                        }
                    }
                }
            }
            else
            {
                Color[] edgeCopy = new Color[this.dimension];
                for (int i = 0; i < this.dimension; i++)
                {
                    edgeCopy[i] = edges[3][i].color;
                }

                for (int i = 3; i >= 0; i--)
                {
                    for (int j = 0; j < this.dimension; j++)
                    {
                        if (i != 0)
                        {
                            edges[i][j].color = edges[i - 1][j].color;
                        }
                        else
                        {
                            edges[i][j].color = edgeCopy[j];
                        }
                    }
                }
            }
        }

        public void RotateCubeUp()
        {
            for (int i = 0; i < this.dimension; i++)
            {
                RotateEdge(2, i, true);
            }
            RotateFace(2, true);
            RotateFace(3, false);
        }

        public void RotateCubeDown()
        {
            for (int i = 0; i < this.dimension; i++)
            {
                RotateEdge(2, i, false);
            }
            RotateFace(2, false);
            RotateFace(3, true);
        }

        public void RotateCubeLeft()
        {
            for (int i = 0; i < this.dimension; i++)
            {
                RotateEdge(4, i, false);
            }
            RotateFace(4, true);
            RotateFace(5, false);
        }

        public void RotateCubeRight()
        {
            for (int i = 0; i < this.dimension; i++)
            {
                RotateEdge(4, i, true);
            }
            RotateFace(4, false);
            RotateFace(5, true);
        }
    }
}
