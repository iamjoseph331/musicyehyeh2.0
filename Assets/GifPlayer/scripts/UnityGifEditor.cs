/* code by 372792797@qq.com https://assetstore.unity.com/packages/2d/environments/gif-play-plugin-116943 */

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GifPlayer
{
    [CustomEditor(typeof(UnityGif))]
    public class UnityGifEditor : Editor
    {
        UnityGif _target;

        private void OnEnable()
        {
            if (!_target)
            {
                //Debug.Log("Getting UnityGif");
                _target = (UnityGif)target;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Preload to Resources 预加载到资源"))
            {
                GifUtil.PreloadToResources(_target.GifBytes);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Message", "Preloaded, please check the checkbox!\r\n\r\n预加载完毕，请勾选Preloaded!", "OK");
            }
        }
    }
}
#endif