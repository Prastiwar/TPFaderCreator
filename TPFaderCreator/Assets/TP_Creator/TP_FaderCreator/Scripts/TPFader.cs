using UnityEngine;
using UnityEngine.EventSystems;

namespace TP.Fader
{
    public class TPFader : MonoBehaviour, IPointerClickHandler
    {
        public enum FaderType
        {
            Alpha,
            Progress
        }

        public FaderType FadeType;
        public string FadeToScene;

        public delegate void OnFade();
        OnFade Fade;

        TPFaderCreator creator;

        void Awake()
        {
            creator = FindObjectOfType<TPFaderCreator>();
            if (Fade == null) Fade = () => creator.Fade(FadeToScene, FadeType);
        }

        public void SetOnFade(OnFade _onFade)
        {
            Fade = _onFade;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Fade();
        }
    }
}