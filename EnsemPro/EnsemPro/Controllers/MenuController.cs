using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    class MenuController : IUpdateable
    {
        public enum Hover
        {
            Story,
            Free,
            Exit,
            None
        }

        public Game game;
        public GameState gameState;
        public MenuView menuView;
        public Hover hover = Hover.None;
        Hover lastHover = Hover.None;

        SoundEffect TitleMove;
        SoundEffect TitleSelect;

        // todo: remove magic numbers
        Rectangle storyBox = new Rectangle(300, 390, 310, 40);
        Rectangle freeBox = new Rectangle(460, 440, 240, 40);
        Rectangle exitBox = new Rectangle(630, 490, 110, 40);

        public MenuController(Game g, GameState gm, SpriteBatch sb)
        {
            game = g;
            gameState = gm;
            menuView = new MenuView(g, sb, this);
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager cm)
        {
            menuView.LoadContent(cm);
            TitleMove = cm.Load<SoundEffect>("Sounds//TitleMove");
            TitleSelect = cm.Load<SoundEffect>("Sounds//TitleSelect");
        }

        bool Contains(Rectangle box)
        {
            Vector2 p = gameState.Input.Position;
            if (p.X < box.Left) return false;
            if (p.X > box.Right) return false;
            if (p.Y < box.Top) return false;
            if (p.Y > box.Bottom) return false;
            return true;
        }

        public void Update(GameTime gameTime)
        {
            if (Contains(storyBox))
                hover = Hover.Story;
            else if (Contains(freeBox))
                hover = Hover.Free;
            else if (Contains(exitBox))
                hover = Hover.Exit;
            else
                hover = Hover.None;

            if (hover != lastHover && hover != Hover.None)
                TitleMove.Play();

            if (gameState.Input.Confirm)
            {
                TitleSelect.Play();
                switch (hover)
                {
                    case Hover.Story:
                        gameState.CurrentScreen = DataTypes.Screens.WorldMap;
                        break;
                    case Hover.Free:
                        gameState.CurrentScreen = DataTypes.Screens.SelectLevel;
                        break;
                    case Hover.Exit:
                        game.Exit();
                        break;                        
                }

            }
            lastHover = hover;
        }

        public void Draw(GameTime gameTime)
        {
            menuView.Draw(gameTime);
        }


        public bool Enabled
        {
            get { return true; }
        }

        public event System.EventHandler<System.EventArgs> EnabledChanged
        {
            add { }
            remove { }
        }

        public int UpdateOrder
        {
            get { return 5; }
        }

        public event System.EventHandler<System.EventArgs> UpdateOrderChanged
        {
            add { }
            remove { }
        }
    }
}
