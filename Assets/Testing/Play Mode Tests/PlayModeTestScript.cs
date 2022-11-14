using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class PlayModeTestScript
    {
        Button testButton;

        [UnityTest]
        public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics_Test()
        {
            var go = new GameObject();
            go.AddComponent<Rigidbody>();
            var originalPosition = go.transform.position.y;

            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(originalPosition, go.transform.position.y);
        }

        [UnityTest]
        public IEnumerator SceneChangeTest()
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            yield return null;
            Scene scene = SceneManager.GetActiveScene();
            Assert.IsTrue(scene.name == "SampleScene");
        }
    }
}
