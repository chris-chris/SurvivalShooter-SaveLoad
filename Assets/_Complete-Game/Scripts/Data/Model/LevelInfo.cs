using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo {
	
	public LevelInfo(int level, int exp)
	{
		this.Level = level;
		this.Exp = exp;
	}

	public int Level;
	public int Exp;
}
