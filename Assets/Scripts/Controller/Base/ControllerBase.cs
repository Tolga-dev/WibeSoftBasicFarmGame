using Managers;
using Managers.Base;

namespace Controller.Base
{
    public class ControllerBase
    {
        protected ManagerBase ManagerBase;

        public virtual void Initialization(ManagerBase managerBaseVal)
        {
            ManagerBase = managerBaseVal;
        }
    }
}