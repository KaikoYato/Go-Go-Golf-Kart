using UnityEngine;

namespace Scrips_for_game.Part_2_shit.Factoy_stuff
{
    public class Top_racers : BaseAiclass
    { // da fast guys
        public override void ChangeStats()
        {
            agent.speed = 60f;
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.blue;
            }
        }

        protected override void Start()
        {
            base.Start();
            ChangeStats();
        }
    }
}