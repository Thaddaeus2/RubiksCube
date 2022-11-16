namespace RubiksCube
{
    public class DirectionGraph
    {
        public class Node
        {
            public int id;
            public int[] neighbors { get; set; }

            public Node(int id)
            {
                this.id = id;
                this.neighbors = new int[4];
            }

            public void AddNeighbor(int d, int n)
            {
                neighbors[d] = n;
            }
        }

        private int direction = 0;
        public int currentNode { get; set; }
        private Node[] nodes = new Node[6];

        public DirectionGraph()
        {
            this.currentNode = 0;
            for (int i = 0; i < 6; i++)
            {
                nodes[i] = new Node(i);
            }

            nodes[0].AddNeighbor(0, 4);
            nodes[0].AddNeighbor(1, 2);
            nodes[0].AddNeighbor(2, 5);
            nodes[0].AddNeighbor(3, 3);

            nodes[1].AddNeighbor(0, 4);
            nodes[1].AddNeighbor(1, 3);
            nodes[1].AddNeighbor(2, 5);
            nodes[1].AddNeighbor(3, 2);

            nodes[2].AddNeighbor(0, 4);
            nodes[2].AddNeighbor(1, 1);
            nodes[2].AddNeighbor(2, 5);
            nodes[2].AddNeighbor(3, 0);

            nodes[3].AddNeighbor(0, 4);
            nodes[3].AddNeighbor(1, 0);
            nodes[3].AddNeighbor(2, 5);
            nodes[3].AddNeighbor(3, 1);

            nodes[4].AddNeighbor(0, 2);
            nodes[4].AddNeighbor(1, 0);
            nodes[4].AddNeighbor(2, 3);
            nodes[4].AddNeighbor(3, 1);

            nodes[5].AddNeighbor(0, 2);
            nodes[5].AddNeighbor(1, 1);
            nodes[5].AddNeighbor(2, 3);
            nodes[5].AddNeighbor(3, 0);
        }

        private void Move(int dir)
        {
            // Direction is forward, adjust to point correct way
            Console.Write("(" + this.currentNode + ", " + this.direction + ") -> (");
            int move = (this.direction + dir) % 4;
            Console.Write(this.direction + ") -> (");

            // Get next node and direction
            int nextNode = nodes[this.currentNode].neighbors[move];
            int nextDirection;
            if (this.currentNode < 4 && nextNode < 4) // Don't change, coordinate systems match
            {
                nextDirection = this.direction;
            }
            else // Mismatching coordinate systems, adjust
            {
                // Get which direction we came from
                int prevDirection = -1;
                for (int i = 0; i < 4; i++)
                {
                    if (nodes[nextNode].neighbors[i] == this.currentNode)
                    {
                        prevDirection = i;
                        break;
                    }
                }

                // Get next direction
                nextDirection = (prevDirection + 2) % 4;
            }

            // Update current node and direction
            this.currentNode = nextNode;
            this.direction = nextDirection;
            Console.WriteLine(nextNode + ", " + nextDirection + ")");
        }

        public void Forward() { this.Move(0); }
        public void Downward() { this.Move(2); }
        public void Leftward() { this.Move(3); }
        public void Rightward() { this.Move(1); }
    }
}
