using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR; 

public class MiscState : State {

    private Texture2D guy;
    private Vector2 guy_position;
    private GamePadState _gamePadState;
    
    public MiscState(DependencyContainer dependencyBox) : base(dependencyBox, StateID.MISC) {
    }

    public override void LoadContent() {
        _backgroundTxt = _dependencyBox.loadTexture2D("test");
        guy = StaticUtil.genPlaceholderTexture2D(_dependencyBox._gamePointer.GraphicsDevice);
        //guy = _dependencyBox.loadTexture2D("guy");
        guy_position.X = _dependencyBox.screen_width / 2;
        guy_position.Y = _dependencyBox.screen_height / 2;
    }

    public override void Update(GameTime gameTime) {
        _gamePadState = GamePad.GetState(PlayerIndex.Four);

        if (!_gamePadState.IsConnected) {
            _dependencyBox.paused = true;
        } else {
            _dependencyBox.paused = false;
            
            //_gamePadState.Buttons.
        }
    }

    public override void Draw(GameTime gameTime) {
        _dependencyBox._SpriteBatch.Draw(_backgroundTxt, Vector2.Zero, Color.Brown);
        
        _dependencyBox._SpriteBatch.Draw(guy, guy_position, Color.White);
        
        string str = "Connected: " + _gamePadState.IsConnected;
        _dependencyBox._SpriteBatch.DrawString(_dependencyBox.Font(), str, new Vector2(30,50), Color.White);

        List<string> buttonstr = new List<string>();
        //buttonstr.Add("A Button: ");
        //buttonstr.Add("B button: ");
        buttonstr.Add(_gamePadState.Buttons.ToString());
        buttonstr.Add(_gamePadState.Triggers.ToString());
        buttonstr.Add(_gamePadState.DPad.ToString());
        
        _dependencyBox._SpriteBatch.DrawString(_dependencyBox.Font(), buttonstr[0], new Vector2(30,70), Color.White);
        _dependencyBox._SpriteBatch.DrawString(_dependencyBox.Font(), buttonstr[1], new Vector2(30,90), Color.White);
        _dependencyBox._SpriteBatch.DrawString(_dependencyBox.Font(), buttonstr[2], new Vector2(30,110), Color.White);


    }
}