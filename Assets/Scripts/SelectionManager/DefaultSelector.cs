using UnityEngine;
using System.Collections;

namespace DevTask.Selection
{
    public class DefaultSelector : MonoBehaviour, ISelector
    {
        private ISelectableRenderer _selected;

        public void CheckRay(Ray ray)
        {
            _selected = null;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out ISelectableRenderer selectable))
                {
                    _selected = selectable;
                }
            }
        }

        public ISelectableRenderer GetSelection()
        {
            return _selected;
        }
    }
}
