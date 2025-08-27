using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScreenView : MonoBehaviour
    {
        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
