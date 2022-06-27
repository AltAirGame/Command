using System.Collections.Generic;
using UnityEngine;


namespace MHamidi
{
    public class GameManger : MonoBehaviour
    {
        
        //List of Work
        //Handel Player Input Such As Exit 
        //
        //
        //
        //
        //
        
        
        
        
        
        public List<Level> levels;
        [SerializeField] private RectTransform mechanicParrent;
        public GameButton ButtonPrefab;

        private void Start()
        {
         
          

           // StartLevel(0);
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
                var command = CommandManger.current.commandLookUpTable[item];
                button.SetListener(() => { CommandManger.current.AddToBuffer(command); });
                var icon = Resources.Load<Sprite>(command.name);
                button.SetIcon(icon);


                button.gameObject.name = command.name;
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