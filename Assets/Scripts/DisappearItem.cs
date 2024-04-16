using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Material _material;
    private DisappearHandler _handler;
    
    private int _index;
    
    private bool _guessed;

    private void OnValidate()
    {
        if(_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if(GetComponent<PolygonCollider2D>() == null)
            gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnMouseDown()
    {
        if (_guessed)
            return;
        
        _guessed = true;

        _handler.OnGuessed(_index);
    }

    public void ProceedDisappearAnimation()
    {
        if(_material)
            StartCoroutine(DisappearAnimation());
    }

    private IEnumerator DisappearAnimation()
    {
        float fill = 0.5f;
        while (fill < 1)
        {
            fill += Time.deltaTime;
            _material.SetFloat("_Amount", Mathf.Clamp01(fill));
            yield return null;
        }
        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        _spriteRenderer.color = Color.clear;
    }

    public void SetMaterial(Material material)
    {
        _material = material;
        _spriteRenderer.material = material;
    }
    
    public void Initialize(DisappearHandler handler, int index)
    {
        _index = index;
        _handler = handler;
    }
}
