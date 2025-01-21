using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "GoogleSheetsConfig", menuName = "ScriptableObjects/GoogleSheetsConfig", order = 1)]
public class GoogleSheetsConfig : ScriptableObject
{
    public string sheetId = "1_WL9GG6i1DM4xD2r3xBirPIwI_-0doylXw6mB-DPLW0";
    public string apiKey = "AIzaSyAjdr-jaIv4XqMwCk5ND4_KbuOuFptbqhM";
    public string range = "currencies!A1:E100";
}
