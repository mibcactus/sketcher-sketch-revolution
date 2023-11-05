using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpenCvSharp;

namespace SSR;

public static class StaticUtil {

    public static bool isPressed(ButtonState _bs) {
        if (_bs == ButtonState.Pressed) {
            return true;
        }

        return false;
    }
    
    public static Texture2D genBasicTexture2D(GraphicsDevice _graphicsDevice, Color color, int width = 100, int height = 100) {
        Texture2D _t = new Texture2D(_graphicsDevice, width, height);

        int h = width * height;
        Color[] data = new Color[h];

        for (int i = 0; i < h; i++) {
            int q = i / 200;
            q += 30;

            data[i] = color;
        }
        
        _t.SetData(data);
        
        return _t;
    }
    
    public static Texture2D genPlaceholderTexture2D(GraphicsDevice _graphicsDevice, int width = 100, int height = 100) {
        Texture2D _t = new Texture2D(_graphicsDevice, width, height);

        int h = width * height;
        Color[] data = new Color[h];

        for (int i = 0; i < h; i++) {
            int q = i / 200;
            q += 30;

            data[i] = new Color(q, q, q);
        }
        
        _t.SetData(data);
        
        return _t;
    }
    
    /*
    public static Texture2D MatToTexture2D(Mat image, GraphicsDevice _graphicsDevice) {
        Texture2D text = genPlaceholderTexture2D(_graphicsDevice);
        return text;
    }*/

    public static int compareimages(string image, Texture2D drawing) {
        try {
            string aaa = @"C:\Users\Milica\Documents\Coding\Hacknotts\sketcher-sketch-revolution\SSR\resources\" +
                         image + ".png";
            
            Mat drawing_mat = Texture2DtoMat(drawing);

            Mat image_mat =
                new Mat(aaa);
            drawing_mat.ConvertTo(drawing_mat, image_mat.Type());

            
            Mat result = image_mat;
            Cv2.Compare(image_mat, drawing_mat, result, CmpType.EQ);
            int similar = Cv2.CountNonZero(result);

            return similar;
        }
        catch (Exception e) {
            Trace.WriteLine(e);
            return 0;
        }
    }
    
    public static Mat Texture2DtoMat(Texture2D texture) {
        // mat with 8bit signed per channel
        try {
            Mat mat = new Mat(new Size(texture.Width, texture.Height), MatType.CV_8S, Scalar.White);
            Color[] temp = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(temp);
            for (int i = 0; i < mat.Width; i++) {
                for (int j = 0; j < mat.Height; j++) {
                    if(temp[i+j*texture.Width] == Color.Black) {
                        mat.Set(i, j, Scalar.Black);
                    }
                }
            }

            return mat;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return new Mat();
        }
    }
}

