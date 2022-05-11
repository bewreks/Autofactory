using System.Collections.Generic;
using Crafting;
using Inventories;
using TMPro;
using UnityEngine;

namespace Windows.CraftingWindow
{
	public class InventoryPopup : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI      objectName;
		[SerializeField] private TextMeshProUGUI      time;
		[SerializeField] private InventoryPopupObject objectPrefab;

		private List<InventoryPopupObject> _objects = new List<InventoryPopupObject>();

		public void SetData(CraftingModel model)
		{
			objectName.text = model.CraftingResult.model.Type.ToString();
			time.text       = model.CraftingTime.ToString();
			foreach (var craftingNeed in model.CraftingNeeds)
			{
				if (craftingNeed.model.Type == InventoryObjectsTypesEnum.NOTHING) continue;

				var popupObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, transform)
												.GetComponent<InventoryPopupObject>();
				popupObject.gameObject.SetActive(true);
				popupObject.SetData(craftingNeed);
				_objects.Add(popupObject);
			}
		}

		public void Reset()
		{
			foreach (var popupObject in _objects)
			{
				Destroy(popupObject.gameObject);
			}
			_objects.Clear();
		}
	}
}