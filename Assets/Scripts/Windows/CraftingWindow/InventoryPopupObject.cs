using Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.CraftingWindow
{
	public class InventoryPopupObject : MonoBehaviour
	{
		[SerializeField] private Image           icon;
		[SerializeField] private TextMeshProUGUI objectName;
		[SerializeField] private TextMeshProUGUI count;

		public void SetData(CraftingNeed data)
		{
			icon.sprite     = data.model.Icon;
			objectName.text = data.model.Type.ToString();
			count.text      = data.count.ToString();
		}
	}
}