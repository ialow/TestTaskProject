using UnityEngine;

namespace Game
{
    public class PlayerMovementSystem : IEntitySystem
    {
        private Transform _transform;
        private CharacterController _controller;
        private Vector3 _currentDirection;
        private Vector2 _currentScreenMousePosition;

        public PlayerMovementSystemConfig Config { get; private set; }

        public PlayerMovementSystem(PlayerMovementSystemConfig config)
        {
            Config = config;
        }

        public void Init(Entity entity)
        {
            if (entity is UnityEntity unityEntity)
            {
                _transform = unityEntity.ViewTransform;
                
                _controller = unityEntity.ViewTransform.GetComponent<CharacterController>();
                if (_controller == null)
                {
                    _controller = unityEntity.ViewTransform.gameObject.AddComponent<CharacterController>();
                }
            }

            if (Config.Input != null)
            {
                Config.Input.RotationEvent += OnRotateInput;
                Config.Input.MoveEvent += OnMoveInput;
            }
        }

        public void Update()
        {
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation,
                 Quaternion.Euler(0f, RotateTowardsCursor, 0f), Config.TurnDegrees * Time.deltaTime);

            var move = _transform.rotation * _currentDirection;
            _controller.Move(move * Config.Speed * Time.deltaTime);
        }

        private void OnMoveInput(Vector2 input)
        {
            _currentDirection = new Vector3(input.x, 0, input.y).normalized;
        }

        private void OnRotateInput(Vector2 input)
        {
            _currentScreenMousePosition = input;
        }

        private float RotateTowardsCursor
        {
            get
            {
                var playerPosXZ = new Vector2(_transform.position.x, _transform.position.z);
                var mausePos = GetMouseWorldPosition();
                var mousePosXZ = new Vector2(mausePos.x, mausePos.z);

                if (Vector2.Distance(mousePosXZ, playerPosXZ) > Config.IgnoringRadiusTurn)
                {
                    var differenceBetweenCursorAndPlayerPosition = mousePosXZ - playerPosXZ;
                    return Mathf.Atan2(differenceBetweenCursorAndPlayerPosition.x, differenceBetweenCursorAndPlayerPosition.y) * Mathf.Rad2Deg;
                }

                return _transform.eulerAngles.y;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            var ray = Camera.main.ScreenPointToRay(_currentScreenMousePosition);
            var plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);

            return Vector3.zero;
        }
    }
}
