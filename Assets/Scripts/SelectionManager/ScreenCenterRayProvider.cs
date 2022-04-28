using UnityEngine;
using System.Collections;
using DevTask.Selection;

namespace DevTask.Selection
{
    public class ScreenCenterRayProvider : MonoBehaviour, IRayProvider
    {
        private Camera _raycastCamera;

        public void Awake()
        {
            _raycastCamera = Camera.main;
        }

        public Ray CreateRay()
        {
            Ray ray = _raycastCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            return ray;
        }
    }
}
