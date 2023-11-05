using Microsoft.Xna.Framework;

namespace SSR; 

public class MenuState : State {
    public MenuState(DependencyContainer dependencyBox) : base(dependencyBox, StateID.MENU) { }
    public override void LoadContent() {
        _backgroundTxt = _dependencyBox.loadTexture2D("");
    }

    public override void Update(GameTime gameTime) {
        throw new System.NotImplementedException();
    }

    public override void Draw(GameTime gameTime) {
        throw new System.NotImplementedException();
    }
}