using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPip", menuName = "Pip")]
public class Pip : ScriptableObject
{
    public Sprite sprite;
    public Color color;
    public PipType type;

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
