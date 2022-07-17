using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera;
    public Player player;
    public static GameManager instance;


    [Header("Dice Resources")]
    public Transform diceHomePoint;
    public GameObject pipPrefab;
    public List<diceSizePrefabs> dicePrefabsBySize = new List<diceSizePrefabs>();
    public List<Pip> pips = new List<Pip>();
    public List<int> sizeOptions
    {
        get
        {
            List<int> _sizeOptions = new List<int>();
            foreach (diceSizePrefabs diceSizePrefab in dicePrefabsBySize){
                _sizeOptions.Add(diceSizePrefab.size);
            }
            _sizeOptions.Sort((a, b) => a.CompareTo(b));
            return _sizeOptions;
        }
    }

    [ContextMenu("Awake")]
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public Pip getPipOfType(PipType type)
    {
        return pips.Find(x => x.type == type);
    }
    public GameObject getFacePrefab_OfSize(int size)
    {
        GameObject diceFacePrefab = dicePrefabsBySize.Find(x => x.size == size).facePrefab;
        if (diceFacePrefab == null) { Debug.LogWarning("Tried to get dice Body prefab of " + size + ". Could not find prefabs of that size"); }
        return diceFacePrefab;
    }
    public GameObject getDiceBodyPrefab_OfSize(int size)
    {
        GameObject diceBodyPrefab = dicePrefabsBySize.Find(x => x.size == size).bodyPrefab;
        if (diceBodyPrefab == null) { Debug.LogWarning("Tried to get dice Body prefab of " + size + ". Could not find prefabs of that size"); }
        return diceBodyPrefab;
    }
}
