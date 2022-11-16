using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraMode;
using static RubiksCube.KeyBindings;

namespace RubiksCube
{
    public class Window
    {
        private int screenWidth = 720;
        private int screenHeight = 720;
        private Camera3D camera;
        private int targetFPS = 60;
        private float tilt = (float)Math.Acos(Math.Sqrt(2.0f / 3.0f));
        private float cubeSize = 100.0f;
        private float magnitude;
        private Vector3 up;
        private Vector3 right;
        private Vector3 pos;
        private RubiksCube rbx;
        private int[,] solution;

        public Window()
        {
            InitWindow(screenWidth, screenHeight, "Rubik's Cube");

            this.magnitude = 400.0f;
            this.up = SetMagnitude(new Vector3(0.0f, 1.0f, 0.0f), 1.0f);
            this.right = SetMagnitude(new Vector3(-1.0f, 0.0f, 1.0f), 1.0f);
            this.pos = RotateVector(new Vector3(1.0f, 0.0f, 1.0f), this.tilt, this.right, this.magnitude);

            this.camera = new Camera3D(
                    this.pos, // Position
                    new Vector3(0.0f, 0.0f, 0.0f), // Target
                    this.up, // Up
                    45.0f, // Field of View
                    0); // Camera projection

            SetCameraMode(camera, CAMERA_CUSTOM);
            SetTargetFPS(targetFPS);

            // Initialize the Rubik's Cube
            this.rbx = new RubiksCube(3, cubeSize);
        }

        public void Run()
        {
            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                this.Update();

                // Draw
                this.Draw();
            }
        }

        public void Update()
        {
            // --------------------------------------------------
            // KeyBindings (see KeyBindings.cs)
            // --------------------------------------------------

            // Reset camera
            if (ResetCam()) { ResetCamera(); }

            // Randomize cube colors (for testing)
            if (Randomize()) { this.rbx.Randomize(); }

            // Moves
            if (FMove()) { Move(0, !AltBind()); }
            if (RMove()) { Move(1, !AltBind()); }
            if (UMove()) { Move(2, AltBind()); }
            if (LMove()) { Move(3, !AltBind()); }
            if (BMove()) { Move(4, !AltBind()); }
            if (DMove()) { Move(5, AltBind()); }
            if (MMove()) { Move(6, AltBind(), 1); }
            if (EMove()) { Move(7, !AltBind(), 1); }
            if (SMove()) { Move(8, !AltBind(), 1); }

            // Game Logic commands
            if (Scramble()) { SetCube(); }

            // Rotate the cube
            if (PanLeft()) { this.rbx.RotateCubeLeft(); }
            if (PanRight()) { this.rbx.RotateCubeRight(); }
            if (PanUp()) { this.rbx.RotateCubeUp(); }
            if (PanDown()) { this.rbx.RotateCubeDown(); }
            if (Test()) { this.pos = RotateVector(this.pos, (float)Math.PI, new Vector3(0.0f, 1.0f, 0.0f), this.magnitude); }

            // Zoom in/out
            if (PanIn() && this.magnitude > this.cubeSize * 2.0f)
            {
                this.pos = SetMagnitude(this.pos, this.magnitude - 1.0f);
                this.magnitude -= 1.0f;
            }
            if (PanOut() && this.magnitude < this.cubeSize * 6.0f)
            {
                this.pos = SetMagnitude(this.pos, this.magnitude + 1.0f);
                this.magnitude += 1.0f;
            }

            // Swap cube dimensions
            if (QuickSwap1()) { this.rbx = new RubiksCube(1, this.cubeSize); ResetCamera(); }
            if (QuickSwap2()) { this.rbx = new RubiksCube(2, this.cubeSize); ResetCamera(); }
            if (QuickSwap3()) { this.rbx = new RubiksCube(3, this.cubeSize); ResetCamera(); }
            if (QuickSwap4()) { this.rbx = new RubiksCube(4, this.cubeSize); ResetCamera(); }
            if (QuickSwap5()) { this.rbx = new RubiksCube(5, this.cubeSize); ResetCamera(); }
            if (QuickSwap6()) { this.rbx = new RubiksCube(6, this.cubeSize); ResetCamera(); }
            if (QuickSwap7()) { this.rbx = new RubiksCube(7, this.cubeSize); ResetCamera(); }
            if (QuickSwap8()) { this.rbx = new RubiksCube(8, this.cubeSize); ResetCamera(); }
            if (QuickSwap9()) { this.rbx = new RubiksCube(9, this.cubeSize); ResetCamera(); }

            // --------------------------------------------------
            // End KeyBindings
            // --------------------------------------------------

            // Update the camera
            UpdateCamera(ref this.camera);
            this.camera.position = this.pos;
        }

