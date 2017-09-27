using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
        private Camera mCamera;
        private Transform mTarget;

        public void SetFollowTarget(Transform t)
        {
            mTarget = t;
        }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            mCamera = this.GetComponent<Camera>();
        }

        void Update()
        {
            if (mTarget == null)
                return;
            Vector3 newPos = new Vector3(this.transform.position.x, mTarget.position.y, mTarget.position.z);
            newPos.y += 3;
            this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime*6);
        }
    }
}
