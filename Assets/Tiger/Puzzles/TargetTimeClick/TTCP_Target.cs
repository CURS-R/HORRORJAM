using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CURSR
{
    public class TTCP_Target : MonoBehaviour
    {
        private bool _isAlive;
        public bool IsAlive { get => this._isAlive; }

        void Awake()
        {
            this.gameObject.SetActive(false);
            this._isAlive = false;
        }

        /// <summary>
        /// Activates the target for puzzle.
        /// </summary>
        public void Activate()
        {
            this.gameObject.SetActive(true);
            this._isAlive = true;
        }

        /// <summary>
        /// Handles functionality for when target is clicked.
        /// </summary>
        public void OnTargetClicked()
        {
            // Play Effets (particles, sound, etc)

            // Deactivate target
            this._isAlive = false;
            this.gameObject.SetActive(false);
        }
    }
}
