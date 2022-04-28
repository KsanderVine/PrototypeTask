using UnityEngine;

namespace DevTask.Selection
{
    public class HighlightSelection : MonoBehaviour, ISelection
    {
        public Color HighlightColor;

        public void OnSelect(ISelectableRenderer selectable)
        {
            Material material = selectable.GetRenderer().material;
            material.color = HighlightColor;
        }

        public void OnDeselect(ISelectableRenderer selectable)
        {
            Material material = selectable.GetRenderer().material;
            Material defaultMaterial = selectable.GetDefaultMaterial();
            material.color = defaultMaterial.color;
        }
    }
}