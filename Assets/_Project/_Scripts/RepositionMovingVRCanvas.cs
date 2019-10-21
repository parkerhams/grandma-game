using UnityEngine;

namespace PixelCrushers.DialogueSystem.Demo
{

    /// <summary>
    /// Add this to the Dialogue Manager to reposition a world space canvas when
    /// a conversation starts, or call RepositionInFrontOfCamera manually to
    /// reposition it any time.
    /// </summary>
    public class RepositionMovingVRCanvas : MonoBehaviour
    {

        public Canvas canvas;
        public Vector3 offset = new Vector3(-1, 0, 2);
        public bool repositionOnConversationStart = true;

        private void OnConversationStart(Transform actor)
        {
            if (repositionOnConversationStart) RepositionInFrontOfCamera();
        }

        public void RepositionInFrontOfCamera()
        {
            var cameraTransform = Camera.main.transform;
            canvas.transform.position = cameraTransform.position + cameraTransform.forward * offset.z + cameraTransform.right * offset.x + Vector3.up * offset.y;
            canvas.transform.rotation = Quaternion.Euler(canvas.transform.rotation.eulerAngles.x, cameraTransform.rotation.eulerAngles.y, canvas.transform.rotation.eulerAngles.x);
        }

    }
}