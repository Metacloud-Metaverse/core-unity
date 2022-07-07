using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Jackpot_Symbol",menuName ="ScriptableObject/Casino/Jackspot/New Jackspot Symbol")]
public class JackpotSymbol : ScriptableObject
{
	public GameObject prfab;
	public JackpotSymbolName symbolName;
	public SymbolType type;               
	public int frequency; 
	public List<JackpotSymbolReward> reward = new List<JackpotSymbolReward>();


	#region EditorVars
	public bool showRewardsEditor;
	public bool showFrecuencyEditor;
    #endregion
}
