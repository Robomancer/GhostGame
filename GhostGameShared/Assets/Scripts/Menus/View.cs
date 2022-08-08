using UnityEngine;

//Courtesy of Abdelfattah Radwan at https://gist.github.com/Abdelfattah-Radwan
//Editted for use by Robomancer Research Group by Michael LaFollette

public abstract class View : MonoBehaviour
{
	public abstract void Initialize();

	public virtual void Hide() => gameObject.SetActive(false);

	public virtual void Show() => gameObject.SetActive(true);
}