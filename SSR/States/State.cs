using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR; 

public abstract class State {
    protected StateID _stateID;
    protected Texture2D _backgroundTxt;
    protected DependencyContainer _dependencyBox;
    protected GamePadState _gamePadState;

    public State(DependencyContainer dependencyBox) {
        _dependencyBox = dependencyBox;
    }

    public State(DependencyContainer dependencyBox, StateID id) {
        _dependencyBox = dependencyBox;
        _stateID = id;
    }
    

    // hooks 
    public abstract void LoadContent();
    
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);
}