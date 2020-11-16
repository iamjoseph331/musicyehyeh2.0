/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GifPlayer
{
    public static class GifUtil
    {
        private static Vector2 _pivotCenter = new Vector2(0.5f, 0.5f);

        public static Sprite GetSprite(this Texture2D texture)
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), _pivotCenter);
            sprite.name = texture.name;
            return sprite;
        }

        private static Dictionary<string, SequenceFrame[]> _cache = new Dictionary<string, SequenceFrame[]>();

        public static SequenceFrame[] GetFrames(TextAsset gifAsset)
        {
            try
            {
                if (_cache.ContainsKey(gifAsset.name))
                    return _cache[gifAsset.name];

                var startTime = DateTime.Now;

                //初始化GIF
                var gif = new GraphicsInterchangeFormat(gifAsset.bytes);

                //序列帧集初始化
                var frames = new SequenceFrame[gif.FrameImageDescriptors.Length];
                //初始化Texture
                var frameTexture = new Texture2D(gif.Width, gif.Height);

                //透明背景
                var transparentPixels = frameTexture.GetPixels32();
                for (var index = 0; index < transparentPixels.Length; index++)
                    transparentPixels[index] = Color.clear;

                //背景色
                var backgroundColor = gif.GetColor32(gif.BgColorIndex);
                var backgroundPixels = frameTexture.GetPixels32();
                for (var index = 0; index < backgroundPixels.Length; index++)
                    backgroundPixels[index] = backgroundColor;

                //记录下一帧的处理方法
                NextFrameDisposalMethod frameDisposalMethod = NextFrameDisposalMethod.Normal;
                bool previousReserved = false;

                //处理每个图块
                for (var frameIndex = 0; frameIndex < frames.Length; frameIndex++)
                {
                    //命名
                    frameTexture.name = "FrameOfIndex" + frameIndex;
                    //图像描述器
                    var frameImageDescriptor = gif.FrameImageDescriptors[frameIndex];
                    //绘图控制扩展
                    var frameGraphicController = gif.FrameGraphicControllers[frameIndex];

                    //上一帧控制器如果记录本帧的处理方法为bg，并且本帧的透明标识为true，那么背景替换为透明
                    if (frameDisposalMethod == NextFrameDisposalMethod.Bg && frameGraphicController.FlagTransparentColor)
                        //hack SetPixels is slower than SetPixels32
                        frameTexture.SetPixels32(transparentPixels);

                    //着色范围
                    var blockWidth = frameImageDescriptor.Width;
                    var blockHeight = frameImageDescriptor.Height;

                    var leftIndex = frameImageDescriptor.MarginLeft;//含
                    var rightBorder = leftIndex + blockWidth;//不含

                    var topBorder = gif.Height - frameImageDescriptor.MarginTop;//不含
                    var bottomIndex = topBorder - blockHeight;//含

                    //色表
                    var descriptorColors = frameImageDescriptor.GetColors32(frameGraphicController, gif);
                    //色表指针
                    var colorIndex = -1;
                    //gif的y是从上往下，texture的y是从下往上
                    for (var y = topBorder - 1; y >= bottomIndex; y--)
                    {
                        for (var x = leftIndex; x < rightBorder; x++)
                        {
                            colorIndex++;
                            //判断是否保留像素
                            if (previousReserved && descriptorColors[colorIndex].a == 0)
                                continue;
                            frameTexture.SetPixel(x, y, descriptorColors[colorIndex]);
                        }
                    }

                    //保存
                    frameTexture.wrapMode = TextureWrapMode.Clamp;
                    frameTexture.Apply();

                    //添加序列帧,并兵初始化Texture
                    var spriteFrame = frameTexture.GetSprite();
                    frames[frameIndex] = new SequenceFrame(spriteFrame, frameGraphicController.DelaySecond);
                    frameTexture = new Texture2D(gif.Width, gif.Height);

                    //预处理下一帧图像
                    previousReserved = false;
                    switch (frameGraphicController.NextFrameDisposalMethod)
                    {
                        //1 - Do not dispose. The graphic is to be left in place. 
                        //保留此帧
                        case NextFrameDisposalMethod.Last:
                            frameTexture.SetPixels32(frames[frameIndex].Texture.GetPixels32());
                            previousReserved = true;
                            break;

                        //2 - Restore to background color. The area used by the graphic must be restored to the background color. 
                        //还原成背景色
                        case NextFrameDisposalMethod.Bg:
                            frameTexture.SetPixels32(backgroundPixels);
                            break;

                        //3 - Restore to previous. The decoder is required to restore the area overwritten by the graphic with what was there prior to rendering the graphic.
                        //还原成上一帧
                        case NextFrameDisposalMethod.Previous:
                            frameTexture.SetPixels32(frames[frameIndex - 1].Texture.GetPixels32());
                            previousReserved = true;
                            break;
                    }
                    frameDisposalMethod = frameGraphicController.NextFrameDisposalMethod;
                }

                Debug.Log(string.Format("Analyzing gif {0} with {1} frames costs {2} seconds ", gifAsset.name, frames.Length,
                    (DateTime.Now - startTime).TotalSeconds.ToString("0.000")));
                _cache.Add(gifAsset.name, frames);
                return frames;
            }
            catch (Exception ex)
            {
                var logBuilder = new StringBuilder();
                logBuilder.AppendLine("GIF解析发生错误/Gif analysed error");
                logBuilder.AppendLine("可能是版本不兼容导致，请用PhotoShop 存储为web所用格式GIF 后重试/Mybe the version invalid , try to convert it by photoshop web format gif");
                logBuilder.Append(ex.Message);
                Debug.LogError(logBuilder.ToString());
                return null;
            }
        }

        public static void FileWrite(string path, byte[] bytes)
        {
            var stream = File.OpenWrite(path);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        private const string _resourceName = "{0}/{1}";
        private const string _resourcePath = "GifPlayer/resources/" + _resourceName;

        public static void PreloadToResources(TextAsset gifAsset)
        {
            var savePath = Application.dataPath + "/" + _resourcePath;
            var saveFolder = string.Format(savePath, gifAsset.name, "");
            Directory.CreateDirectory(saveFolder);

            var frames = GetFrames(gifAsset);
            var delaySecondsArray = string.Join(",", frames.Select(m => m.DelaySeconds.ToString()).ToArray());
            for (var index = 0; index < frames.Length; index++)
                FileWrite(string.Format(savePath, gifAsset.name, index + ".png"), frames[index].Sprite.texture.EncodeToPNG());
            FileWrite(string.Format(savePath, gifAsset.name, "delays.txt"), Encoding.UTF8.GetBytes(delaySecondsArray));

            Process.Start(saveFolder);
        }

        public static SequenceFrame[] GetFramesFromResources(TextAsset gifAsset)
        {
            if (_cache.ContainsKey(gifAsset.name))
                return _cache[gifAsset.name];

            var delaysSecondsAsset = Resources.Load<TextAsset>(string.Format(_resourceName, gifAsset.name, "delays"));
            if (!delaysSecondsAsset)
            {
                Debug.LogError("Please make sure that gif has preloaded.");
                return null;
            }
            var delaysSeconds = delaysSecondsAsset.text.Split(',');
            var frames = new SequenceFrame[delaysSeconds.Length];
            for (var index = 0; index < frames.Length; index++)
            {
                var sprite = Resources.Load<Texture2D>(string.Format(_resourceName, gifAsset.name, index)).GetSprite();
                frames[index] = new SequenceFrame(sprite, float.Parse(delaysSeconds[index]));
            }

            _cache.Add(gifAsset.name, frames);
            return frames;
        }
    }
}