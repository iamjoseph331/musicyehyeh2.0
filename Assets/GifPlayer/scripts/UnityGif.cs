/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */

using System;
using UnityEngine;

namespace GifPlayer
{
    public class UnityGif : SequenceFrames
    {
        [Header("GIF的文件流，将 .gif 变为 .gif.bytes 后便可拖拽")]
        [Header("For Serialize Field,you can rename .gif to .gif.bytes")]
        public TextAsset GifBytes;

        [Header("预加载状态，请先点击Preload按钮，确认资源已经生成后再勾选")]
        [Header("resources had been gernarated ,then select")]
        [Header("Preload state,click Preload botton,make sure")]
        public bool Preloaded = false;

        protected override void Awake()
        {
            if (!GifBytes)
            {
                Debug.LogError("UnityGif@" + name + ": GifBytes is null, Check GifBytes 请检查文件流");
                return;
            }

            base.Awake();

            if (Preloaded)
                Frames = GifUtil.GetFramesFromResources(GifBytes);
            else
                Frames = GifUtil.GetFrames(GifBytes);
        }
    }
}