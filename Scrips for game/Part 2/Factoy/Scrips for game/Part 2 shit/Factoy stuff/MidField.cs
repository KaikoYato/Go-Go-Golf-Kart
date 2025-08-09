using UnityEngine;

namespace Scrips_for_game.Part_2_shit.Factoy_stuff
{
    public class MidField : BaseAiclass
    { // da not so slow guys
        public override void ChangeStats()
        {
            agent.speed = 50f;
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.red;
            }
        }
        protected override void Start()
        {
            base.Start();
            ChangeStats();
        }
    }
}