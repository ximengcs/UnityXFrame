using UnityEngine;
using XFrame.Modules;

namespace Assets.Scripts
{
    public class GameObjectCom : Com
    {
        private GameObject m_Inst;

        protected override void OnInit(EntityData data)
        {
            m_Inst = new GameObject();
        }

        protected override void OnUpdate(float elapseTime)
        {

        }

        protected override void OnDestroy()
        {

        }
    }
}
