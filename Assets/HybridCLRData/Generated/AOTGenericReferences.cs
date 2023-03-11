public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	//XFrame.Core.SingletonModule`1<System.Object>
	//XFrame.Modules.Tasks.XTask`1<System.Object>
	// }}

	public void RefMethods()
	{
		// System.Object[] System.Array::Empty<System.Object>()
		// System.Object UnityEngine.GameObject::AddComponent<System.Object>()
		// System.Object XFrame.Modules.Resource.ResModule::Load<System.Object>(System.String)
	}
}