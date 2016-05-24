using System;
using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.CameraInput
{
    public class CameraController : MonoBehaviour
    {
        private enum CameraState
        {
            Topdown,
            Angled
        }

        private Transform _transform; //camera tranform
        private CameraState _cameraState = CameraState.Angled;
        private float _zoomPos; //value in range (0, 1) used as t in Matf.Lerp



        //Control Variables
        private const float KeyboardMoveSpeed = 5f;
        private const float ScreenEdgeMovementSpeed = 3f;
        private const float FollowingSpeed = 10f;
        private const float RotationSpeed = 50f;
        private const float PanningSpeed = 5f;
        private const float MouseRotationSpeed = 50f;
        private const float ScreenEdgeBorder = 25f;
        private float _maxHeight = 10f;
        private float _minHeight = 15f;
        private const float HeightDampening = 5f;
        private const float KeyboardZoomingSensitivity = 3f;
        private const float ScrollWheelZoomingSensitivity = 60f;
        private  float _limitX = 15; //x limit of map
        private  float _limitY = 15; //z limit of map

        /// <summary>
        /// Follow Target Variables
        /// </summary>
        public Transform TargetFollow; //target to follow
        private bool _followingTarget;


        /// <summary>
        /// Assigned default controls
        /// </summary>
        private const  KeyCode ZoomInKey = KeyCode.Minus;
        private const  KeyCode ZoomOutKey = KeyCode.Equals;
        private const  KeyCode PanningKey = KeyCode.Mouse2;
        private const  KeyCode RotateRightKey = KeyCode.E;
        private const  KeyCode RotateLeftKey = KeyCode.Q;
        private const  KeyCode MouseRotationKey = KeyCode.Mouse1;
        private const  KeyCode ChangeCameraKey = KeyCode.Space;

        
        /// <summary>
        /// Handle zooming direction depending on button pressed
        /// </summary>
        private int ZoomDirection
        {
            get
            {
                var zoomIn = Input.GetKey(ZoomInKey);
                var zoomOut = Input.GetKey(ZoomOutKey);
                if (zoomIn && zoomOut)
                    return 0;
                if (!zoomIn && zoomOut)
                    return 1;
                if (zoomIn)
                    return -1;
                return 0;
            }
        }

        /// <summary>
        /// Determine rotation direction depending on button pressed
        /// </summary>
        private static int RotationDirection
        {
            get
            {
                var rotateRight = Input.GetKey(RotateRightKey);
                var rotateLeft = Input.GetKey(RotateLeftKey);
                if (rotateLeft && rotateRight)
                    return 0;
                if (rotateLeft)
                    return -1;
                if (rotateRight)
                    return 1;
                return 0;
            }
        }

        /// <summary>
        /// Initialising
        /// </summary>
        private void Start()
        {
            _transform = transform;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _cameraState = CameraState.Topdown;
                    ChangeCameraMode();
                    break;
                case "MapCreatorScene":

                    _cameraState = CameraState.Angled;
                    ChangeCameraMode();
                    break;
            }
        }

        /// <summary>
        /// Update camera movement and rotation 
        /// </summary>
        private void Update()
        {
            if(SceneManager.GetActiveScene().name == "Game" && GameManager.Instance.ReturnPauseManager().IsPaused()) return;
            _followingTarget = TargetFollow != null; //Check if a target exists and adjust the bool accordingly


            if (_followingTarget)
                FollowTarget();
            else
                Move();

            
            //Other Various Input
            if (Input.anyKeyDown && BattleManager.Instance.ReturnCurrentPlayer().IsUser())
                ResetTarget();
            if (Input.GetKeyDown(ChangeCameraKey))
                ChangeCameraMode();

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    _limitX = (BattleManager.Instance.ReturnMapSize()/2f) + 2;
                    _limitY = (BattleManager.Instance.ReturnMapSize()/2f) + 2;
                    LimitPosition();
                    break;
                case "MapCreatorManager":
                    _limitX = 30f;
                    _limitY = 30f;
                    LimitPosition();
                    break;
            }
            HeightCalculation();
            Rotation();
        }

        #region CAMERA MODE

        /// <summary>
        /// Determines the current Camera Mode (90 & 45 angle) and changes it to the other
        /// </summary>
        private void ChangeCameraMode()
        {
            switch (_cameraState)
            {
                case CameraState.Topdown:
                {
                        _cameraState = CameraState.Angled;
                        SetXRotation(45);
                    Debug.Log("Camera is Angled");

                    break;
                }
                case CameraState.Angled:
                {
                    _cameraState = CameraState.Topdown;
                    SetXRotation(90f);
                    Debug.Log("Camera is Topdown");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            SetCameraSettings();
        }


        /// <summary>
        /// Changes the camera's rotation to a particular angle
        /// </summary>
        /// <param name="angle"></param>Required Angle for camera to change into
        private void SetXRotation(float angle)
        {
            _transform.localEulerAngles = new Vector3(angle, _transform.localEulerAngles.y, _transform.localEulerAngles.z);
        }

        /// <summary>
        /// Change default camera settings depending on what mode it is on
        /// </summary>
        private void SetCameraSettings()
        {
            switch (_cameraState)
            {
                case CameraState.Topdown:
                {
                    SetHeight(4, 16);
                    break;
                }
                case CameraState.Angled:
                    SetHeight(3, 10);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetHeight(int max, int min)
        {
            _maxHeight = max;
            _minHeight = min;
        }

        #endregion

        #region ROTATION

        /// <summary>
        /// rotate camera
        /// </summary>
        private void Rotation()
        {
            switch (_cameraState)
            {
                case CameraState.Topdown:
                    {
                       transform.Rotate(Vector3.up, RotationDirection * Time.deltaTime * RotationSpeed, Space.World);
                        break;
                    }
                case CameraState.Angled:
                   transform.Rotate(Vector3.up, RotationDirection*Time.deltaTime*RotationSpeed, Space.World);
                    if (Input.GetKey(MouseRotationKey))
                        _transform.Rotate(Vector3.up, -ControlInput.MouseAxis.x*Time.deltaTime*MouseRotationSpeed, Space.World);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        #endregion

        #region MOVEMENT

        // Rotation we should blend towards.
        private Quaternion _targetRotation = Quaternion.identity;
        // Call this when you want to turn the object smoothly.
        public void SetBlendedEulerAngles(Vector3 angles)
        {
            _targetRotation = Quaternion.Euler(angles);
        }


        /// <summary>
        /// Move camera with keyboard or with screen edge
        /// </summary>
        private void Move()
        {

            if (_cameraState == CameraState.Angled)
            {
                SetBlendedEulerAngles(new Vector3(45,transform.localEulerAngles.y,0));
                // Turn towards our target rotation.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 50*Time.deltaTime);
            }

            //Movement with keys
            var desiredMove = new Vector3(ControlInput.KeyboardInput.x, 0, ControlInput.KeyboardInput.y);
            desiredMove *= KeyboardMoveSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f))*desiredMove;
            desiredMove = _transform.InverseTransformDirection(desiredMove);
            _transform.Translate(desiredMove, Space.Self);
            desiredMove = new Vector3();

            //Screne edge panning
            var leftRect = new Rect(0, 0, ScreenEdgeBorder, Screen.height);
            var rightRect = new Rect(Screen.width - ScreenEdgeBorder, 0, ScreenEdgeBorder, Screen.height);
            var upRect = new Rect(0, Screen.height - ScreenEdgeBorder, Screen.width, ScreenEdgeBorder);
            var downRect = new Rect(0, 0, Screen.width, ScreenEdgeBorder);
            desiredMove.x = leftRect.Contains(ControlInput.MouseInput) ? -1 : rightRect.Contains(ControlInput.MouseInput) ? 1 : 0;
            desiredMove.z = upRect.Contains(ControlInput.MouseInput) ? 1 : downRect.Contains(ControlInput.MouseInput) ? -1 : 0;
            desiredMove *= ScreenEdgeMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f))*desiredMove;
            desiredMove = _transform.InverseTransformDirection(desiredMove);
            _transform.Translate(desiredMove, Space.Self);


            if (!Input.GetKey(PanningKey) || ControlInput.MouseAxis == Vector2.zero) return;
            desiredMove = new Vector3(-ControlInput.MouseAxis.x, 0, -ControlInput.MouseAxis.y);

            desiredMove *= PanningSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f))*desiredMove;
            desiredMove = _transform.InverseTransformDirection(desiredMove);

            _transform.Translate(desiredMove, Space.Self);
        }

        #endregion

        #region CAMERALIMITS

        /// <summary>
        /// limit camera position
        /// </summary>
        private void LimitPosition()
        {
            if (_transform.position.y < 3)
                _transform.position = new Vector3(_transform.position.x, 3, _transform.position.z);
            _transform.position = new Vector3(Mathf.Clamp(_transform.position.x, -_limitX, _limitX), Mathf.Clamp(_transform.position.y, -200, 200), Mathf.Clamp(_transform.position.z, -_limitY, _limitY));
        }

        /// <summary>
        /// calcualte height
        /// </summary>
        private void HeightCalculation()
        {
           
            _zoomPos += ControlInput.ScrollWheel *Time.deltaTime*ScrollWheelZoomingSensitivity;
            _zoomPos += ZoomDirection*Time.deltaTime*KeyboardZoomingSensitivity;

            _zoomPos = Mathf.Clamp01(_zoomPos);

            var targetHeight = Mathf.Lerp(_minHeight, _maxHeight, _zoomPos);
            float difference = 0;

            

            _transform.position = Vector3.Lerp(_transform.position,
                new Vector3(_transform.position.x, targetHeight + difference, _transform.position.z),
                Time.deltaTime*HeightDampening);
        }

       

        #endregion

        #region FOLLOWTARGET

        /// <summary>
        /// follow targetif target != null
        /// </summary>
        private void FollowTarget()
        {
            switch (_cameraState)
            {
                case CameraState.Topdown:
                  
                    //Set Target Position
                    _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(TargetFollow.position.x, _transform.position.y - 1, TargetFollow.position.z),  Time.deltaTime * FollowingSpeed*2f);
                    break;
                case CameraState.Angled:                    
                    //Find rotative position
                    var targetRotation = Quaternion.LookRotation(TargetFollow.position - transform.position);
                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
                    transform.localEulerAngles = new Vector3(45, transform.localEulerAngles.y, 0);
                    //Set Target Position
                    var targetPos = TargetFollow.position;
                    if(Vector3.Distance(new Vector3(_transform.position.x,0,0), new Vector3(TargetFollow.position.x,0,0)) > 8)
                        _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(targetPos.x, targetPos.y, targetPos.z), Time.deltaTime * FollowingSpeed/2);
                    if (_transform.position == new Vector3(targetPos.x, targetPos.y, targetPos.z))
                    {
                        _followingTarget = false;
                    }
                        
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// set the target
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            _followingTarget = true;
            TargetFollow = target;
        }

        /// <summary>
        /// reset the target (target is set to null)
        /// </summary>
        private void ResetTarget()
        {
            _followingTarget = false;
            TargetFollow = null;
          
        }

        #endregion
    }
}