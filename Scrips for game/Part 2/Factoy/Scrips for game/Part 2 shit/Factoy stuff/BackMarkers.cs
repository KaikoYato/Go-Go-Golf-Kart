using UnityEngine;

namespace Scrips_for_game.Part_2_shit.Factoy_stuff
{
    public class BackMarkers : BaseAiclass
    { // da slow guys
        public override void ChangeStats()
        {
            agent.speed = 40f;
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.white;
            }
        }
        protected override void Start()
        {
            base.Start();
            ChangeStats();
        }
    }
}