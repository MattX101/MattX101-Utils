using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class ExitApp : MonoBehaviour
    {
        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
