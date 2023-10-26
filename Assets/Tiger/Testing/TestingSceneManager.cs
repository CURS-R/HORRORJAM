using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR
{
    public class TestingSceneManager : MonoBehaviour
    {
        public GameObject TimerTimeClickPuzzle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Testing Target Time Click Puzzle
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(this.TimerTimeClickPuzzle);
            }
        }
    }
}
