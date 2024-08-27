using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTabuCard", menuName = "TabuCard")]
public class TabuCard : ScriptableObject
{
    public string mainWord;
    public List<string> tabooWords = new List<string>();
}