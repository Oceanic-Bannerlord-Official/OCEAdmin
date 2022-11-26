using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OCEAdmin.Patches
{
    public class PatchAddCosmeticItemsToEquipment
    {
        public static bool Prefix(ref Equipment equipment, Dictionary<string, string> choosenCosmetics)
        {
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
			{
				if (equipment[equipmentIndex].Item == null)
				{
					string key = equipmentIndex.ToString();
					switch (equipmentIndex)
					{
						case EquipmentIndex.NumAllWeaponSlots:
							key = "Head";
							break;
						case EquipmentIndex.Body:
							key = "Body";
							break;
						case EquipmentIndex.Leg:
							key = "Leg";
							break;
						case EquipmentIndex.Gloves:
							key = "Gloves";
							break;
						case EquipmentIndex.Cape:
							key = "Cape";
							break;
					}
					string text = null;
					if (choosenCosmetics != null)
					{
						choosenCosmetics.TryGetValue(key, out text);
					}
					if (text != null)
					{
						ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(text);
						EquipmentElement value = equipment[equipmentIndex];
						value.CosmeticItem = @object;
						equipment[equipmentIndex] = value;
					}
				}
				else if(IsModdedType(choosenCosmetics)) {
					string cosmetic;

					// if we find the slot key in cosmetics, output to replace
					if (choosenCosmetics.TryGetValue(equipmentIndex.ToString(), out cosmetic)) {
						EquipmentElement equipmentElement = equipment[equipmentIndex];

						if (cosmetic != "")
						{
							ItemObject itemObject = MBObjectManager.Instance.GetObject<ItemObject>(cosmetic);
							equipmentElement.CosmeticItem = itemObject;
							equipment[equipmentIndex] = equipmentElement;
						}
						else
						{
							equipment[equipmentIndex] = new EquipmentElement();
						}
					}
				}
				else
				{
					string stringId = equipment[equipmentIndex].Item.StringId; // name of the item in the slot
					string text2 = null;
					if (choosenCosmetics != null)
					{
						choosenCosmetics.TryGetValue(stringId, out text2); // if we find item name key, get value (which is the replacement)
					}
					if (text2 != null)
					{
						ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>(text2);
						EquipmentElement value2 = equipment[equipmentIndex];
						value2.CosmeticItem = object2;
						equipment[equipmentIndex] = value2;
					}
				}
			}

			return false;
        }

		private static string[] equipmentIndexes = { "NumAllWeaponSlots", "Body", "Cape", "Gloves", "Leg" };

		public static bool IsModdedType(Dictionary<string, string> choosenCosmetics)
        {
			if (choosenCosmetics == null)
				return false;

			foreach (KeyValuePair<string, string> entry in choosenCosmetics)
			{
				if(equipmentIndexes.Contains(entry.Key))
                {
					return true;
                }
			}

			return false;
        }
    }
}
