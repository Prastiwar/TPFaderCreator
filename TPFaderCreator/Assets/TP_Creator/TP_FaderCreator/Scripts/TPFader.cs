﻿using UnityEngine;

namespace TP_Fader
{
    public class TPFader : MonoBehaviour
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
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Fade());
        }

        public void SetOnFade(OnFade _onFade)
        {
            Fade = _onFade;
        }
    }
}