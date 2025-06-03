#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using RectangleF = System.Drawing.RectangleF;

namespace MarioWinForms
{
    public partial class Form1 : Form
    {
        // --- константы геймплея ---
        private const int TILE = 32;
        private const float GRAVITY = 0.50f;
        private const float MAX_FALL = 12f;
        private const float MOVE_SPEED = 3.0f;
        private const float JUMP_FORCE = 10.5f;

        // --- кисти для отрисовки ---
        private static readonly Brush BlockBrush = Brushes.SaddleBrown;
        private static readonly Brush PlayerBrush = Brushes.Red;

        // --- данные уровня ---
        private readonly string[] level =
        {
            "############################",
            "#..........................#",
            "#..........###.............#",
            "#..................##......#",
            "#..P.......................#",
            "############################"
        };

        private readonly List<Block> blocks = new();
        private Player? player;

        // --- ввод ---
        private bool leftHeld, rightHeld, jumpHeld;

        public Form1()
        {
            InitializeComponent();

            BuildLevel();
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        // ---------- построение карты ----------
        private void BuildLevel()
        {
            blocks.Clear();
            player = null;

            for (int y = 0; y < level.Length; y++)
                for (int x = 0; x < level[y].Length; x++)
                {
                    switch (level[y][x])
                    {
                        case '#':
                            blocks.Add(new Block(x * TILE, y * TILE));
                            break;
                        case 'P':
                            player = new Player(x * TILE, y * TILE);
                            break;
                    }
                }

            if (player is null)
                throw new InvalidOperationException("В карте нет символа 'P'.");
        }

        // ---------- игровой цикл ----------
        private void GameLoop(object? sender, EventArgs e)
        {
            if (player is null) return;

            HandleInput();
            player.Update(blocks);
            Invalidate();                 // запрос перерисовки
        }

        private void HandleInput()
        {
            if (player is null) return;

            player.Vel = new Vector2(
                leftHeld ? -MOVE_SPEED :
                rightHeld ? MOVE_SPEED : 0,
                player.Vel.Y);

            if (jumpHeld && player.Grounded)
            {
                player.Vel = new Vector2(player.Vel.X, -JUMP_FORCE);
                player.Grounded = false;
            }
        }

        // ---------- рендер ----------
        protected override void OnPaint(PaintEventArgs e)
        {
            if (player is null) return;

            Graphics g = e.Graphics;
            int camX = Math.Max(0, (int)(player.Rect.X + TILE / 2) - ClientSize.Width / 2);

            foreach (var b in blocks)
                g.FillRectangle(BlockBrush, b.Rect.X - camX, b.Rect.Y, TILE, TILE);

            g.FillRectangle(PlayerBrush, player.Rect.X - camX, player.Rect.Y, TILE, TILE);
        }

        // ---------- клавиатура ----------
        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.A or Keys.Left) leftHeld = true;
            if (e.KeyCode is Keys.D or Keys.Right) rightHeld = true;
            if (e.KeyCode is Keys.Space or Keys.W or Keys.Up) jumpHeld = true;
        }
        private void OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.A or Keys.Left) leftHeld = false;
            if (e.KeyCode is Keys.D or Keys.Right) rightHeld = false;
            if (e.KeyCode is Keys.Space or Keys.W or Keys.Up) jumpHeld = false;
        }

        // ---------- сущности ----------
        private sealed class Player
        {
            internal RectangleF Rect;
            internal Vector2 Vel;
            internal bool Grounded;

            internal Player(float x, float y)
                => Rect = new RectangleF(x, y, TILE, TILE);

            internal void Update(IEnumerable<Block> world)
            {
                // гравитация
                if (!Grounded)
                    Vel = new Vector2(Vel.X, Math.Min(Vel.Y + GRAVITY, MAX_FALL));

                // --- горизонталь ---
                Rect.X += Vel.X;
                foreach (var b in world)
                    if (Rect.IntersectsWith(b.Rect))
                        Rect.X = Vel.X > 0 ? b.Rect.Left - Rect.Width
                                           : b.Rect.Right;

                // --- вертикаль ---
                Grounded = false;
                Rect.Y += Vel.Y;
                foreach (var b in world)
                    if (Rect.IntersectsWith(b.Rect))
                    {
                        if (Vel.Y > 0)
                        {
                            Rect.Y = b.Rect.Top - Rect.Height;
                            Grounded = true;
                        }
                        else
                            Rect.Y = b.Rect.Bottom;

                        Vel = new Vector2(Vel.X, 0);
                    }
            }
        }

        private sealed class Block
        {
            internal RectangleF Rect;
            internal Block(float x, float y)
                => Rect = new RectangleF(x, y, TILE, TILE);
        }
    }
}
