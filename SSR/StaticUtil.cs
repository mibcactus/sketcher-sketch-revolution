using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenCvSharp;

namespace SSR;

public static class StaticUtil {
    
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
    
    /*
    public static Mat Texture2DtoMat(Texture2D texture) {
        Mat mat;

        return mat;
    }*/
}

