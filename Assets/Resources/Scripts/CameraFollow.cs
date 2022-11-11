using System;
using System.Collections;
using UnityEngine;

namespace Resources.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [Tooltip("If we need sometimes to turn off camera movement")]
        public bool hasFreezeMovement;

        [Header("Object to follow")]
        [SerializeField] private Transform target;

        [Header("Configuration")]
        [SerializeField] private Vector3 offsetFromTarget;

        [Tooltip("The axis which will be freezed")]
        [SerializeField] private Axis freezeAxis;
        
        private Vector3 _defaultPosition;
       // public bool targetChanged;
       // private Vector3 _newOffset;
        private Vector3 _prevOffset;
        [SerializeField] private float speedModifier;
        [SerializeField] private Vector3 targetOffsetPositionAtTruck = new Vector3(0, 15f, -2f);
        private Camera _mainCam;
       [SerializeField] private float targetFOV;
        public Vector3 OffsetFromTarget1 { get => offsetFromTarget; set => offsetFromTarget = value; }

        private void Awake()
        {
            // if (!target)
            // {
            //     target = GameObject.FindGameObjectWithTag(tagOfTarget).transform;
            // }
            _mainCam =Camera.main;
        }
        private void OnEnable()
        {
            _defaultPosition = gameObject.transform.position;
            _prevOffset = OffsetFromTarget1;
        }

        public void SetTarget(Transform targetObject)
        {
            this.target = targetObject;
            Debug.Log(this.target.name);
            SetFreezeMovement(false);
        }

        private void Update()
        {
            if (!hasFreezeMovement)
            {
                SetCameraAbove();
            }
        }

        private void SetCameraAbove()
        {
            if (!target) return;
            var newPosition = GetCameraPosition();
            FreezeCameraMovementAxis(ref newPosition);
            transform.position = newPosition;
                
            //transform.DOLookAt(_target.position, 0.5f);
        }

        public void RotateCam(float newXOffset,Quaternion newRot)
        {
            OffsetFromTarget1 = new Vector3(OffsetFromTarget1.x + newXOffset, OffsetFromTarget1.y, OffsetFromTarget1.z);

        }

        // private void UnFreezeMovement()
        // {
        //     OffsetFromTarget1 = _newOffset;
        // }

        private Vector3 GetCameraPosition()
        {
            if(_prevOffset != OffsetFromTarget1)
            {
                Vector3 newOffset = new Vector3(Mathf.Lerp(_prevOffset.x, OffsetFromTarget1.x, Time.deltaTime), OffsetFromTarget1.y, OffsetFromTarget1.z);
                _prevOffset = newOffset;
                return target.position + newOffset;
            }
            else
            {
                return target.position + OffsetFromTarget1;
            }
        }

        private void FreezeCameraMovementAxis(ref Vector3 position)
        {
            if (freezeAxis.x)
            {
                position.x = _defaultPosition.x;
            }

            if (freezeAxis.y)
            {
                position.y = _defaultPosition.y;
            }

            if (freezeAxis.z)
            {
                position.z = _defaultPosition.z;
            }
        }

        [Serializable]
        private struct Axis
        {
            public bool x;
            public bool y;
            public bool z;
        }

     

        private void SetOffset()
        {
            OffsetFromTarget1 = targetOffsetPositionAtTruck;
            hasFreezeMovement = false;
        }


        private void SetFreezeMovement(bool status) => hasFreezeMovement = status;
  


        private IEnumerator ChangeCamFov()
        {
            var tparam = 0f;
            while (tparam < 1)
            {
                _mainCam.fieldOfView = Mathf.Lerp(_mainCam.fieldOfView, targetFOV, tparam );
                tparam += Time.deltaTime * speedModifier;
                yield return null;
            }
        }
    }
}