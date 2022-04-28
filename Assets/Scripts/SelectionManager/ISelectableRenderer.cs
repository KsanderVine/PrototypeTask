using UnityEngine;

namespace DevTask.Selection
{
    public interface ISelectableRenderer
    {
        bool IsDestroyed { get; }

        Renderer GetRenderer();
        Transform GetTransform();
        Material GetDefaultMaterial();
    }
}