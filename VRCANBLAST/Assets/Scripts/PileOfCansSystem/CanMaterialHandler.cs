using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CanMaterialHandler : MonoBehaviour
{
    [SerializeField] MeshRenderer m_Renderer;
    [SerializeField] Material[] materialList;

    private void Awake()
    {
        List<Material> materials = new();

        materials.Add(m_Renderer.material);
        materials.Add(materialList[Random.Range(0, materialList.Length)]);

        m_Renderer.SetMaterials(materials);
    }
}
