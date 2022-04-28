using UnityEngine;
using System.Collections;

namespace DevTask.Selection
{
    interface ISelector
    {
        void CheckRay(Ray ray);
        ISelectableRenderer GetSelection();
    }
}
