using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace UnityToolbag
{
    public static class GenerateSlnFiles
    {
        public static void Sync()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
            Debug.Log("Solution files generated!");
        }
    }
}