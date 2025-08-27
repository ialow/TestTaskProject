using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraFollowSystem : IEntitySystem
    {
        public CameraFollowSystemConfig Config { get; private set; }

        private Transform _cameraTransform;
        private Transform _targetTransform;

        public CameraFollowSystem(CameraFollowSystemConfig config)
        {
            Config = config;
        }

        public void Init(Entity entity)
        {
            if (entity is UnityEntity unityEntity)
            {
                _targetTransform = unityEntity.ViewTransform;
                _cameraTransform = Camera.main.transform;
            }
        }
        private Vector3 _velocity = Vector3.zero;

        public void Update()
        {
            if (_cameraTransform == null || _targetTransform == null) return;

            var targetPosition = new Vector3(_targetTransform.position.x, _cameraTransform.position.y, _targetTransform.position.z);
            _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, targetPosition, ref _velocity, Config.PositionLag);
        }
    }
}
