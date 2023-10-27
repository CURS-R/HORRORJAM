using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class Camera : MonoBehaviour
    {
        private Transform target;
        internal void GiveTarget(Transform target) => this.target = target;

        private void Awake()
        {
            if (CameraManager.Instance.Camera == null && CameraManager.Instance.Camera != this)
                CameraManager.Instance.Camera = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (target != null)
                Anchor();
        }

        public void Anchor()
        {
            this.transform.SetPositionAndRotation(target.position, target.rotation);
        }
    }
}
