using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bluerant{
    public class BlueArchiveMaterialSetter : MonoBehaviour
    {
        // Assets 메뉴에 BlueArchiveMaterialSetter 항목을 추가한다
        [MenuItem("Assets/BlueArchiveMaterialSetter")]
        public static void Setup()
        {
            // 프로젝트 뷰에서 선택된 머테리얼을 가져온다
            List<Material> selectedMaterials = new List<Material>(Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets).OfType<Material>());

            // 머테리얼이 선택돼 있지 않으면 오류가 발생한다
            if (selectedMaterials.Count < 1)
            {
                Debug.LogWarning("No material selected.");
                return;
            }

            // 첫 머테리얼이 있는 폴더에 나중에 에셋을 저장한다
            string baseMatDir =
                Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedMaterials[0]));

            foreach (var mat in selectedMaterials)
            {
                Debug.Log("Material Name: " + mat.name);
                // 쉐이더를 "SimpleNiloToonURP"로 변경한다
                mat.shader = Shader.Find("SimpleURPToonLitExample(With Outline)");

                if (mat.name.Contains("_Face") || mat.name.Contains("_EyeMouth") || mat.name.Contains("_Eyebrow"))
                {
                    // 쉐이더에서 _IsFace 를 True로
                    mat.SetFloat("_IsFace", 1);
                }

                if (mat.name.Contains("_Hair") || mat.name.Contains("_Body") || mat.name.Contains("_Weapon") ||
                    mat.name.Contains("_Shield"))
                {
                    // Occulsion 셋팅
                    Texture2D occlusionMap = (Texture2D)AssetDatabase.LoadAssetAtPath(baseMatDir + "/Texture2D/" + mat.name + "_Mask.png", typeof(Texture2D));
                    if(occlusionMap != null)
                    {
                        mat.SetFloat("_UseOcclusion", 1);
                        
                        // occlusionMap Texture Type to Normal Map : 노멀맵은 아니지만 Linear 이미지기 때문에 텍스쳐 타입을 Normal Map으로 변경해준다
                        string path = AssetDatabase.GetAssetPath(occlusionMap);
                        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);
                        ti.textureType = TextureImporterType.NormalMap;
                        ti.SaveAndReimport();
                        
                        mat.SetTexture("_OcclusionMap",
                            (Texture2D)AssetDatabase.LoadAssetAtPath(baseMatDir + "/Texture2D/" + mat.name + "_Mask.png",
                                typeof(Texture2D)));
                        mat.SetVector("_OcclusionMapChannelMask", new Vector4(0, 1, 0, 0));
                        mat.SetFloat("_OcclusionRemapStart", 1);
                        mat.SetFloat("_OcclusionRemapEnd", 0.5f);
                    }
                    
                }
            }
            AssetDatabase.SaveAssets();
        }
    }
}

