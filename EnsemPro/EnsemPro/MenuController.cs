using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnsemPro
{
    // Sorry Caleb, for changing your code, please fix it if you are unsatisfied with the design
    class MenuController // : DrawableGameComponent
    {
        public enum Hover
        {
            Story,
            Free,
            Exit
        }

        public GameModel gameState;
        public MenuView menuView;
        public Hover hover;
        KeyboardState lastState = Keyboard.GetState();

        public MenuController(GameModel gm, SpriteBatch sb)
            //: base(g)
        {
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
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down))
            {
                NextHover(hover);
            }
            else if (ks.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
            {
                PreviousHover(hover);
            }
            else if (gameState.Input.Confirm)
            {
                switch (hover)
                {
                    case Hover.Story:
                        gameState.CurrentScreen = DataTypes.Screens.WorldMap;
                        break;
                    case Hover.Free:
                        gameState.CurrentScreen = DataTypes.Screens.SelectLevel;
                        break;
                    case Hover.Exit:
                        // TODO DOESN'T DO ANYTHING YET
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

        public /*override*/ void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            menuView.Draw(hover);
        }
    }
}
