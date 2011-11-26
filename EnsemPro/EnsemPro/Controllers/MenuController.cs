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
            Exit
        }

        public Game game;
        public GameState gameState;
        public MenuView menuView;
        public Hover hover;
        KeyboardState lastState = Keyboard.GetState();

        SoundEffect TitleMove;
        SoundEffect TitleSelect;

        public MenuController(Game g, GameState gm, SpriteBatch sb)
            //: base(g)
        {
            game = g;
            gameState = gm;
            menuView = new MenuView(sb);
            hover = Hover.Story;
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

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down))
            {
                NextHover(hover);
                TitleMove.Play();
            }
            else if (ks.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
            {
                PreviousHover(hover);
                TitleMove.Play();
            }
            else if (gameState.Input.Confirm)
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
            lastState = ks;
        }

        public void NextHover(Hover h)
        {
            switch (h)
            {
                case Hover.Story:
                    hover = Hover.Free;
                    break;
                case Hover.Free:
                    hover = Hover.Exit;
                    break;
                case Hover.Exit:
                    hover = Hover.Story;
                    break;
                default:
                    break;
            }
        }

        public void PreviousHover(Hover h)
        {
            switch (h)
            {
                case Hover.Story:
                    hover = Hover.Exit;
                    break;
                case Hover.Free:
                    hover = Hover.Story;
                    break;
                case Hover.Exit:
                    hover = Hover.Free;
                    break;
                default:
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            menuView.Draw(hover);
        }


        public bool Enabled
        {
            get { return true; }
        }

        public event System.EventHandler<System.EventArgs> EnabledChanged;

        public int UpdateOrder
        {
            get { return 5; }
        }

        public event System.EventHandler<System.EventArgs> UpdateOrderChanged;
    }
}
