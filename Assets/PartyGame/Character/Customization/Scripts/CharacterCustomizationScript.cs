using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationScript : MonoBehaviour
{

    [Header("Customization prefabs")]
    [SerializeField] private GameObject[] CharactersArray;
    [SerializeField] private GameObject[] ArmorArray;
    [SerializeField] private GameObject[] BackPacksArray;
    [SerializeField] private GameObject[] HatsArray;
    [SerializeField] private GameObject[] AccesoryArray;
    [SerializeField] private GameObject[] FacialHairArray;
    [SerializeField] private GameObject[] LeftPouchArray;
    [SerializeField] private GameObject[] RightPouchArray;

    [Header("Custom Materials")]
    [SerializeField] private Material[] materials01;
    [SerializeField] private Material[] materials02;
    [SerializeField] private Material[] materials03;
    [SerializeField] private Material[] materials04;

    private Dictionary<ENUM_CustomizationType, GameObject[]> customizationArrays = new Dictionary<ENUM_CustomizationType, GameObject[]>();
    private Dictionary<ENUM_CustomizationType, int> currentCustomizationIndices = new Dictionary<ENUM_CustomizationType, int>();
    private Dictionary<ENUM_MaterialType, Material[]> materialDictionary = new Dictionary<ENUM_MaterialType, Material[]>();

    private ENUM_MaterialType currentMaterialType = ENUM_MaterialType.Type1;
    private int currentSkinIndex = 0;

    public enum ENUM_CustomizationType
    {
        Character,
        Armor,
        Backpack,
        Hat,
        Accessory,
        FacialHair,
        LeftPouch,
        RightPouch,
    }

    public enum ENUM_MaterialType
    {
        Type1,
        Type2,
        Type3,
        Type4,
    }

    private void Start()
    {
        // Initialize arrays and indices
        customizationArrays[ENUM_CustomizationType.Character] = CharactersArray;
        customizationArrays[ENUM_CustomizationType.Armor] = ArmorArray;
        customizationArrays[ENUM_CustomizationType.Backpack] = BackPacksArray;
        customizationArrays[ENUM_CustomizationType.Hat] = HatsArray;
        customizationArrays[ENUM_CustomizationType.Accessory] = AccesoryArray;
        customizationArrays[ENUM_CustomizationType.FacialHair] = FacialHairArray;
        customizationArrays[ENUM_CustomizationType.LeftPouch] = LeftPouchArray;
        customizationArrays[ENUM_CustomizationType.RightPouch] = RightPouchArray;

        InitializeMaterials();

        // Initialize current indices
        foreach (ENUM_CustomizationType type in Enum.GetValues(typeof(ENUM_CustomizationType)))
        {
            currentCustomizationIndices[type] = 0;
        }

        // Initialize with the first character
        SetCustomization(ENUM_CustomizationType.Character, 0);
        SetCustomization(ENUM_CustomizationType.Armor, 0);
        SetCustomization(ENUM_CustomizationType.Backpack, 0);
        SetCustomization(ENUM_CustomizationType.Hat, 0);
        SetCustomization(ENUM_CustomizationType.Accessory, 0);
        SetCustomization(ENUM_CustomizationType.FacialHair, 0);
        SetCustomization(ENUM_CustomizationType.LeftPouch, 0);
        SetCustomization(ENUM_CustomizationType.RightPouch, 0);
        UpdateCharacterMaterials();
    }


    public void NavigateCustomization(ENUM_CustomizationType type, int direction)
    {
        int currentIndex = currentCustomizationIndices[type];
        int maxIndex = customizationArrays[type].Length - 1;

        // Calculate the new index with cyclical behavior
        int newIndex = (currentIndex + direction) % (maxIndex + 1);
        if (newIndex < 0)
        {
            newIndex = maxIndex;
        }

        SetCustomization(type, newIndex);
    }

    private void SetCustomization(ENUM_CustomizationType type, int index)
    {
        // Current index
        int currentIndex = currentCustomizationIndices[type];

        // Disable the current customization
        customizationArrays[type][currentIndex].SetActive(false);

        // Enable the new customization
        customizationArrays[type][index].SetActive(true);

        // Update the current index
        currentCustomizationIndices[type] = index;

        UpdateCharacterMaterials();
    }

    private void InitializeMaterials()
    {
        // Initialize the materials for each MaterialType (customize this according to your setup)
        materialDictionary[ENUM_MaterialType.Type1] = materials01;
        materialDictionary[ENUM_MaterialType.Type2] = materials02;
        materialDictionary[ENUM_MaterialType.Type3] = materials03;
        materialDictionary[ENUM_MaterialType.Type4] = materials04;
    }

 
    public void NavigateMaterialType(bool next)
    {
        // Cycle through MaterialType options
        int materialCount = Enum.GetValues(typeof(ENUM_MaterialType)).Length;
        int currentMaterialIndex = (int)currentMaterialType;

        if (next)
        {
            currentMaterialIndex = (currentMaterialIndex + 1) % materialCount;
        }
        else
        {
            currentMaterialIndex = (currentMaterialIndex - 1 + materialCount) % materialCount;
        }

        currentMaterialType = (ENUM_MaterialType)currentMaterialIndex;
        UpdateCharacterMaterials();
    }

    private void UpdateCharacterMaterials()
    {
        ENUM_CustomizationType type = ENUM_CustomizationType.Character;

        // Current index
        int currentIndex = currentCustomizationIndices[type];

        // Disable the current customization
        Renderer renderer = customizationArrays[type][currentIndex].GetComponent<Renderer>();

        // Retrieve the materials for the current MaterialType
        Material[] materials = materialDictionary[currentMaterialType];

        renderer.material = materials[currentSkinIndex];
    }


    public void NavigateSkinType(int direction)
    {
        int maxIndex = 2;

        // Calculate the new index with cyclical behavior
        int newIndex = (currentSkinIndex + direction) % (maxIndex + 1);
        if (newIndex < 0)
        {
            newIndex = maxIndex;
        }

        currentSkinIndex = newIndex;
        UpdateCharacterMaterials();
    }
}
