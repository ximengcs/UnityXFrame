using XFrame.Core;
using XFrame.Modules.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XFrame.Modules.Diagnotics;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace UnityXFrame.Core.Resource
{
    [CoreModule]
    public class HotUpdateModule : SingletonMono<HotUpdateModule>
    {
        public ITask CheckScript()
        {
            return new EmptyTask();
        }

        public ITask UpdateScript()
        {
            return new EmptyTask();
        }

        public void UpdateResource()
        {
            AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(true);
            if (checkHandle.Status == AsyncOperationStatus.Succeeded)
            {
                List<string> updateList = checkHandle.Result;
            }


        }

        
    }
}
