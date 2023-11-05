using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SSR; 

public class MenuState : State {
    public MenuState(DependencyContainer deps) : base(deps, StateID.MENU) { }
    public override void LoadContent() {
        _backgroundTxt = _deps.loadTexture2D("menu_bg");
    }

    public override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A)) {
            _deps.SetState("game");
        }
    }

    public override void Draw(GameTime gameTime) {
        _deps._SpriteBatch.Draw(_backgroundTxt, Vector2.Zero, Color.White);
        
        //_deps._SpriteBatch.Draw(_deps.placeholder, _deps.CentreVector(), Color.Aqua);
    }
}