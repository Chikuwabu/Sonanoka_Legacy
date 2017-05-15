using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Text;
using SyntaxTree.VisualStudio.Unity.Bridge;
using System.Xml.Linq;

public class ScriptableObjectFactory : MonoBehaviour
{
	[MenuItem("Assets/Create/Score")]
	public static void Score()
	{
		CreateAsset<Score>();
	}

	public static void CreateAsset<Type>() where Type : ScriptableObject
	{
		Type item = ScriptableObject.CreateInstance<Type>();

		//string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Data/" + typeof(Type) + ".asset");
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(item.GetInstanceID(), ScriptableObject.CreateInstance<EndNameEditAction>(), "New Sonanoka Score.asset", AssetPreview.GetMiniTypeThumbnail(typeof(Type)), "");

		
	}

	class EndNameEditAction : UnityEditor.ProjectWindowCallback.EndNameEditAction
	{
		public override void Action(int instanceId, string path, string resourceFile)
		{
			var item = (ScriptableObject)EditorUtility.InstanceIDToObject(instanceId);

			AssetDatabase.CreateAsset(item, path);
			AssetDatabase.SaveAssets();

			ProjectWindowUtil.ShowCreatedAsset(item);
		}
	}

}