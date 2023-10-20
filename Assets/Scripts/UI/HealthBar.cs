using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoEntity, IUpdatable
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthPrefab;
    [SerializeField] private Sprite _aliveSprite;
    [SerializeField] private Sprite _deadSprite;


    private readonly List<Image> images = new();

    private void Start()
    {
        _healthPrefab.gameObject.SetActive(false);
    }

    public UpdateOrder UpdateOrder => UpdateOrder.UI;
    public void UpdateManual()
    {
        ManageCount();
        for (var i = 0; i < images.Count; i++)
        {
            images[i].sprite = i < _health.Amount ? _aliveSprite : _deadSprite;
        }
    }

    private void ManageCount()
    {
        while (images.Count > _health.MaxHealth)
        {
            var toDestroy = images[images.Count];
            Destroy(toDestroy);
            images.RemoveAt(images.Count - 1);
        }

        while (images.Count < _health.MaxHealth)
        {
            var newPrefab = Instantiate(_healthPrefab, transform);
            newPrefab.gameObject.SetActive(true);
            images.Add(newPrefab);
        }
    }
}
