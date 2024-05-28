using UnityEngine;
using UnityEditor;

public class BatchRename : ScriptableObject
{
    [MenuItem("Tools/Batch Rename Items")]
    private static void RenameStones()
    {
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone"); 
        GameObject[] hemps = GameObject.FindGameObjectsWithTag("Hemp");
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        GameObject[] trees2 = GameObject.FindGameObjectsWithTag("Tree2");
        GameObject[] trees3 = GameObject.FindGameObjectsWithTag("Tree3");
        GameObject[] trees4 = GameObject.FindGameObjectsWithTag("Tree4");




        for (int i = 0; i < stones.Length; i++)
        {
            stones[i].name = $"Stone_{i + 1}";
            EditorUtility.SetDirty(stones[i]);
        }

        for (int i = 0; i < hemps.Length; i++)
        {
            hemps[i].name = $"Hemp_{i + 1}";
            EditorUtility.SetDirty(hemps[i]);
        }

        for (int i = 0; i < trees.Length; i++)
        {
            trees[i].name = $"Tree_{i + 1}";
            EditorUtility.SetDirty(trees[i]);
        }


        for (int i = 0; i < trees2.Length; i++)
        {
            trees2[i].name = "Tree 9";
            EditorUtility.SetDirty(trees2[i]);
        }

        for (int i = 0; i < trees3.Length; i++)
        {
            trees3[i].name = "Tree 7";
            EditorUtility.SetDirty(trees3[i]);
        }

        for (int i = 0; i < trees4.Length; i++)
        {
            trees4[i].name = "Tree 5";
            EditorUtility.SetDirty(trees4[i]);
        }
    }
}
