using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] protected int Capacity;

    private List<Stone> _stones = new List<Stone>();

    protected void Initialize(List<Stone> stones)
    {
        for (int i = 0; i < Capacity; i++)
        {
            int randomIndex = Random.Range(0, stones.Count);
            Stone spawned = Instantiate(stones[randomIndex], _container.transform);

            spawned.gameObject.SetActive(false);
            _stones.Add(spawned);
        }
    }

    protected bool TryGetObject(out Stone result)
    {
        int randomIndex = Random.Range(0, _stones.Count);

        result = _stones.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result != null;
    }
}
