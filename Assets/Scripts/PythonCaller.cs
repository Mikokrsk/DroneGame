using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;
public class PythonCaller : MonoBehaviour
{
    //[MenuItem("MyPythonScripts/Ensure Naming")]
    public void RunEnsureNaming()
    {
        PythonRunner.RunFile($"{Application.dataPath}/Scripts/UnityGame.py");
    }
}