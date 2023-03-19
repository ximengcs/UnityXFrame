public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	//System.Action`1<System.Object>
	//XFrame.Core.SingletonModule`1<System.Object>
	//XFrame.Modules.Tasks.XTask`1<System.Object>
	// }}

	public void RefMethods()
	{
		// System.Object[] System.Array::Empty<System.Object>()
		// System.Object UnityEngine.GameObject::AddComponent<System.Object>()
	}
}