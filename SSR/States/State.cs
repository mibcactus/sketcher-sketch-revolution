using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR; 

public abstract class State {
    protected StateID _stateID;
    protected Texture2D _backgroundTxt;
    protected DependencyContainer _deps;
    protected GamePadState _gamePadState;

    public State(DependencyContainer deps) {
        _deps = deps;
    }

    public State(DependencyContainer deps, StateID id) {
        _deps = deps;
        _stateID = id;
    }
    

    // hooks 
    public abstract void LoadContent();
    
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}