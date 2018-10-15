using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication9
{
    public partial class Form1 : Form
    {
        Dictionary<string, int> letterCount = new Dictionary<string, int>();
        List<string> letters = new List<string>();
        List<HuffmanNode> huffman = new List<HuffmanNode>();
        HuffmanTree huffmanTree = new HuffmanTree();
        string name;
        public Form1()
        {
            InitializeComponent();
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseUp += panel1_MouseUp;
            panel1.MouseMove += panel1_MouseMove;
        }
        private bool _dragging = false;
        private Point _start_point = new Point(0, 0);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            string encodedString = "";
            huffmanTree.Build(name);
            BitArray encoded = huffmanTree.Encode(name);
            foreach (bool bit in encoded)
            {
                encodedString += (bit ? 1 : 0);
            }
            textBox2.Text = encodedString;
            for (int x = 0; x < name.Length; x++)
            {
                if (!letters.Contains(name[x].ToString()))
                {
                    letters.Add(name[x].ToString());
                }
            }
            for (int x = 0; x < letters.Count; x++)
            {
                int letterFreq = 0;
                for (int y = 0; y < name.Length; y++)
                {
                    if (letters[x] == name[y].ToString())
                        letterFreq++;
                }
                letterCount.Add(letters[x], letterFreq);
            }
            foreach (var letter in letterCount)
            {
                huffman.Add(new HuffmanNode(letter.Key, letter.Value, null, null));
                dataGridView1.Rows.Add(letter.Key, letter.Value);
            }
        }

        public void printHuffman(List<HuffmanNode> huffman)
        {
            List<HuffmanNode> sortedHuffman = sortHuffman(huffman);
            for (int x = 0; x < huffman.Count; x++)
            {
                Console.WriteLine(sortedHuffman[x].name + " " + sortedHuffman[x].frequency);
            }
            Console.WriteLine();
            while (sortedHuffman.Count != 0)
            {
                if (sortedHuffman.Count == 1)
                {
                    Console.WriteLine(sortedHuffman[0].frequency + " " + sortedHuffman[0].name);
                    sortedHuffman.RemoveAt(0);
                }
                else
                {
                    Console.WriteLine(sortedHuffman[0].frequency + " " + sortedHuffman[0].name);
                    Console.WriteLine(sortedHuffman[1].frequency + " " + sortedHuffman[1].name);
                    sortedHuffman.Add(new HuffmanNode(sortedHuffman[0].name + sortedHuffman[1].name, sortedHuffman[0].frequency + sortedHuffman[1].frequency, sortedHuffman[0], sortedHuffman[1]));
                    sortedHuffman.RemoveAt(0);
                    sortedHuffman.RemoveAt(0);
                    sortedHuffman = sortHuffman(sortedHuffman);
                    Console.WriteLine();
                }
            }

        }
        public List<HuffmanNode> sortHuffman(List<HuffmanNode> huffman)
        {
            List<HuffmanNode> sortedHuffman = new List<HuffmanNode>();
            int[] frequency = new int[huffman.Count];
            string[] name = new string[huffman.Count];
            for (int x = 0; x < huffman.Count; x++)
            {
                frequency[x] = huffman[x].frequency;
                name[x] = huffman[x].name;
            }
            for (int x = 0; x < huffman.Count; x++)
            {
                int minimum = x;
                for (int y = x; y < huffman.Count; y++)
                {
                    if (frequency[minimum] > frequency[y])
                    {
                        minimum = y;
                    }
                }
                int temp = frequency[x];
                frequency[x] = frequency[minimum];
                frequency[minimum] = temp;
                string tempName = name[x];
                name[x] = name[minimum];
                name[minimum] = tempName;
            }
            for (int x = 0; x < huffman.Count; x++)
            {
                sortedHuffman.Add(new HuffmanNode(name[x], frequency[x], null, null));
            }
            return sortedHuffman;
        }

        private void button2_Click(object sender, EventArgs e)
        {      
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            List<HuffmanNode> sortedHuffman = sortHuffman(huffman);
            while (sortedHuffman.Count != 0)
            {
                if (sortedHuffman.Count == 1)
                {
                    sortedHuffman.RemoveAt(0);
                }
                else
                {
                    graph.AddNode(sortedHuffman[1].name + " | " + sortedHuffman[1].frequency.ToString());
                    graph.AddNode(sortedHuffman[0].name + " | " + sortedHuffman[0].frequency.ToString());
                    graph.FindNode(sortedHuffman[1].name + " | " + sortedHuffman[1].frequency.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                    graph.FindNode(sortedHuffman[0].name + " | " + sortedHuffman[0].frequency.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                    graph.FindNode(sortedHuffman[1].name + " | " + sortedHuffman[1].frequency.ToString()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                    graph.FindNode(sortedHuffman[0].name + " | " + sortedHuffman[0].frequency.ToString()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                    graph.AddEdge((sortedHuffman[0].name + sortedHuffman[1].name) + " | " + ((sortedHuffman[0].frequency + sortedHuffman[1].frequency)).ToString(), sortedHuffman[1].name + " | " + sortedHuffman[1].frequency.ToString());
                    graph.AddEdge((sortedHuffman[0].name + sortedHuffman[1].name) + " | " + ((sortedHuffman[0].frequency + sortedHuffman[1].frequency)).ToString(), sortedHuffman[0].name + " | " + sortedHuffman[0].frequency.ToString());
                    graph.AddNode((sortedHuffman[0].name + sortedHuffman[1].name) + " | " + ((sortedHuffman[0].frequency + sortedHuffman[1].frequency)).ToString());
                    graph.FindNode((sortedHuffman[0].name + sortedHuffman[1].name) + " | " + ((sortedHuffman[0].frequency + sortedHuffman[1].frequency)).ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                    graph.FindNode((sortedHuffman[0].name + sortedHuffman[1].name) + " | " + ((sortedHuffman[0].frequency + sortedHuffman[1].frequency)).ToString()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                    sortedHuffman.Add(new HuffmanNode(sortedHuffman[0].name + sortedHuffman[1].name, sortedHuffman[0].frequency + sortedHuffman[1].frequency, sortedHuffman[0], sortedHuffman[1]));
                    sortedHuffman.RemoveAt(0);
                    sortedHuffman.RemoveAt(0);
                    sortedHuffman = sortHuffman(sortedHuffman);
                }
            }

            viewer.Graph = graph;
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string encodedString = "";
            huffmanTree.Build(name);
            BitArray encoded = huffmanTree.Encode(name);
            foreach (bool bit in encoded)
            {
                encodedString += (bit ? 1 : 0);
            }
            string decoded = huffmanTree.Decode(encoded);
            textBox3.Text = decoded.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class HuffmanNode
    {
        public string name;
        public int frequency;

        public HuffmanNode left;
        public HuffmanNode right;

        public HuffmanNode(string name, int frequency, HuffmanNode left, HuffmanNode right)
        {
            this.name = name;
            this.frequency = frequency;
            this.left = left;
            this.right = right;
        }
    }

    public class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

                if (orderedNodes.Count >= 2)
                {
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                    Node parent = new Node()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();

            }

        }

        public BitArray Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            Node current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(Node node)
        {
            return (node.Left == null && node.Right == null);
        }

    }

    public class Node
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            if (Right == null && Left == null)
            {
                if (symbol.Equals(this.Symbol))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
