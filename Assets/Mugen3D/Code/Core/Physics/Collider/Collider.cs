using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public abstract class Collider : MonoBehaviour
    {
        public string tag;
        [HideInInspector]
        public Unit owner;
        public bool interactable = true;
        public Color color = Color.red;

        public abstract Geometry GetGeometry();
        public abstract bool IsHit(Ray ray, out RaycastHit hitResult);

        public void SetOwner(Unit owner)
        {
            this.owner = owner;
        }

        public void SetInteractable(bool interactable)
        {
            this.interactable = interactable;
        }
    }
}
