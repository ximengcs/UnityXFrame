using UnityEngine;
using XFrame.Modules.Entities;

namespace UnityXFrame.Core.Entities
{
    public class GameObjectCom : Com
    {
        private GameObject m_Inst;

        public Transform Tf => m_Inst.transform;

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

        protected override void OnDelete()
        {

        }
    }
}
