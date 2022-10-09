using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace SimpleX
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            OnRefresh = Refresh;
        }

        private Random random = new Random();
        private Quadtree<Agent> quadtree = null;
        private List<Agent> agents = new List<Agent>(100);

        private Thread thread = null;
        private Action OnRefresh;

        private int W => this.ClientSize.Width;
        private int H => this.ClientSize.Height;

        private void OnLoadHandler(object sender, EventArgs e)
        {
            quadtree = new Quadtree<Agent>(W * 0.5f, H * 0.5f, W, H, 5, 6);

            for (int i=0; i<500; i++)
            {
                var agent = CreateRandomAgent();
                agents.Add(agent);

                quadtree.Insert(agent);
            }

            thread = new Thread(OnTick);
            thread.Start();
        }

        private void OnPaintHandler(object sender, PaintEventArgs e)
        {
            var grap = e.Graphics;
            grap.SmoothingMode = SmoothingMode.HighQuality;

            foreach (var agent in agents)
            {
                DrawAgent(grap, agent);
            }

            if (quadtree != null)
            {
                DrawQuadtree(grap, quadtree);
            }
        }

        private void DrawAgent(Graphics grap, Agent agent)
        {
            var brush = new SolidBrush(agent.color);

            var x = agent.x - agent.w * 0.5f;
            var y = agent.y - agent.h * 0.5f;
            var w = agent.w;
            var h = agent.h;

            grap.FillRectangle(brush, x, y, w, h);
        }

        private void DrawQuadtree(Graphics grap, Quadtree<Agent> quadtree)
        {
            var pen = new Pen(Color.DarkSlateGray);

            var x = quadtree.x - quadtree.width * 0.5f;
            var y = quadtree.y - quadtree.height * 0.5f;
            var w = quadtree.width;
            var h = quadtree.height;

            grap.DrawRectangle(pen, x, y, w, h);

            if (quadtree.nodes != null)
            {
                foreach (var subtree in quadtree.nodes)
                {
                    DrawQuadtree(grap, subtree);
                }
            }
        }

        private void OnTick()
        {
            while (true)
            {
                var dt = 0.016f;// 固定的间隔时间
                foreach (var agent in agents)
                {
                    var m = agent.direction * agent.speed * dt;
                    agent.x += m.x;
                    agent.y += m.y;
                    CheckBorderCollision(agent);
                }
                RebuildQuadtree();

                Invoke(OnRefresh);
            }
        }

        private Agent CreateRandomAgent()
        {
            var x = random.Next(30, W - 30);
            var y = random.Next(30, H - 30);
            var w = random.Next(10, 20);
            var h = random.Next(10, 20);
            var s = random.Next(10, 30);

            var agent = new Agent(x, y, w, h, s);

            var u = (float)random.NextDouble();
            var v = (float)random.NextDouble();
            var d = new Vector(u, v);
            agent.direction = d.normalized;

            var r = random.Next(0, 255);
            var g = random.Next(0, 255);
            var b = random.Next(0, 255);
            agent.color = Color.FromArgb(r, g, b);

            return agent;
        }

        private void CheckBorderCollision(Agent agent)
        {
            var minx = agent.x - agent.w * 0.5f;
            var miny = agent.y - agent.h * 0.5f;
            var maxx = agent.x + agent.w * 0.5f;
            var maxy = agent.y + agent.h * 0.5f;

            var normal = Vector.zero;
            if (minx <= 0)
            {
                agent.x = agent.w * 0.5f;
                normal = Vector.right;
            }
            else if (maxx >= W)
            {
                agent.x = W - agent.w * 0.5f;
                normal = Vector.left;
            }
            else if (miny <= 0)
            {
                agent.y = agent.h * 0.5f;
                normal = Vector.down;
            }
            else if (maxy >= H)
            {
                agent.y = H - agent.h * 0.5f;
                normal = Vector.up;
            }
            else
            {
                return;
            }

            var direction = agent.direction;
            agent.direction = Vector.Reflect(ref direction, ref normal);
        }

        private void RebuildQuadtree()
        {
            if (quadtree != null)
            {
                quadtree.Clear();
                foreach (var agent in agents)
                {
                    quadtree.Insert(agent);
                }
            }
        }

        private void OnClosingHandler(object sender, FormClosingEventArgs e)
        {
            try
            {
                thread.Abort();
            }
            catch
            {

            }
        }
    }
}
