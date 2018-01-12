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
        public enum FaderType
        {
            Alpha,
            Progress
        }

        [System.Serializable]
        public struct TP_ProgressFade
        {
            public GameObject ProgressPrefab;
            public Slider LoadingBar;
            public Image LoadingImage;
            public Text LoadingText;
            public string LoadingTextString;
            public bool MustKeyToStart;
            public bool LoadingAnyKeyToStart;
            public KeyCode LoadingKeyToStart;
        }

        public FaderType FadeType;
        public bool IsFading = false;
        public float FadeSpeed;
        public Sprite FadeTexture;
        public Color FadeColor;
        public TP_ProgressFade ProgressFader;

        string fadeScene;
        Image Image;
        CanvasGroup Alpha;

        WaitForSeconds update = new WaitForSeconds(0.049f);
        WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

        void OnValidate()
        {
            Awake();
        }
        void Awake()
        {
            if (Alpha == null) Alpha = GetComponent<CanvasGroup>();
            if (Image == null) Image = GetComponent<Image>();
            Alpha.alpha = 0;
            Image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void FadeTo(string sceneName)
        {
            if (IsFading)
                return;

            IsFading = true;
            fadeScene = sceneName;

            switch (FadeType)
            {
                case FaderType.Alpha:
                    StartCoroutine(FadeAlpha());
                    break;
                case FaderType.Progress:
                    StartCoroutine(FadeProgress());
                    break;
                default:
                    break;
            }
        }

        IEnumerator FadeAlpha()
        {
            while (Alpha.alpha < 1)
            {
                yield return update;
                Alpha.alpha += FadeSpeed / 100;
            }
            StartCoroutine(FadeOut());
        }
        IEnumerator FadeOut()
        {
            SceneManager.LoadScene(fadeScene);

            yield return waitForEnd;

            Alpha.alpha = 1;
            while (Alpha.alpha > 0)
            {
                yield return update;
                Alpha.alpha -= FadeSpeed / 100;
                if(Alpha.alpha < 0.1f)
                    IsFading = false;
            }
        }

        IEnumerator FadeProgress()
        {
            Alpha.alpha = 1;
            GameObject layout = Instantiate(ProgressFader.ProgressPrefab, transform);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(fadeScene);
            
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if(ProgressFader.LoadingBar != null)
                    ProgressFader.LoadingBar.value = asyncLoad.progress;
                if (ProgressFader.LoadingImage != null)
                    ProgressFader.LoadingImage.fillAmount = asyncLoad.progress * 100;

                if (asyncLoad.progress >= 0.9f)
                {
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
            Destroy(layout);
            Alpha.alpha = 0;
        }

        //private static string[] ReadNames()
        //{
        //    List<string> temp = new List<string>();
        //    foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        //    {
        //        if (S.enabled)
        //        {
        //            string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
        //            name = name.Substring(0, name.Length - 6);
        //            temp.Add(name);
        //        }
        //    }
        //    return temp.ToArray();
        //}
    }

}