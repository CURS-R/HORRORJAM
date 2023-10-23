using CURSR.Game;
using UnityEngine;
using UnityEngine.UI;

namespace CURSR.Display
{
    public class ItemDisplay : MonoBehaviour
    {
        [field:SerializeField] private Image ItemSpriteImage;
        [field:SerializeField] private GameObject SelectionVisual;
        
        private Sprite itemSprite;
        public Sprite ItemSprite
        {
            get => itemSprite;
            set
            {
                ItemSpriteImage.sprite = value == null ? default : itemSprite;
                itemSprite = value;
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