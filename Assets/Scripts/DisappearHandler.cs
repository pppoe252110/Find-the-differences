using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisappearHandler : MonoBehaviour
{
    [SerializeField] public DisappearItem[] _items;
    [SerializeField] private Material _disappearMaterial;
    [SerializeField] private ParticleSystem _guessParticles;

    private LevelController _levelController;

    private List<DisappearItem> _guessedItems = new List<DisappearItem>();

    private void OnValidate()
    {
        if (_items == null || _items.Length <= 0)
        {
            _items = GetComponentsInChildren<DisappearItem>();
        }
    }

    public void Initialize()
    {
        CloneDeck();
        InitializeItems();
        SetNewMaterials();
    }

    public void OnGuessed(int index)
    {
        if (_guessedItems.Contains(_items[index]))
            return;

        _levelController.OnGuess(index);
    }

    public void InitializeItems()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            int index = i;

            _items[i].Initialize(this, index);
        }
    }

    public void SetNewMaterials()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].SetMaterial(new Material(_disappearMaterial));
        }
    }

    private void CloneDeck()
    {
        var original = Instantiate(this, transform.parent);

        original.transform.position += Vector3.down * 13.9f;
        original.HideAll();
        original.InitializeItems();
    }

    private void HideAll()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].Hide();
        }
    }

    public void SetLevelController(LevelController levelController)
    {
        _levelController = levelController;
    }

    internal void Guessed(int index)
    {
        _items[index].ProceedDisappearAnimation();

        Instantiate(_guessParticles, _items[index].transform.position, Quaternion.identity);

        _guessedItems.Add(_items[index]);
    }
}
