using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AppearanceGenerator : MonoBehaviourPun
{
    public SkinnedMeshRenderer character;
    public SkinnedMeshRenderer beard;
    public SkinnedMeshRenderer hair;

    public Texture[] availableHairAndBeards;
    public Texture[] availableCharacterVariants;

    void Start() {
        if (photonView.IsMine)
        {
            // Character Variation
            var textureIndexCharacter = Random.Range(0, availableCharacterVariants.Length - 1);
            // Beard and Hair
            var textureIndexHairAndBeard = Random.Range(0, availableHairAndBeards.Length - 1);
            

            // Set Appearance Locally
            Debug.Log("SetAppearance locally: " + textureIndexCharacter + " " + textureIndexHairAndBeard);
            GenerateMaterialAndSetToPlayer(textureIndexCharacter, textureIndexHairAndBeard);
            // Set Appearance Remote
            photonView.RPC("SetAppearance", RpcTarget.AllBuffered, textureIndexCharacter, textureIndexHairAndBeard);
        }
    }

    [PunRPC]
    void SetAppearance(int textureIndexCharacter, int textureIndexHairAndBeard)
    {
        Debug.Log("SetAppearance remote: " + textureIndexCharacter + " " +  textureIndexHairAndBeard);
        GenerateMaterialAndSetToPlayer(textureIndexCharacter, textureIndexHairAndBeard);
    }

    void GenerateMaterialAndSetToPlayer(int textureIndexCharacter, int textureIndexHairAndBeard)
    {
        // Character Variation
        Material characterMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        characterMaterial.SetTexture("_BaseMap", availableCharacterVariants[textureIndexCharacter]);
        Material[] characterMaterials = new Material[]{characterMaterial};
        character.materials = characterMaterials;

        // Beard and Hair
        Material hairAndBeardMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        hairAndBeardMaterial.SetTexture("_BaseMap", availableHairAndBeards[textureIndexHairAndBeard]);
        Material[] hairAndBeardMaterials = new Material[]{hairAndBeardMaterial};
        beard.materials = hairAndBeardMaterials;
        hair.materials = hairAndBeardMaterials;
    }
}
