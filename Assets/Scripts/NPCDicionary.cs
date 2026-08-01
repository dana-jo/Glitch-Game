using System.Collections.Generic;
using UnityEngine;

public class NPCDicionary : MonoBehaviour
{
    public static NPCDicionary Instance { get; private set; }

    public List<NPC> NPCs;
    private Dictionary<int, GameObject> NPCsDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        NPCsDictionary = new Dictionary<int, GameObject>();
        for (int i = 0; i < NPCs.Count; i++)
        {
            if (NPCs[i] != null)
            {
                NPCs[i].npcID = i + 1;
            }
        }

        foreach (NPC npc in NPCs)
        {
            NPCsDictionary[npc.npcID] = npc.gameObject;
        }
    }

    public GameObject GetNPC(int npcId)
    {
        NPCsDictionary.TryGetValue(npcId, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"npc with ID {npcId} not found in dictionary");

        }
        return prefab;

    }
}
