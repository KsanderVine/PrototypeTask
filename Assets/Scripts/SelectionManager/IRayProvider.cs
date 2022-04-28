using UnityEngine;
using System.Collections;

namespace DevTask.Selection
{
    interface IRayProvider
    {
        Ray CreateRay();
    }
}