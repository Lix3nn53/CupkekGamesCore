#if UNITY_ADDRESSABLES
using UnityEngine;
using UnityEngine.UI;

namespace CupkekGames.Core
{
  public class SceneLoadTransitionCircleUI : FadeableMono
  {
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private CoreEventDatabase _coreEventDatabase;

    [SerializeField] private string _propertyName;
    private Image _image;

    protected override void Awake()
    {
      base.Awake();

      _image = transform.GetComponentInChildren<Image>();

      Fadeable.OnApply += OnApply;
      Fadeable.OnFadeOutStart += OnFadeOutStart;
      Fadeable.OnFadeOutComplete += OnFadeOutComplete;
      Fadeable.OnFadeInStart += OnFadeInStart;

      Fadeable.SetFadedOut();
    }

    private void OnApply()
    {
      _image.material.SetFloat(_propertyName, Fadeable.Value);
    }
    private void OnFadeOutStart()
    {
      _image.gameObject.SetActive(true);
    }
    private void OnFadeOutComplete()
    {
      _image.gameObject.SetActive(false);
    }
    private void OnFadeInStart()
    {
      _image.gameObject.SetActive(true);
    }
  }
}
#endif