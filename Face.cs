using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace RubiksCube
{
    public class Face
    {
        private int dimension;
        private int faceNumber;
        private float faceSize;
        private float tileSize;
        private Color initialColor;
        private Tile[,] tiles;
        public Vector3 direction { get; private set; }
        public Vector2 direction2D { get; set; }
        private Vector3 tileShape;
        private Vector3 iDirection;
        private Vector3 jDirection;
        public Dictionary<Vector3, int> directionIndex { get; private set; }
        public int[] indexOrder { get; private set; }

        public Face(int dim, int faceNumber, float faceSize = 10.0f, float sRatio = 0.8f, float dRatio = 0.1f)
        {
            this.dimension = dim;
            this.faceSize = faceSize;
            this.tileSize = faceSize / (float)this.dimension;
            this.faceNumber = faceNumber;

            // Determine face direction
            switch (faceNumber)
            {
                case 0: // Front
                    this.direction = new Vector3(0.0f, 0.0f, 1.0f);
                    this.direction2D = new Vector2(1.0f, -1.0f);
                    this.initialColor = RED;
                    this.iDirection = new Vector3(1.0f, 0.0f, 0.0f);
                    this.jDirection = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case 1: // Back
                    this.direction = new Vector3(0.0f, 0.0f, -1.0f);
                    this.direction2D = new Vector2(1.0f, -1.0f);
                    this.initialColor = WHITE;
                    this.iDirection = new Vector3(-1.0f, 0.0f, 0.0f);
                    this.jDirection = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case 2: // Right
                    this.direction = new Vector3(1.0f, 0.0f, 0.0f);
                    this.direction2D = new Vector2(1.0f, -1.0f);
                    this.initialColor = BLUE;
                    this.iDirection = new Vector3(0.0f, 0.0f, -1.0f);
                    this.jDirection = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case 3: // Left
                    this.direction = new Vector3(-1.0f, 0.0f, 0.0f);
                    this.direction2D = new Vector2(1.0f, -1.0f);
                    this.initialColor = ORANGE;
                    this.iDirection = new Vector3(0.0f, 0.0f, 1.0f);
                    this.jDirection = new Vector3(0.0f, 1.0f, 0.0f);
                    break;
                case 4: // Top
                    this.direction = new Vector3(0.0f, 1.0f, 0.0f);
                    this.direction2D = new Vector2(1.0f, 1.0f);
                    this.initialColor = GREEN;
                    this.iDirection = new Vector3(0.0f, 0.0f, 1.0f);
                    this.jDirection = new Vector3(1.0f, 0.0f, 0.0f);
                    break;
                case 5: // Bottom
                    this.direction = new Vector3(0.0f, -1.0f, 0.0f);
                    this.direction2D = new Vector2(1.0f, 1.0f);
                    this.initialColor = YELLOW;
                    break;
                default: // Should never happen
                    this.direction = new Vector3(0.0f, 0.0f, 0.0f);
                    this.direction2D = new Vector2(1.0f, 1.0f);
                    this.initialColor = BLACK;
                    this.iDirection = new Vector3(0.0f, 0.0f, -1.0f);
                    this.jDirection = new Vector3(1.0f, 0.0f, 0.0f);
                    break;
            }

            // Use face direction to determine tile shape ratios
            this.tileShape = new Vector3();
            this.tileShape.X = (Math.Abs(this.direction.X) == 1.0f) ? dRatio * this.direction.X : sRatio;
            this.tileShape.Y = (Math.Abs(this.direction.Y) == 1.0f) ? dRatio * this.direction.Y : sRatio;
            this.tileShape.Z = (Math.Abs(this.direction.Z) == 1.0f) ? dRatio * this.direction.Z : sRatio;

            this.tiles = new Tile[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Vector3 position = GetBotLeft() + (i * this.iDirection + j * this.jDirection) * this.tileSize;
                    this.tiles[i, j] = new Tile(
                            position, // Position
                            this.tileSize, // Size
                            this.tileShape, // ShapeRatio
                            this.initialColor); // Color
                }
            }

            this.directionIndex = new Dictionary<Vector3, int>();
            this.indexOrder = new int[4];
        }

        public void Draw()
        {
            for (int i = 0; i < this.dimension; i++)
            {
                for (int j = 0; j < this.dimension; j++)
                {
                    this.tiles[i, j].Draw();
                }
            }
        }

        public void Draw2D(Vector2 pos, float size, bool wires = false)
        {
            Vector2 initPos = pos - (this.direction2D * size / this.dimension);
            DrawRectangle((int)(pos.X - 0.7f * size / 2.0f),
                    (int)(pos.Y - 0.7f * size / 2.0f),
                    (int)size,
                    (int)size,
                    BLACK);

            for (int i = 0; i < this.dimension; i++)
            {
                for (int j = 0; j < this.dimension; j++)
                {
                    if (this.faceNumber < 4)
                    {
                        Vector2 tilePos = initPos + (this.direction2D * new Vector2(i, j)) * size / this.dimension;
                        this.tiles[i, j].Draw2D(tilePos,
                                size / this.dimension,
                                wires);
                    }
                    else
                    {
                        Vector2 tilePos = initPos + (this.direction2D * new Vector2(j, i)) * size / this.dimension;
                        this.tiles[i, j].Draw2D(tilePos,
                                size / this.dimension,
                                wires);
                    }
                }
            }
        }

        public Vector3 GetBotLeft()
        {
            // Center of face
            Vector3 botLeft = this.direction * this.faceSize / 2.0f;

            // Move from center to bottom left edge
            botLeft -= this.iDirection * this.faceSize / 2.0f;
            botLeft -= this.jDirection * this.faceSize / 2.0f;

            // Move to center of the tile at bottom left
            botLeft += this.iDirection * this.tileSize / 2.0f;
            botLeft += this.jDirection * this.tileSize / 2.0f;

            return botLeft;
        }

        public Color this[int i, int j]
        {
            get { return this.tiles[i, j].color; }
            set { this.tiles[i, j].color = value; }
        }

        public Tile[] GetColumn(int j, bool reversed = false)
        {
            Tile[] column = new Tile[this.dimension];
            for (int i = 0; i < this.dimension; i++)
            {
                column[i] = this.tiles[i, j];
            }
            if (reversed)
            {
                Array.Reverse(column);
            }
            return column;
        }

        public Tile[] GetRow(int i, bool reversed = false)
        {
            Tile[] row = new Tile[this.dimension];
            for (int j = 0; j < this.dimension; j++)
            {
                row[j] = this.tiles[i, j];
            }
            if (reversed)
            {
                Array.Reverse(row);
            }
            return row;
        }

        public Tile[] GetEdge(Vector3 dir, int num, bool reversed = false)
        {
            if (!this.directionIndex.ContainsKey(dir)) { return null; }
            switch(this.directionIndex[dir])
            {
                case 0:
                    return GetColumn(num, reversed);
                case 1:
                    return GetRow(num, reversed);
                case 2:
                    return GetColumn(this.dimension - num - 1, !reversed);
                case 3:
                    return GetRow(this.dimension - num - 1, !reversed);
            }
            return null;
        }

        public void AddIndex(Vector3 dir, int num)
        {
            this.directionIndex.Add(dir, num);
        }

        public void SetEdgeOrder(int n1, int n2, int n3, int n4)
        {
            this.indexOrder[0] = n1;
            this.indexOrder[1] = n2;
            this.indexOrder[2] = n3;
            this.indexOrder[3] = n4;
        }
    }

    public class Tile
    {
        private Vector3 position;
        private Vector3 shape;
        public Color color { get; set; }

        public Tile(Vector3 position, float size, Vector3 shapeRatio, Color color)
        {
            this.position = position;
            this.shape = shapeRatio * size;
            this.color = color;
        }

        public void Draw()
        {
            DrawCubeV(this.position, // Position
                this.shape, // Shape
                this.color); // Color
            DrawCubeWiresV(this.position, // Position
                this.shape * 1.1f, // Shape
                BLACK); // Color
        }

        public void Draw2D(Vector2 pos, float size, bool wires = false, bool extra = false)
        {
            DrawRectangle((int)pos.X, (int)pos.Y, (int)(size * 0.9f), (int)(size * 0.9f), this.color);
            if (wires)
            {
                DrawRectangleLines((int)pos.X, (int)pos.Y, (int)size, (int)size, GRAY);
            }
            if (extra)
            {
                DrawRectangle((int)pos.X, (int)pos.Y, (int)(size * 0.9f), (int)(size * 0.9f), BLACK);
            }
        }
    }
}
