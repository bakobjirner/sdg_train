using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;

public class Ragdoll : MonoBehaviour
{
    
    public SkinnedMeshRenderer character;
    public SkinnedMeshRenderer beard;
    public SkinnedMeshRenderer hair;
    public Texture[] availableHairAndBeards;
    public Texture[] availableCharacterVariants;
    private int textureIndexCharacter;
    private int textureIndexHairAndBeard;
    private Material characterMaterial;
    private Material hairAndBeardMaterial;

    void Start()
    {
        object[] data = this.GetComponent<PhotonView>().InstantiationData;
        textureIndexCharacter = (int)data[0];
        textureIndexHairAndBeard = (int)data[1];
        Debug.Log(textureIndexCharacter+" "+textureIndexHairAndBeard);
        GenerateMaterialAndSetToPlayer(textureIndexCharacter, textureIndexHairAndBeard);
    }

    void GenerateMaterialAndSetToPlayer(int textureIndexCharacter, int textureIndexHairAndBeard)
    {
        // Character Variation
        characterMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        characterMaterial.SetTexture("_BaseMap", availableCharacterVariants[textureIndexCharacter]);
        Material[] characterMaterials = new Material[]{characterMaterial};
        character.materials = characterMaterials;

        // Beard and Hair
        hairAndBeardMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        hairAndBeardMaterial.SetTexture("_BaseMap", availableHairAndBeards[textureIndexHairAndBeard]);
        Material[] hairAndBeardMaterials = new Material[]{hairAndBeardMaterial};
        beard.materials = hairAndBeardMaterials;
        hair.materials = hairAndBeardMaterials;
    }
}
