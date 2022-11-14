using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditModeTestScript
    {
        [UnityTest]
        public IEnumerator RunEditorUtilityInBackgroundTest()
        {
            if (Application.isEditor)
            {
                Application.runInBackground = true;
                yield return null;
            }
            Assert.IsTrue(Application.isEditor);
        }

        [UnityTest]
        public IEnumerator GetPersistentDataPathTest()
        {
            string path = Application.persistentDataPath;
            yield return null;
            Assert.IsNotEmpty(path);
        }
    }
}
