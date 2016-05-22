using Assets.Scripts.Menu;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class UserPlayer : Player {



        

        /// <summary>
        /// Update when a units turn begins (once)
        /// </summary>
        public override void TurnUpdate ()
        {
            if (PositionQueue.Count > 0)
            {
                transform.position += (PositionQueue[0] - transform.position).normalized*MoveSpeed*Time.deltaTime;
                if (Vector3.Distance(PositionQueue[0], transform.position) <= 0.1f)
                {
                    transform.position = PositionQueue[0];
                    PositionQueue.RemoveAt(0);
                    if (PositionQueue.Count == 0)
                        ActionPoints--;
                }
            }

            base.TurnUpdate();
        }
    }
}
