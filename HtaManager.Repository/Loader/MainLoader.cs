using HtaManager.Infrastructure;
using HtaManager.Repository.Endpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Resolution;

namespace HtaManager.Repository.Loader
{
    public class MainLoader
    {
        IUnityContainer container;
        public event EventHandler LoadProgressed;

        List<Action> loaderFunctions = new List<Action>();
        public int StepCount
        {
            get => loaderFunctions.Count;
        }
            
        public MainLoader(IUnityContainer container)
        {
            this.container = container;
            loaderFunctions.Add(LoadEndpointDescriptorList);
        }

        public void Load()
        { 
            foreach (Action function in loaderFunctions)
            {
                function();
                LoadProgressed?.Invoke(this, null);
            }
        }

        private void LoadEndpointDescriptorList()
        {
            IEndpointRepository endpointRepo = container.Resolve<IEndpointRepository>(new ParameterOverride("connectionString", Constants.ConnectionString));
            endpointRepo.GetEndpointDescriptorList();
            
        }
    }
}
