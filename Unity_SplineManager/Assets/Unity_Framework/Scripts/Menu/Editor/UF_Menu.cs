using UnityEditor;
using UnityEngine;

namespace Unity_Framework.Scripts.Menu.Editor
{
    public class UF_Menu
    {
        [MenuItem("UF/Set selected object Dirty ", false, 2)]
        public static void SaveDataOnPlay()
        {
            foreach (Object _o in Selection.objects)
            {
                EditorUtility.SetDirty(_o);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
