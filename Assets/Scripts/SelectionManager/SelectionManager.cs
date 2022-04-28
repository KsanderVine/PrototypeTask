using UnityEngine;

namespace DevTask.Selection
{
    public class SelectionManager : MonoBehaviour
    {
        private IRayProvider _rayProvider;
        private ISelector _selector;
        private ISelection _selection;

        private ISelectableRenderer _currentSelected;

        public void Start()
        {
            _rayProvider = GetComponent<IRayProvider>();
            _selector = GetComponent<ISelector>();
            _selection = GetComponent<ISelection>();
        }

        public void Update()
        {
            if (_currentSelected != null && _currentSelected.IsDestroyed == false)
            {
                _selection.OnDeselect(_currentSelected);
            }
            
            _selector.CheckRay(_rayProvider.CreateRay());
            _currentSelected = _selector.GetSelection();

            if(_currentSelected != null && _currentSelected.IsDestroyed == false)
            {
                _selection.OnSelect(_currentSelected);
            }
        }
    }
}