        public void Draw()
        {
            // Draw
            BeginDrawing();
            ClearBackground(RAYWHITE);

            // Background
            DrawRectangle(0, 0, screenWidth, screenHeight, new Color(240, 240, 240, 255));

            // Draw 3D cube
            BeginMode3D(this.camera);
            this.rbx.Draw();
            EndMode3D();

            // Draw 2D cube minimap
            this.rbx.Draw2D(new Vector2(6.5f * screenWidth / 8.0f, 1.5f * screenHeight / 8.0f), this.magnitude / 8.0f);

            DrawFPS(10, 10);
            EndDrawing();
        }

        public Vector3 SetMagnitude(Vector3 v, float finalMagnitude)
        {
            float initialMagnitude = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

            // No change, invalid magnitude or already correct
            if (initialMagnitude == finalMagnitude) return v;

            // Get normalized vector
            Vector3 normalized = Vector3.Normalize(v);

            // Get new vector
            return normalized * finalMagnitude;
        }

        public Vector3 RotateVector(Vector3 v, float angle, Vector3 axis, float mag)
        {
            // Get normalized vector
            Vector3 normalized = Vector3.Normalize(v);

            // Get new vector
            return SetMagnitude(Vector3.Transform(normalized, Matrix4x4.CreateFromAxisAngle(axis, angle)), mag);
        }

        public void ResetCamera()
        {
            this.magnitude = 400.0f;
            this.up = SetMagnitude(new Vector3(0.0f, 1.0f, 0.0f), 1.0f);
            this.right = SetMagnitude(new Vector3(-1.0f, 0.0f, 1.0f), 1.0f);
            this.pos = RotateVector(new Vector3(1.0f, 0.0f, 1.0f), this.tilt, this.right, this.magnitude);
        }

        public int GetCurrentFace()
        {
            // Get position of front face
            Vector3 facePosition = RotateVector(this.pos, -this.tilt, this.right, 1.0f);
            facePosition = RotateVector(facePosition, -(float)Math.PI / 4.0f, this.up, 1.0f);

            // Get which face based on direction of the face
            return 0;
        }

        public void SetCube(int numMoves = 100)
        {
            this.rbx = new RubiksCube(this.rbx.dimension, this.cubeSize);
            var rand = new Random();
            this.solution = new int[numMoves, 3];
            for (int i = 0; i < numMoves; i++)
            {
                // Move type
                int move = rand.Next(0, 17);

                // Check for edge cases
                int reverse = rand.Next(0, 2);  // Reverse move

                // Row to move (affects M,E,S moves only)
                int rowNum = rand.Next(0, this.rbx.dimension);

                // Add to solution array
                this.solution[i, 0] = move;
                this.solution[i, 1] = reverse;
                this.solution[i, 2] = rowNum;

                // Perform move
                Move(move, (reverse == 1), rowNum);
            }
        }

        public void Move(int num, bool reverse = false, int rowNum = 1)
        {
            switch(num)
            {
            case 0:
                this.rbx.RotateFace(0, reverse);
                this.rbx.RotateEdge(0, 0, reverse);
                break;
            case 1:
                this.rbx.RotateFace(2, reverse);
                this.rbx.RotateEdge(2, 0, reverse);
                break;
            case 2:
                this.rbx.RotateFace(4, !reverse);
                this.rbx.RotateEdge(4, 0, reverse);
                break;
            case 3:
                this.rbx.RotateFace(3, reverse);
                this.rbx.RotateEdge(3, 0, reverse);
                break;
            case 4:
                this.rbx.RotateFace(1, reverse);
                this.rbx.RotateEdge(1, 0, reverse);
                break;
            case 5:
                this.rbx.RotateFace(5, !reverse);
                this.rbx.RotateEdge(5, 0, reverse);
                break;
            case 6:
                this.rbx.RotateEdge(2, 1, reverse);
                break;
            case 7:
                this.rbx.RotateEdge(0, 1, reverse);
                break;
            case 8:
                this.rbx.RotateEdge(4, 1, reverse);
                break;
            }
        }
    }
}
