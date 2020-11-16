/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GifPlayer
{
    /// <summary>
    /// GIF序列帧 描述器
    /// </summary>
    public struct FrameImageDescriptor
    {
        /// <summary>
        /// 描述器 标志 分隔符 0x2C
        /// </summary>
        public byte Separator;

        /// <summary>
        /// 左边距离
        /// </summary>
        public ushort MarginLeft;

        /// <summary>
        /// 顶部距离
        /// </summary>
        public ushort MarginTop;

        /// <summary>
        /// 帧宽
        /// </summary>
        public ushort Width;

        /// <summary>
        /// 帧高
        /// </summary>
        public ushort Height;

        #region Packet Fields(1 byte)

        /// <summary>
        /// 是否包含局部色表（为真时使用局部色表,为假时使用全局色表）
        /// </summary>
        public bool LocalColorTableFlag;

        /// <summary>
        /// 是否包含交错
        /// </summary>
        public bool FlagInterlace;

        /// <summary>
        /// 重要颜色排序标志 一般为0 不作处理
        /// </summary>
        public bool FlagSort;

        /// <summary>
        /// 保留字段
        /// </summary>
        public int Reserved;

        /// <summary>
        /// 局部色表大小
        /// </summary>
        public int LocalColorTableSize;

        #endregion

        /// <summary>
        /// 局部色表
        /// </summary>
        public Color32[] LocalColorTable;

        /// <summary>
        /// 表示表示一个像素索引值所用的最少比特位数,如：该值为0x08时表示解码后的每个像素索引值为8 位（一个字节,可以表示256种颜色）。
        /// </summary>
        public byte LzwCodeSize;

        /// <summary>
        /// lzw压缩后的色号字节块
        /// </summary>
        public BytesBlock[] LzwPixelsBlocks;

        public FrameImageDescriptor(byte[] bytes, ref int byteIndex)
        {
            //描述器标识符 0x2c (1 byte)
            Separator = bytes[byteIndex];
            byteIndex++;

            //图像起始点 (2 byte)
            MarginLeft = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //图像起始点 (2 byte)
            MarginTop = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //图像宽度 (2 byte)
            Width = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            //图像高度 (2 byte)
            Height = BitConverter.ToUInt16(bytes, byteIndex);
            byteIndex += 2;

            #region Packet Fields (1 byte) //此处算法参照全局配置

            //是否使用局部色表 (1 bit)
            LocalColorTableFlag = bytes[byteIndex] >> 7 == 1;

            //是否包含交错 (1 bit)
            FlagInterlace = (bytes[byteIndex] >> 6) % 2 == 1;

            //重要颜色靠前标识 (1 bit)
            FlagSort = (bytes[byteIndex] >> 5) % 2 == 1;

            //保留字段 (2 bit)
            Reserved = (bytes[byteIndex] >> 3) % 4;

            //局部色表长度 (3 bit)
            int power = bytes[byteIndex] % 8 + 1;
            LocalColorTableSize = (int)Math.Pow(2, power);

            #endregion

            //指针下移
            byteIndex++;

            LocalColorTable = new Color32[LocalColorTableSize];
            //是否包含局部色表,计算方法参照全局色表
            if (LocalColorTableFlag)
            {
                for (int localColorIndex = 0; localColorIndex < LocalColorTableSize; localColorIndex++)
                {
                    LocalColorTable[localColorIndex] = new Color32(bytes[byteIndex], bytes[byteIndex + 1], bytes[byteIndex + 2], 255);
                    byteIndex += 3;
                }
            }

            //Lzw解码后 拆分成色号的单位bit长度(1 byte)
            LzwCodeSize = bytes[byteIndex];
            byteIndex++;

            var lzwPixelsBlocks = new List<BytesBlock>();
            //紧接着是表示色号的lzw压缩后的字节块 结构：长度+数据+长度+数据+...+结尾符
            while (true)
            {
                //字节块长度(1 byte)
                var blockSize = bytes[byteIndex];
                byteIndex++;

                //判断是否为结尾符
                if (blockSize == 0x00)
                    break;

                //初始化字节块
                var block = new BytesBlock();
                block.Size = blockSize;
                block.Bytes = new byte[block.Size];

                //字节块赋值
                for (var index = 0; index < block.Bytes.Length; index++)
                {
                    block.Bytes[index] = bytes[byteIndex];
                    byteIndex++;
                }

                lzwPixelsBlocks.Add(block);
            }

            LzwPixelsBlocks = lzwPixelsBlocks.ToArray();
        }

        /// <summary>
        /// 获取像素的色号集
        /// </summary>
        public byte[] GetColorIndexs()
        {
            //合并Lzw数据
            var lzwedBytes = new List<byte>();
            foreach (BytesBlock block in LzwPixelsBlocks)
                lzwedBytes.AddRange(block.Bytes);

            // Lzw解码
            var indexsLength = Height * Width;
            var indexs = LzwUtil.GetLzwDecodedBytes(lzwedBytes, LzwCodeSize, indexsLength);

            // 整理交错
            if (FlagInterlace)
                indexs = LzwUtil.GetInterlaceDecodedIndexs(indexs, Width);

            return indexs;
        }

        /// <summary>
        /// 局部色表中获取颜色
        /// </summary>
        public Color32[] GetColors32(FrameGraphicController controller, GraphicsInterchangeFormat gif)
        {
            var colorIndexs = GetColorIndexs();
            var colors32 = new Color32[colorIndexs.Length];
            for (int index = 0; index < colorIndexs.Length; index++)
            {
                var colorIndex = colorIndexs[index];
                //透明色
                if (controller.FlagTransparentColor && colorIndex == controller.TransparentColorIndex)
                {
                    colors32[index] = Color.clear;
                    continue;
                }

                //全局色
                if (!LocalColorTableFlag)
                {
                    colors32[index] = gif.GetColor32(colorIndex);
                    continue;
                }

                //异常
                if (colorIndex >= LocalColorTableSize)
                {
                    Debug.LogError("超出局部色表长度");
                    colors32[index] = Color.clear;
                    continue;
                }

                //局部色
                colors32[index] = LocalColorTable[colorIndex];
            }
            return colors32;
        }
    }
}
