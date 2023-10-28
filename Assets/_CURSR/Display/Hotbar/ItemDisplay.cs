using System;
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

        private Sprite _originalSprite;
        
        private void Awake()
        {
            _originalSprite = ItemSpriteImage.sprite;
        }
        
        private Item _itemBind;
        public Item itemBind
        {
            get
            {
                ItemSpriteImage.sprite = _itemBind == null ? _originalSprite : _itemBind.ItemSO.icon;
                return _itemBind;
            }
            set
            {
                _itemBind = value;
                ItemSpriteImage.sprite = _itemBind == null ? _originalSprite : _itemBind.ItemSO.icon;
            }
        }

        private bool _selectionVisibility;
        public bool SelectionVisibility
        {
            get
            {
                return _selectionVisibility;
            }
            set
            {
                SelectionVisual.SetActive(value);
                _selectionVisibility = value;
            }
        }
    }
}