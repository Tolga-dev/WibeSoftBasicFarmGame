using Managers;

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