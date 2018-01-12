using UnityEngine;

namespace TP_Fader
{
    public class TPFader : MonoBehaviour
    {
        public int FadeToSceneIndex;
        TPFaderCreator creator;

        public delegate void OnFade();
        OnFade Fade;

        void Awake()
        {
            creator = FindObjectOfType<TPFaderCreator>();
            if (Fade == null) Fade = () => creator.Fade(FadeToSceneIndex);
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Fade());
        }

        public void SetOnFade(OnFade _onFade)
        {
            Fade = _onFade;
        }
    }
}