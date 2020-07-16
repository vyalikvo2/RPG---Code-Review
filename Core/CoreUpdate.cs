using System;
using SP.Core.Data;
using UnityEngine;

namespace SP.Core
{
    public class CoreUpdate : MonoBehaviour
    {
        void Update()
        {
            RegenableStat.Update(Time.deltaTime);
        }
    }
}