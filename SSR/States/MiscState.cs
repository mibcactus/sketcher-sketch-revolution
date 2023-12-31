﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR; 

public class MiscState : State {

    private Texture2D guy;
    private Vector2 guy_position;

    private Pen _pen;

    public MiscState(DependencyContainer deps) : base(deps, StateID.MISC) {
        _pen = new Pen(_deps, new Vector2(200,200));
    }

    public override void LoadContent() {
        _backgroundTxt = _deps.loadTexture2D("test");
        guy = StaticUtil.genPlaceholderTexture2D(_deps._gamePointer.GraphicsDevice);
        guy_position.X = _deps.screen_width / 2;
        guy_position.Y = _deps.screen_height / 2;
    }

    public override void Update(GameTime gameTime) {
        _gamePadState = GamePad.GetState(PlayerIndex.Four);

        if(_deps.hasInputBufferElapsed()) {
            _pen.Update(gameTime, _gamePadState);
        }
    }

    public override void Draw(GameTime gameTime) {
        _deps._SpriteBatch.Draw(_backgroundTxt, Vector2.Zero, Color.Brown);
        
        _deps._SpriteBatch.Draw(guy, guy_position, Color.White);
        
        _pen.Draw();
        
        string str = "Connected: " + _gamePadState.IsConnected;
        _deps._SpriteBatch.DrawString(_deps.Font(), str, new Vector2(30,50), Color.White);

        List<string> buttonstr = new List<string>();
        buttonstr.Add(_gamePadState.Buttons.ToString());
        buttonstr.Add(_gamePadState.Triggers.ToString());
        buttonstr.Add(_gamePadState.DPad.ToString());
        buttonstr.Add("Input buffer elapsed: " + _deps.hasInputBufferElapsed());
        buttonstr.Add("Pen down: " + _pen.isDown);
        
        Color textcolour = Color.Indigo;
        
        _deps._SpriteBatch.DrawString(_deps.Font(), buttonstr[0], new Vector2(30,70), Color.White);
        _deps._SpriteBatch.DrawString(_deps.Font(), buttonstr[1], new Vector2(30,90), Color.White);
        _deps._SpriteBatch.DrawString(_deps.Font(), buttonstr[2], new Vector2(30,110), Color.White);
        _deps._SpriteBatch.DrawString(_deps.Font(), buttonstr[3], new Vector2(30,140), Color.White);
        _deps._SpriteBatch.DrawString(_deps.Font(), buttonstr[4], new Vector2(30,160), Color.White);

        string angle_str = "Current angle: " + _pen.returnAngle() + " is this many degrees: " + _pen.toDegrees();
        _deps._SpriteBatch.DrawString(_deps.Font(), angle_str, new Vector2(30, 190), textcolour);
        //string pn = "Unit vector: "

    }
}