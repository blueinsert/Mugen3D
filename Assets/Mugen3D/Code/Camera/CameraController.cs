using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class CameraController : MonoBehaviour
    {
        public float dumpRatio = 10;
        private Camera mCamera;
        public float yOffset = 2;
        private Transform mTarget1;
        private Transform mTarget2;

        void Start()
        {
            mCamera = this.GetComponent<Camera>();
        }

        public void SetFollowTarget(Transform t1, Transform t2)
        {
            mTarget1 = t1;
            mTarget2 = t2;
        }

        void Update()
        {
            if (mTarget1 == null)
                return;
            Vector3 newPos = new Vector3(this.transform.position.x, mTarget1.position.y + yOffset, mTarget1.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime * dumpRatio);
            //transform.LookAt(mTarget.transform);
        }
    }
}
