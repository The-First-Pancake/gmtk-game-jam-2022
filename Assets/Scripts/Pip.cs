using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPip", menuName = "Pip")]
public class Pip : ScriptableObject
{
    public Sprite sprite;
    public Color color;
    public PipType type;
    public float rarity = 10;
    public Vector2 abundance = new Vector2(1, 4);
}
public enum PipType
{
    Bullet,
    Gun,
    Energy,
    Fire,
    Evade,
    Search
}
