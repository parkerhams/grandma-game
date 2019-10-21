using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrushers.DialogueSystem.Demo
{

    /// <summary>
    /// Add this to any world space or screen space - camera canvas to
    /// make it automatically assign the main camera to itself.
    /// </summary>
    public class AssignMainCameraToCanvas : MonoBehaviour
    {

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            AssignMainCamera();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            AssignMainCamera();
        }

        public void AssignMainCamera()
        {
            var canvas = GetComponent<Canvas>();
            if (canvas == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay || canvas.worldCamera != null) return;
            canvas.worldCamera = Camera.main;
        }
    }

}