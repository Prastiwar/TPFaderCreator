using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace TP_Fader
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class TPFaderCreator : MonoBehaviour
    {
        [System.Serializable]
        public struct TP_ProgressFade
        {
            public GameObject ProgressPrefab;
            public Slider LoadingBar;
            public Image LoadingImage;
            public Text LoadingText;
            public string LoadingTextString;
            public float ProgressFadeSpeed;
            public bool MustKeyToStart;
            public bool LoadingAnyKeyToStart;
            public KeyCode LoadingKeyToStart;
        }

        public enum FaderType
        {
            Alpha,
            Progress
        }

        [HideInInspector] public TP_ProgressFade ProgressFader;
        [HideInInspector] public FaderType FadeType;
        [HideInInspector] public bool IsFading = false;
        [HideInInspector] public float FadeSpeed;
        [HideInInspector] public Sprite FadeTexture;
        [HideInInspector] public Color FadeColor;
        [HideInInspector] public List<GameObject> Faders;

        public delegate void BeforeSceneLoad();
        BeforeSceneLoad BeforeSceneIsLoaded;

        public delegate void OnFadeStarted();
        OnFadeStarted OnFade;

        [HideInInspector] [SerializeField] Image FadeImage;
        CanvasGroup Alpha;
        Canvas canvas;
        int FadeScene;

        WaitForSeconds update = new WaitForSeconds(0.049f);
        WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Refresh();
        }

        public void Refresh()
        {
            if (Alpha == null) Alpha = GetComponent<CanvasGroup>();
            if (FadeImage == null) FadeImage = GetComponent<Image>();
            if (canvas == null) canvas = GetComponent<Canvas>();

            FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            FadeImage.sprite = FadeTexture;
            FadeImage.color = FadeColor;
            FadeImage.raycastTarget = false;

            Alpha.interactable = false;
            Alpha.blocksRaycasts = false;

            canvas.enabled = false;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            if(canvas.sortingOrder <= 1)
                canvas.sortingOrder = 16;
        }

        public void Fade()
        {
            Fade(-1);
        }
        public void Fade(int sceneIndex)
        {
            if (IsFading)
                return;

            IsFading = true;
            FadeScene = sceneIndex;

            switch (FadeType)
            {
                case FaderType.Alpha:
                    StartCoroutine(Fade(true, Alpha, FadeSpeed));
                    break;
                case FaderType.Progress:
                    if (FadeScene < 0)
                    {
                        Debug.Log("Progress loading is available to scene only!");
                        break;
                    }
                    StartCoroutine(FadeProgress());
                    break;
                default:
                    break;
            }
        }

        IEnumerator Fade(bool IsAlpha, CanvasGroup _Alpha, float _FadeSpeed)
        {
            canvas.enabled = true;
            if (OnFade != null)
                OnFade();
            _Alpha.alpha = 0;
            while (_Alpha.alpha < 1)
            {
                yield return update;
                _Alpha.alpha += _FadeSpeed / 100;
            }
            if(IsAlpha)
                StartCoroutine(FadeOut(IsAlpha, _Alpha, _FadeSpeed));
        }
        IEnumerator FadeOut(bool IsAlpha, CanvasGroup _Alpha, float _FadeSpeed)
        {
            if(BeforeSceneIsLoaded != null)
                BeforeSceneIsLoaded();
            if (IsAlpha && FadeScene != -1)
                SceneManager.LoadScene(FadeScene);

            yield return waitForEnd;

            _Alpha.alpha = 1;
            while (_Alpha.alpha > 0)
            {
                yield return update;
                _Alpha.alpha -= _FadeSpeed / 100;
                if(_Alpha.alpha < 0.1f)
                    IsFading = false;
            }
            canvas.enabled = false;
        }

        IEnumerator FadeProgress()
        {
            canvas.enabled = true;
            if (OnFade != null)
                OnFade();
            FadeImage.enabled = false;
            Alpha.alpha = 1;

            GameObject layout = Instantiate(ProgressFader.ProgressPrefab, transform);
            CanvasGroup ProgressAlpha = layout.GetComponent<CanvasGroup>();
            StartCoroutine(Fade(false, ProgressAlpha, ProgressFader.ProgressFadeSpeed));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(FadeScene);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if(ProgressFader.LoadingBar != null)
                    ProgressFader.LoadingBar.value = asyncLoad.progress;
                if (ProgressFader.LoadingImage != null)
                    ProgressFader.LoadingImage.fillAmount = asyncLoad.progress * 100;

                if (asyncLoad.progress >= 0.9f)
                {
                    if(BeforeSceneIsLoaded != null)
                        BeforeSceneIsLoaded();
                    if (ProgressFader.LoadingBar != null)
                        ProgressFader.LoadingBar.value = 1f;
                    if (ProgressFader.LoadingImage != null)
                        ProgressFader.LoadingImage.fillAmount = 100;

                    ProgressFader.LoadingText.text = ProgressFader.LoadingTextString;

                    if (ProgressFader.MustKeyToStart)
                    {
                        if (!ProgressFader.LoadingAnyKeyToStart)
                        {
                            if (Input.GetKeyDown(ProgressFader.LoadingKeyToStart))
                                asyncLoad.allowSceneActivation = true;
                        }
                        else
                        {
                            if (Input.anyKeyDown)
                                asyncLoad.allowSceneActivation = true;
                        }
                    }
                    else
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
            StartCoroutine(FadeOut(false, ProgressAlpha, ProgressFader.ProgressFadeSpeed));
            yield return new WaitWhile(() => ProgressAlpha.alpha > 0);
            Destroy(layout);
            Alpha.alpha = 0;
            FadeImage.enabled = true;
            canvas.enabled = false;
        }

        public void SetOnFaderStarted(OnFadeStarted _OnFade)
        {
            OnFade = _OnFade;
        }

        public void SetBeforeSceneLoaded(BeforeSceneLoad _BeforeSceneIsLoaded)
        {
            BeforeSceneIsLoaded = _BeforeSceneIsLoaded;
        }
    }

}