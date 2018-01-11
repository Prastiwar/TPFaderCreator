using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TP_Fader
{
    public class TPFaderCreator : MonoBehaviour
    {
        public Texture2D FadeTexture;

        public void FadeTo(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}