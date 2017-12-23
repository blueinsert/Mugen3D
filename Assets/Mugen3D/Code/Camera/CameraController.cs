using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class CameraController : MonoBehaviour
    {
        public float dumpRatio = 10;
        private Camera mCamera;
        private Transform mTarget;

        void Start()
        {
            mCamera = this.GetComponent<Camera>();
        }

        public void SetFollowTarget(Transform t)
        {
            mTarget = t;
        }

        void Update()
        {
            if (mTarget == null)
                return;
            Vector3 newPos = new Vector3(this.transform.position.x, mTarget.position.y, mTarget.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime * dumpRatio);
            transform.LookAt(mTarget.transform);
        }
    }
}
