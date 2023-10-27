using CURSR.Game;
using UnityEngine;
using UnityEngine.UI;

namespace CURSR.Display
{
    public class ItemDisplay : MonoBehaviour
    {
        [Header("Containers")]
        [field:SerializeField] private GameContainer gameContainer;
        
        [field:SerializeField] private Image ItemSpriteImage;
        [field:SerializeField] private GameObject SelectionVisual;

        private Item _itemBind;
        public Item itemBind
        {
            get => _itemBind;
            set
            {
                ItemSpriteImage.sprite = value == null ? default : value.ItemSO.icon;
                _itemBind = value;
            }
        }

        private bool _selectionVisibility;
        public bool SelectionVisibility
        {
            get => _selectionVisibility;
            set
            {
                SelectionVisual.SetActive(value);
                _selectionVisibility = value;
            }
        }
    }
}