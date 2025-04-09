using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core
{
	public class CharacterCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
	{
		[SerializeField] private Color _baseColor = Color.black;
		[SerializeField] private Color _hoverColor = Color.yellow;
		[SerializeField] private RectTransform _rect;
		[SerializeField] private Outline _outline;
		[SerializeField] private GameObject _frontSide;
		[SerializeField] private GameObject _backSide;
		[SerializeField] private Button _flipButton;
		[SerializeField] private TextMeshProUGUI _clanText;

		[Header("Spine data")]
		[SerializeField] private SkeletonGraphic _runeSpine;
		[SerializeField] private SkeletonGraphic _charSpine;
		[SerializeField, SpineAnimation(dataField = nameof(_runeSpine))]
		private string _runeHoverAnimation;
		[SerializeField, SpineAnimation(dataField = nameof(_charSpine))]
		private string _charHoverAnimation, _idleAnimation;

		[Header("Character data")]
		[SerializeField] private string _clanName;
		[SerializeField] private string _charName;
		[SerializeField, TextArea] private string _tooltipInfo;
		[SerializeField] private float _tooltipTimer = 4f;

		[Header("Flip animation data")]
		[SerializeField] private Vector3 _punchPower = Vector3.one;
		[SerializeField] private Vector3 _shakePower = Vector3.one;
		[SerializeField] private float _punchDuration = 0.1f;
		[SerializeField] private float _shakeDuration = 0.1f;

		private bool _isTooltipNeeded = false;
		private float _tooltipTime = 0f;

		private void OnEnable() => _flipButton.onClick.AddListener(OnFlipButtonClicked);

		private void OnDisable() => _flipButton.onClick.RemoveListener(OnFlipButtonClicked);

		private void Update()
		{
			if(_tooltipTime >= _tooltipTimer)
			{
				_tooltipTime = 0f;
				CursorUIMono.Instance.TooltipHandler(true, _tooltipInfo);
			}

			if(_isTooltipNeeded)
				_tooltipTime += Time.deltaTime;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_isTooltipNeeded = true;
			_runeSpine.enabled = true;
			Spine.AnimationState runeAnimationState = _runeSpine.AnimationState;
			Spine.AnimationState charAnimationState = _charSpine.AnimationState;

			charAnimationState.SetAnimation(0, _charHoverAnimation, false);
			charAnimationState.AddAnimation(0, _idleAnimation, true, 0f);
			runeAnimationState.SetAnimation(0, _runeHoverAnimation, false);
			runeAnimationState.AddEmptyAnimation(0, 0f, 0f);

			_outline.effectColor = _hoverColor;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_runeSpine.enabled = false;
			_isTooltipNeeded = false;
			_outline.effectColor = _baseColor;
			CursorUIMono.Instance.TooltipHandler(false);
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			_tooltipTime = 0f;
			CursorUIMono.Instance.TooltipHandler(false);
		}

		private void OnFlipButtonClicked()
		{
			bool isFrontActive = _frontSide.activeSelf;
			Sequence flipSequence = DOTween.Sequence(this);

			flipSequence.Append(_rect.DOPunchScale(_punchPower, _punchDuration))
						.Append(_rect.DOShakeRotation(_shakeDuration, _shakePower, randomnessMode: ShakeRandomnessMode.Harmonic));

			_frontSide.SetActive(!isFrontActive);
			_backSide.SetActive(isFrontActive);
			_clanText.text = isFrontActive ? _charName : _clanName;
		}
	}
}
