using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR;

public class Pen {
    private const float MAX_RADIAN = 6.28f;
    private const float Pi = 3.14f;

    private GamePadDPad free_dpad = new GamePadDPad(
        ButtonState.Released, ButtonState.Released,
        ButtonState.Released, ButtonState.Released);

    private DependencyContainer _dependencyBox;

    private Texture2D down_texture;
    private Texture2D up_texture;
    private Texture2D current_texture;

    private Vector2 _position;
    private Vector2 _origin_offset;
    private float _angle = 0f;

    private float _rotation_speed = 0.15f;

    private Vector2 vector_speed = Vector2.Zero;

    private const int MAX_SPEED = 30;
    private const int MIN_SPEED = 1;
    private int _velocity = 15;


    public bool isDown = true;

    private Canvas _canvas;

    public Pen(DependencyContainer _dependencies) {
        _dependencyBox = _dependencies;

        _position = _dependencies.CentreVector();

        // TODO: Add appropriate textures
        down_texture = _dependencyBox.loadTexture2D("pen_down");
        
        up_texture = _dependencyBox.loadTexture2D("pen_up");
        current_texture = down_texture;

        _origin_offset = new Vector2(up_texture.Width / 2, up_texture.Height / 2);

        _canvas = new Canvas(_dependencies, new Vector2(200, 200));
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    private bool RotatePressed(GamePadDPad dpad) {
        return dpad.Left == ButtonState.Pressed || dpad.Right == ButtonState.Pressed;
    }

    private bool buttonsPressed(GamePadState gamePadState) {
        if (gamePadState.DPad != free_dpad) {
            return true;
        }

        if (gamePadState.Buttons.A == ButtonState.Pressed || gamePadState.Buttons.A == ButtonState.Pressed ||
            gamePadState.Buttons.Start == ButtonState.Pressed) {
            return true;
        }

        return gamePadState.Triggers.Right < 0.5f;
    }

    /*
    private void old() {
        if (dPad.Up == ButtonState.Pressed) {
            displacement.Y += _pixel_speed;
        }

        if (dPad.Down == ButtonState.Pressed) {
            displacement.Y -= _pixel_speed;
        }

        //if (RotatePressed(dPad)) {
        
        float temp_angle = 0;
        if (dPad.Left == ButtonState.Pressed) {
            temp_angle -= _rotation_speed;
        }

        if (dPad.Right == ButtonState.Pressed) {
            temp_angle += _rotation_speed;
        }

        _angle += temp_angle;

        if (_angle > MAX_RADIAN) {
            _angle -= MAX_RADIAN;
        }
        else if (_angle < 0) {
            _angle = MAX_RADIAN - _angle;
        }
        //}

        double degree_angle = (180 / Pi) * _angle;
        //double degree_angle = temp_angle * (180 / Pi);



        float newx = (float)Math.Floor((Math.Cos(degree_angle) * displacement.X) -
                                       (Math.Sin(degree_angle) * displacement.Y));
        float newy = (float)Math.Floor((Math.Sin(degree_angle) * displacement.X) -
                                       (Math.Cos(degree_angle) * displacement.Y));

        displacement.X = newx;
        displacement.Y = newy;

        displacement.Normalize();
        displacement = Vector2.Multiply(displacement, _pixel_speed);

        if (dPad.Up == ButtonState.Pressed) {
            _position -= displacement;
        }

        if (dPad.Down == ButtonState.Pressed) {
            _position += displacement;
        }
            
    }*/


    public void Update(GameTime gameTime, GamePadState gamePadState) {
        /*if (!buttonsPressed(gamePadState)) {
            return;
        }*/

        _dependencyBox.updateInputBuffer();

        if (gamePadState.IsButtonDown(Buttons.Start)) {
            _dependencyBox.paused = !_dependencyBox.paused;
        }
        /*
        if (gamePadState.IsButtonDown(Buttons.RightTrigger)) {
            isDown = !isDown;
        }*/

        isDown = true;
        
        if (isDown) {
            if (gamePadState.IsButtonDown(Buttons.B)) {
                _velocity -= 3;
            }

            if (gamePadState.IsButtonDown(Buttons.A)) {
                _velocity += 3;
            }
        
            _canvas.setBrushSize(_velocity);
            
            GamePadDPad dPad = gamePadState.DPad;
        
            float temp_angle = 0;
            if (dPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A)) {
                temp_angle -= _rotation_speed;
            }

            if (dPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D)) {
                temp_angle += _rotation_speed;
            }

        
            _angle += temp_angle;
            if (_angle > MAX_RADIAN) {
                _angle -= MAX_RADIAN;
            }
            else if (_angle < 0) {
                _angle = MAX_RADIAN - _angle;
            }
        
            Vector2 ExtendedVector = new Vector2((float) Math.Cos(_angle), (float) Math.Sin(_angle));
            //ExtendedVector.Normalize();
            //Vector2.Multiply(ExtendedVector, _pixel_speed);
            ExtendedVector.X *= _velocity;
            ExtendedVector.Y *= _velocity;
            Trace.WriteLine("\nExtendedVector: " + ExtendedVector);
        
            if (dPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W)) {
                _position -= ExtendedVector;
            }

            if (dPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S)) {
                _position += ExtendedVector;
            }
            
            current_texture = down_texture;
            _canvas.updateCanvas(_position + _origin_offset);
        }
        else {
            current_texture = up_texture;
        }
    }

    public double toDegrees() {
        return _angle * (180 / Pi);
    }

    public float returnAngle() {
        return _angle;
    }

    public void Draw() {
        _canvas.drawCanvas();
        //_dependencyBox._SpriteBatch.Draw(current_texture, _position, Color.White);
        var rotate_angle = _angle + 1.5f*Pi;
        _dependencyBox._SpriteBatch.Draw(current_texture, _position + _origin_offset, null, Color.White, rotate_angle,
            _origin_offset, Vector2.One, SpriteEffects.None, 0f);
    }
}