using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	[RequireComponent(typeof(RectTransform))]
	public class CursorUIMono : MonoBehaviour
	{
		[SerializeField] private Canvas _targetCanvas;
		[SerializeField] private RectTransform _cursorRect;
		[SerializeField] private CanvasGroup _tooltipGroup;
		[SerializeField] private TextMeshProUGUI _tooltipText;

		[Header("Cursor Settings")]
		[SerializeField] private Vector2 _hotspotOffset = Vector2.zero;
		[SerializeField] private bool _autoCenter = true;
		[SerializeField, Range(0, 1)] private float _smoothing = 0.1f;

		[Header("Tooltip Animation data")]
		[SerializeField] private float _fadeDuration = 0.2f;

		private static CursorUIMono _instance;

		public static CursorUIMono Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = FindFirstObjectByType<CursorUIMono>();

					if(_instance == null)
					{
						GameObject obj = new("CursorUI");
						_instance = obj.AddComponent<CursorUIMono>();
					}
				}
				return _instance;
			}
		}

		private CanvasScaler _canvasScaler;
		private Vector2 _currentPosition;
		private Camera _canvasCamera;

		private void Awake()
		{
			if(_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;

			if(transform.parent == null)
				DontDestroyOnLoad(gameObject);

			Initialize();
		}

		private void Update() => UpdateCursorPosition();

		private void OnDestroy() => Cursor.visible = true;

		public void TooltipHandler(bool show, string text = default)
		{
			if(_tooltipGroup.isActiveAndEnabled == show)
				return;

			float alpha = show ? 1 : 0;
			_tooltipText.text = text;
			_tooltipGroup.gameObject.SetActive(show);
			_tooltipGroup.DOFade(alpha, _fadeDuration);
		}

		private void Initialize()
		{
			if(_targetCanvas == null)
				FindParentCanvas();

			_canvasScaler = _targetCanvas.GetComponent<CanvasScaler>();
			_canvasCamera = _targetCanvas.worldCamera;

			Cursor.visible = false;

			if(_autoCenter)
			{
				_hotspotOffset = new Vector2(
					_cursorRect.rect.width * _cursorRect.pivot.x,
					_cursorRect.rect.height * _cursorRect.pivot.y
				);
			}
		}

		private void UpdateCursorPosition()
		{
			Vector2 mousePosition = Input.mousePosition;
			Vector2 targetPosition = ConvertScreenPosition(mousePosition);

			_currentPosition = Vector2.Lerp(
				_currentPosition,
				targetPosition - _hotspotOffset,
				Time.unscaledDeltaTime / _smoothing
			);

			_cursorRect.anchoredPosition = _currentPosition;
		}

		private Vector2 ConvertScreenPosition(Vector2 screenPosition)
		{
			if(_targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				float scaleFactor = _canvasScaler.scaleFactor;
				return screenPosition / scaleFactor;
			}

			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				_targetCanvas.GetComponent<RectTransform>(),
				screenPosition,
				_canvasCamera,
				out Vector2 localPoint
			);

			return localPoint;
		}

		private void FindParentCanvas()
		{
			_targetCanvas = GetComponentInParent<Canvas>();

			if(_targetCanvas == null)
			{
				_targetCanvas = FindFirstObjectByType<Canvas>();
				Debug.LogWarning("UICursor: Auto-assigned Canvas to " + _targetCanvas.name);
			}

			if(_targetCanvas == null)
				Debug.LogError("UICursor: No Canvas found in the scene!");
		}
	}
}
