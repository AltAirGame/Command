using System.Collections.Generic;
using UnityEngine;


namespace MHamidi
{


    public class GameManger : MonoBehaviour
    {
        public List<Level> levels;
        [SerializeField] private RectTransform mechanicParrent;
        public GameButton ButtonPrefab;

        private void Start()
        {
            levels = new List<Level>();
            var commandList = new List<ICommand>();
            commandList.Add(new MoveCommand());
            commandList.Add(new TurnLeftCommand());
            commandList.Add(new TurnRightCommand());
            commandList.Add(new InteractCommand());
            commandList.Add(new JumpCommand());
            // levels.Add(new Level(commandList, 8));

            StartLevel(0);
        }


        private void StartLevel(int i)
        {
            //SetUp the Level By Level Object 
            //The Ui Must be Update By Level Detail
            //Each Level has it's Own Buffer Size
            //Each level has A P1 Buffer
            //Each Level Has A P2 Buffer


            foreach (var item in levels[i].AvailableCommand)
            {
                var button = Instantiate(ButtonPrefab, mechanicParrent);
                button.SetListener(() => { CommandManger.current.AddToBuffer(item); });
                var icon = Resources.Load<Sprite>(item.name);
                button.SetIcon(icon);


                button.gameObject.name = item.name;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                CommandManger.current.Rewind();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                CommandManger.current.Play();
            }
        }
    }
}