using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_CollisionSFX_Param : MonoBehaviour {

    [SerializeField] private Enums.MaterialTypes m_MaterialType;

    public Enums.MaterialTypes GetMaterialType()
    {
        return m_MaterialType;
    }
}
