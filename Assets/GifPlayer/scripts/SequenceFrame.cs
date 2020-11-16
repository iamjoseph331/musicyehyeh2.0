/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */

using System;
using UnityEngine;

namespace GifPlayer
{
    [Serializable]
    public class SequenceFrame
    {
        public Sprite Sprite;

        public Texture2D Texture
        {
            get
            {
                return Sprite.texture;
            }
        }

        public float DelaySeconds = 0.1f;

        public SequenceFrame(Sprite sprite, float delaySeconds)
        {
            Sprite = sprite;
            DelaySeconds = delaySeconds;
        }
    }
}