using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CURSR
{
    public class TargetTimeClickPuzzle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _puzzleResult;
        [SerializeField] private TextMeshProUGUI _puzzlePrompt;
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private Image _timerSliderFill;
        public EventSystem _eventSystem;

        // Target Spawning
        [SerializeField] private List<TTCP_Target> Targets;
        private List<TTCP_Target> _currTargets;

        // Targets
        private int _numTargetsToSpawn;
        private int _activatedTargets;
        private bool _targetsInitialized;

        // Timer
        private float _puzzleTimer = 3.0f;
        private bool _isCountingDown;

        // Puzzle Solved
        private bool _isPuzzleSolved;
        public bool IsPuzzleSolved { get => this._isPuzzleSolved; }

        private void Awake()
        {
            // Check class dependencies.
            this.CheckDependencies();
        }

        private void Start()
        {
            this.SetupPuzzle();
        }

        private void Update()
        {
            if (this._isCountingDown)
            {
                this.DecreasePuzzleTimer();

                if (this._targetsInitialized && this._isPuzzleSolved == false && this.CheckAllTargetsGlicked())
                {
                    this._isPuzzleSolved = true;
                    this.PuzzleSolved();
                }
            }

            // Change value and color of timer slider.
            this._timerSlider.value = (3.0f - this._puzzleTimer) / 3.0f;
            this._timerSliderFill.color = Color.Lerp(Color.green, Color.red, this._timerSlider.value);
        }

        /// <summary>
        /// Counts down puzzle timer.
        /// </summary>
        private void DecreasePuzzleTimer()
        {
            this._puzzleTimer -= Time.deltaTime;

            // Time has run out
            if (this._puzzleTimer <= 0)
            {
                this._isCountingDown = false;
                this._isPuzzleSolved = false;
                this._puzzleResult.color = Color.red;
                this._puzzleResult.text = "FAIL!!!";
                Destroy(this.gameObject, 1.0f);
            }
        }

        /// <summary>
        /// Handles functionality for setting up the puzzle.
        /// </summary>
        private void SetupPuzzle()
        {
            this._currTargets = new List<TTCP_Target>();
            this._puzzleResult.text = string.Empty;
            this._puzzlePrompt.text = "Click all targets before the time runs out!".ToUpper();

            // Setting up Targets
            if (this.Targets.Count >= 6)
            {
                this._numTargetsToSpawn = UnityEngine.Random.Range(3, 6);
            }
            else
            {
                this._numTargetsToSpawn = UnityEngine.Random.Range(0, this.Targets.Count);
            }
            while (this._targetsInitialized == false)
            {
                this.SetupTargets();
            }

            // Start puzzle timer countdown
            this._isCountingDown = true;
        }

        /// <summary>
        /// Set up targtes for puzzle.
        /// </summary>
        private void SetupTargets()
        {
            if (this._activatedTargets == this._numTargetsToSpawn)
            {
                this._targetsInitialized = true;
                return;
            }

            int RandomIndex = Random.Range(0, this.Targets.Count);

            if (this._activatedTargets < this._numTargetsToSpawn && this.Targets[RandomIndex].IsAlive)
            {
                this.SetupTargets();
            }
            else if (this._activatedTargets < this._numTargetsToSpawn && this.Targets[RandomIndex].IsAlive == false)
            {
                this.Targets[RandomIndex].Activate();
                this._currTargets.Add(this.Targets[RandomIndex]);
                this._activatedTargets++;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>If all targets have been clicked.</returns>
        private bool CheckAllTargetsGlicked()
        {
            foreach (var target in this._currTargets)
            {
                if (target.IsAlive)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Handles functionality for when puzzle is solved.
        /// </summary>
        private void PuzzleSolved()
        {
            this._isCountingDown = false;

            this._puzzleResult.color = Color.green;
            this._puzzleResult.text = "SOLVED!!!";
            Destroy(this.gameObject, 1.0f);
        }

        /// <summary>
        /// Reset the puzzle.
        /// </summary>
        public void ResetPuzzle()
        {
            this.SetupPuzzle();
        }

        /// <summary>
        /// Check class dependencies.
        /// </summary>
        private void CheckDependencies()
        {
            if (this._puzzleResult == null)
            {
                Debug.LogError($"'PuzzleResult' reference missing. Maybe forgot to drag it in editor.");
            }

            if (this._puzzlePrompt == null)
            {
                Debug.LogError($"'PuzzlePrompt' reference missing. Maybe forgot to drag it in editor.");
            }

            if (this._timerSlider == null)
            {
                Debug.LogError($"'TimerSlider' reference missing. Maybe forgot to drag it in editor.");
            }

            if (this._timerSliderFill == null)
            {
                Debug.LogError($"'TimerSliderFill' reference is missing. Maybe forgot to drag it in editor.");
            }

            if (this.Targets.Count < 1)
            {
                Debug.LogError($"'Target' list is empty. There are no targets to spawn for this puzzle.");
            }

            if (this._eventSystem == null)
            {
                this._eventSystem = FindObjectOfType<EventSystem>();
                if (this._eventSystem == null)
                {
                    Debug.LogError($"There is no event system in the scene. The scene must have an EventSystem object for this puzzle to work properly. Try adding one. [UI > Event System]");
                }
            }
        }
    }
}
