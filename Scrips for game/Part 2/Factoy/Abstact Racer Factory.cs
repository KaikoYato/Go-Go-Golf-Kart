using UnityEngine;

namespace Scrips_for_game.Part_2_shit.Factoy_stuff
{
    public abstract class Abstact_Racer_Factory : MonoBehaviour
    {// ABSTRACT FACTORY
        public abstract BaseAiclass CreateRacer1( Vector3 spawnPosition );
        public abstract BaseAiclass CreateRacer2( Vector3 spawnPosition );
        public abstract BaseAiclass CreateRacer3( Vector3 spawnPosition );
    }
}