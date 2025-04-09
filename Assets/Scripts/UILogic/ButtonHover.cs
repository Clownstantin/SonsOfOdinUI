using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
	public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] protected Color baseColor = Color.white;
		[SerializeField] protected Color hoverColor = Color.yellow;
		[SerializeField] protected Image icon;

		public void OnPointerEnter(PointerEventData eventData) => OnEnter();

		public void OnPointerExit(PointerEventData eventData) => OnExit();

		protected virtual void OnEnter() => icon.color = hoverColor;
		protected virtual void OnExit() => icon.color = baseColor;
	}
}
