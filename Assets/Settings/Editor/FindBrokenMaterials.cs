
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class FindBrokenMaterials
{
    [MenuItem("Tools/Find Broken Materials")]
    public static void Scan()
    {
        int count = 0;

        for (int s = 0; s < SceneManager.sceneCount; s++)
        {
            Scene scene = SceneManager.GetSceneAt(s);
            if (!scene.isLoaded) continue;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                Renderer[] renderers =
                    root.GetComponentsInChildren<Renderer>(true);

                foreach (Renderer renderer in renderers)
                {
                    Material[] materials = renderer.sharedMaterials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        Material mat = materials[i];

                        string issue = null;

                        if (mat == null)
                        {
                            issue = "Missing Material";
                        }
                        else if (mat.shader == null)
                        {
                            issue = "Missing Shader";
                        }
                        else if (!mat.shader.isSupported ||
                            mat.shader.name == "Hidden/InternalErrorShader")
                        {
                            issue = "Broken or Unsupported Shader";
                        }

                        if (issue != null)
                        {
                            Debug.LogWarning(
                                $"[{issue}] Object: {renderer.name} | " +
                                $"Slot: {i} | " +
                                $"Material: {(mat ? mat.name : "NULL")}",
                                renderer.gameObject
                            );

                            count++;
                        }
                    }
                }
            }
        }

        Debug.Log($"Scan completed. Found {count} issues.");
    }
}
