/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GifPlayer
{
    public class SequenceFrames : MonoBehaviour
    {
        public bool Loop = true;
        public SequenceFrame[] Frames;

        SpriteRenderer _rendererCanvas;
        Image _imageCanvas;
        RawImage _rawCanvas;

        protected virtual void Awake()//awake before enable,start after enable
        {
            _rendererCanvas = GetComponent<SpriteRenderer>();
            _imageCanvas = GetComponent<Image>();
            _rawCanvas = GetComponent<RawImage>();
        }

        int _frameIndex = 0;

        IEnumerator PlayNextFrame()
        {
            _frameIndex %= Frames.Length;

            if (_rendererCanvas)
                _rendererCanvas.sprite = Frames[_frameIndex].Sprite;
            if (_imageCanvas)
                _imageCanvas.sprite = Frames[_frameIndex].Sprite;
            if (_rawCanvas)
                _rawCanvas.texture = Frames[_frameIndex].Texture;

            yield return new WaitForSeconds(Frames[_frameIndex].DelaySeconds);

            _frameIndex++;

            if (!Loop && _frameIndex == Frames.Length)
                yield break;

            StartCoroutine(PlayNextFrame());
        }

        void OnEnable()
        {
            StartCoroutine(PlayNextFrame());
        }

        void OnDisable()
        {
            _frameIndex = 0;
        }
    }
}