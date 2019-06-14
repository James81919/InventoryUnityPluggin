using UnityEngine;
using UnityEditor;

public class ItemCustomizer : EditorWindow
{
    // Collapsible booleans
    private bool ShowTextFont = true;
    private bool ShowQuality = false;

    // Text Font Variables
    public float NameTextSize = 16.0f;

    public float DescriptionTextSize = 14.0f;
    public string DescriptionTextColor = "lime";

    public float StatsTextSize = 14.0f;
    public string StatsTextColor = "white";
    public string StatsTextNegativeColor = "red";

    // Item Quality Variables
    public string Quality_Color_Common;
    public string Quality_Color_Uncommon;
    public string Quality_Color_Rare;
    public string Quality_Color_Epic;
    public string Quality_Color_Legendary;
    public string Quality_Color_Artifact;

    [MenuItem("InventorySystem/Item Stylizer")]
    public static void ShowWindow()
    {
        GetWindow<ItemCustomizer>("Item Stylizer");
    }

    public void Awake()
    {
        PlayerPrefs.GetFloat("InventoryNameTextSize", NameTextSize);
        PlayerPrefs.GetFloat("InventoryDescriptionTextSize", DescriptionTextSize);
        PlayerPrefs.GetString("IntentoryDescriptionTextColor", DescriptionTextColor);
        PlayerPrefs.GetFloat("InventoryStatsTextSize", StatsTextSize);
        PlayerPrefs.GetString("InventoryStatsTextColor", StatsTextColor);
        PlayerPrefs.GetString("InventoryStatsTextNegativeColor", StatsTextNegativeColor);

        PlayerPrefs.GetString("InventoryQualityColorCommon", Quality_Color_Common);
        PlayerPrefs.GetString("InventoryQualityColorUncommon", Quality_Color_Uncommon);
        PlayerPrefs.GetString("InventoryQualityColorRare", Quality_Color_Rare);
        PlayerPrefs.GetString("InventoryQualityColorEpic", Quality_Color_Epic);
        PlayerPrefs.GetString("InventoryQualityColorLegendary", Quality_Color_Legendary);
        PlayerPrefs.GetString("InventoryQualityColorArtifact", Quality_Color_Artifact);

        ResetVariablesToCurrentValues();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        ShowTextFont = EditorGUILayout.Foldout(ShowTextFont, "Text Font", EditorStyles.boldFont);
        if (ShowTextFont)
        {
            GUILayout.Label("   Name Text:", EditorStyles.boldLabel);
            NameTextSize = float.Parse(EditorGUILayout.TextField("    Size", NameTextSize.ToString()));

            GUILayout.Label("   Description Text:", EditorStyles.boldLabel);
            DescriptionTextSize = float.Parse(EditorGUILayout.TextField("    Size", DescriptionTextSize.ToString()));
            DescriptionTextColor = EditorGUILayout.TextField("    Color", DescriptionTextColor);

            GUILayout.Label("   Stats Text:", EditorStyles.boldLabel);
            StatsTextSize = float.Parse(EditorGUILayout.TextField("    Size", StatsTextSize.ToString()));
            StatsTextColor = EditorGUILayout.TextField("    Positive Color", StatsTextColor);
            StatsTextNegativeColor = EditorGUILayout.TextField("    Negative Value Color", StatsTextNegativeColor);

            EditorGUILayout.Space();
        }

        ShowQuality = EditorGUILayout.Foldout(ShowQuality, "Item Quality Colors", EditorStyles.boldFont);
        if (ShowQuality)
        {
            Quality_Color_Common = EditorGUILayout.TextField("    Common", Quality_Color_Common);
            Quality_Color_Uncommon = EditorGUILayout.TextField("    Uncommon", Quality_Color_Uncommon);
            Quality_Color_Rare = EditorGUILayout.TextField("    Rare", Quality_Color_Rare);
            Quality_Color_Epic = EditorGUILayout.TextField("    Epic", Quality_Color_Epic);
            Quality_Color_Legendary = EditorGUILayout.TextField("    Legendary", Quality_Color_Legendary);
            Quality_Color_Artifact = EditorGUILayout.TextField("    Artifact", Quality_Color_Artifact);
        }

        if (GUILayout.Button("Apply"))
        {
            PlayerPrefs.SetFloat("InventoryNameTextSize", NameTextSize);
            PlayerPrefs.SetFloat("InventoryDescriptionTextSize", DescriptionTextSize);
            PlayerPrefs.SetString("IntentoryDescriptionTextColor", DescriptionTextColor);
            PlayerPrefs.SetFloat("InventoryStatsTextSize", StatsTextSize);
            PlayerPrefs.SetString("InventoryStatsTextColor", StatsTextColor);
            PlayerPrefs.SetString("InventoryStatsTextNegativeColor", StatsTextNegativeColor);

            PlayerPrefs.SetString("InventoryQualityColorCommon", Quality_Color_Common);
            PlayerPrefs.SetString("InventoryQualityColorUncommon", Quality_Color_Uncommon);
            PlayerPrefs.SetString("InventoryQualityColorRare", Quality_Color_Rare);
            PlayerPrefs.SetString("InventoryQualityColorEpic", Quality_Color_Epic);
            PlayerPrefs.SetString("InventoryQualityColorLegendary", Quality_Color_Legendary);
            PlayerPrefs.SetString("InventoryQualityColorArtifact", Quality_Color_Artifact);
        }
    }

    private void ResetVariablesToCurrentValues()
    {
        NameTextSize = PlayerPrefs.GetFloat("InventoryNameTextSize");
        DescriptionTextSize = PlayerPrefs.GetFloat("InventoryDescriptionTextSize");
        DescriptionTextColor = PlayerPrefs.GetString("IntentoryDescriptionTextColor");
        StatsTextSize = PlayerPrefs.GetFloat("InventoryStatsTextSize");
        StatsTextColor = PlayerPrefs.GetString("InventoryStatsTextColor");
        StatsTextNegativeColor = PlayerPrefs.GetString("InventoryStatsTextNegativeColor");

        Quality_Color_Common = PlayerPrefs.GetString("InventoryQualityColorCommon");
        Quality_Color_Uncommon = PlayerPrefs.GetString("InventoryQualityColorUncommon");
        Quality_Color_Rare = PlayerPrefs.GetString("InventoryQualityColorRare");
        Quality_Color_Epic = PlayerPrefs.GetString("InventoryQualityColorEpic");
        Quality_Color_Legendary = PlayerPrefs.GetString("InventoryQualityColorLegendary");
        Quality_Color_Artifact = PlayerPrefs.GetString("InventoryQualityColorArtifact");
    }
}
