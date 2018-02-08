using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class CameraController : MonoBehaviour, Collideable
    {
        public float dumpRatio = 10;
        private Camera mCamera;
        public float yOffset = 2;
        private Transform mTarget1;
        private Transform mTarget2;

        private Rect mViewPortRect;
        private readonly int colliderWidth = 2;
        private RectCollider mLeftCollider = new RectCollider(new Rect(Vector2.zero, 2, 999), Rect.RIGHT);
        private RectCollider mRightCollider = new RectCollider(new Rect(Vector2.zero, 2, 999), Rect.LEFT);

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
            Vector3 newPos = new Vector3((mTarget1.position.x + mTarget2.position.x)/2, mTarget1.position.y + yOffset, transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime * dumpRatio);
            //transform.LookAt(mTarget.transform);
            CalcViewportRect();

        }

        private void CalcViewportRect()
        {
            float fileOfView = mCamera.fieldOfView;
            float h = Mathf.Tan(fileOfView / 2 / 180 * Mathf.PI) * Mathf.Abs(transform.position.z) * 2;
            float w = mCamera.aspect * h;
            mViewPortRect = new Rect(new Vector2(transform.position.x, transform.position.y), w, h);
            mLeftCollider.rect.position.x = mViewPortRect.position.x - w / 2 - 1;
            mRightCollider.rect.position.x = mViewPortRect.position.x + w / 2 + 1;
        }

        private void OnDrawGizmos()
        {
            if (mViewPortRect != null)
            {
                mViewPortRect.DrawGizmos(Color.black);
                mLeftCollider.DrawGizmos();
                mRightCollider.DrawGizmos();
            }
        }

        Collider[] Collideable.GetColliders()
        {
            return new Collider[] {mLeftCollider, mRightCollider};
        }
    }
}
