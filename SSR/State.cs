using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SSR; 

public abstract class State {
    protected StateID _stateID;
    protected Texture2D _backgroundTxt;
    protected DependencyContainer _dependencyBox;

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