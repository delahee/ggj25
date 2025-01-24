using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using static UnityEngine.GraphicsBuffer;

public class CurrencyJuice : MonoBehaviour
{
    public TMP_Text Currency;
    TMP_TextInfo Info;

    void FixedUpdate()
    {
        Currency.ForceMeshUpdate();
        
    }
    public void OnClick(int Target)
    {
        TextShaker(Target, new Vector3(0, 1, 0), 25, 3, 1.4f, false, false);
    }

    public void AllTextShake() 
    { 
        for(int i = 0; i < Currency.textInfo.characterCount; i++) 
        {
            TextShaker(i, new Vector3(0, 1, 0), 10, 3, 2f, false, false);
        }
    }
    public void TextShaker(int Target, Vector3 Direction, float Amplitude = 0f, int Frequency = 1, float Loss = 1f, bool Rotational = false, bool Fragmented = false)
    {
        StartCoroutine(TextShakerR(Target, Direction, Amplitude, Frequency, Loss, Rotational, Fragmented));
    }
    IEnumerator TextShakerR(int Target, Vector3 Direction, float Amplitude = 0f, int Frequency = 1, float Loss = 1f, bool Rotational = false, bool Fragmented = false)
    {
        Info = Currency.textInfo;
        var Vertices = Info.meshInfo[Info.characterInfo[Target].materialReferenceIndex].vertices;
        var OGVertices = Info.meshInfo[Info.characterInfo[Target].materialReferenceIndex].vertices;
        for (int a = 0; a < 4; a++) 
        {
            Vertices[Info.characterInfo[Target].vertexIndex +a] = Vertices[Info.characterInfo[Target].vertexIndex +a] + (Direction*Amplitude);
        }
        bool Rotationfollow = true;
        bool Fragmentfollow = true;
        for (float i = Amplitude; i > 1; i = i / Loss)
        {
            if (Rotational)
            {
                if (Rotationfollow)
                {
                    Direction.x = -Direction.x;
                    Rotationfollow = false;
                }
                else
                {
                    Direction.y = -Direction.y;
                    Rotationfollow = true;
                }
            }
            else if (Fragmented)
            {
                if (Fragmentfollow)
                {
                    Direction.x = -Direction.x;
                    Fragmentfollow = false;
                }
                else
                {
                    Direction = -Direction;
                    Fragmentfollow = true;
                }
            }
            else
            {
                Direction = -Direction;
            }
            for (int a = 0; a < 4; a++)
            {
                Vertices[Info.characterInfo[Target].vertexIndex + a] = OGVertices[Info.characterInfo[Target].vertexIndex + a];
            }
            Amplitude = Amplitude / Loss;
            for (int a = 0; a < 4; a++)
            {
                Vertices[Info.characterInfo[Target].vertexIndex +a] = Vertices[Info.characterInfo[Target].vertexIndex +a] + (Direction * Amplitude);
            }
            for (int b=0; b<Info.meshInfo.Length; b++)
            {
                var meshinfo = Info.meshInfo[b];
                meshinfo.mesh.vertices = meshinfo.vertices;
                Currency.UpdateGeometry(meshinfo.mesh, b);
            }
            for (int j = Frequency; j > 0; j--) yield return new WaitForFixedUpdate();
        }
        for (int a = 0; a < 4; a++)
        {
            Vertices[Info.characterInfo[Target].vertexIndex + a] = OGVertices[Info.characterInfo[Target].vertexIndex + a];
        }
        for (int b = 0; b < Info.meshInfo.Length; b++)
        {
            var meshinfo = Info.meshInfo[b];
            meshinfo.mesh.vertices = meshinfo.vertices;
            Currency.UpdateGeometry(meshinfo.mesh, b);
        }
    }
}
