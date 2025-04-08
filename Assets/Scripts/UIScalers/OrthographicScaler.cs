using UnityEngine;

namespace Core
{
	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	public class OrthographicScaler : MonoBehaviour
	{
		[SerializeField] private Camera _camera = default;
		[SerializeField] private Vector2 _referenceResolution = new(1080, 1920);
		[Range(0.01f, 1), SerializeField] private float _resolutionScaleFactor = 0.1f;
		[Range(0, 1), SerializeField] private float _matchWidthOrHeight = 0;
		[SerializeField] private bool _checkXCameraPosition = false;
		[SerializeField] private bool _checkYCameraPosition = false;

		public void SetMatch(in float match)
		{
			_matchWidthOrHeight = match;
			Actualize();
		}

		public virtual void Actualize()
		{
			Vector2 referenceSize = _referenceResolution * _resolutionScaleFactor;
			Vector2 screenSize = new(Screen.width, Screen.height);
			Vector3 localPosition = transform.localPosition;

			float logWidth = Mathf.Log(screenSize.x / referenceSize.x, 2);
			float logHeight = Mathf.Log(screenSize.y / referenceSize.y, 2);
			float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, _matchWidthOrHeight);
			float orthoSize = 2 * Mathf.Pow(2, logWeightedAverage);

			_camera.orthographicSize = screenSize.y / orthoSize;

			if(_checkXCameraPosition) localPosition.x = screenSize.x / orthoSize;
			if(_checkYCameraPosition) localPosition.y = screenSize.y / orthoSize;

			transform.localPosition = localPosition;
		}

#if UNITY_EDITOR
		private void Update() => Actualize();
#endif
		private void Start() => Actualize();
	}
}
