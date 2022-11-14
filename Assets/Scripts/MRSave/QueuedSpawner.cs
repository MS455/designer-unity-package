using com.hexaware.mr;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace com.hexaware.spawner
{
    /// <summary>
    /// This class is used to queue addressable object spawning and placement
    /// </summary>
    public class QueuedSpawner : MonoBehaviour
    {

        public Queue<Request> requests = new Queue<Request>();

        [SerializeField]
        bool requestRunning = false;

        [SerializeField]
        Request currentRequest;

        [SerializeField]
        GameObjectValueList existingARModelValueList;

        AsyncOperationHandle<GameObject> handle;

        bool allRequestsDone = false;

        [SerializeField]
        UnityEvent<string> callback;

        public void AddRequests(List<Request> a_requests, GameObjectValueList a_existingARModelValueList)
        {
            this.existingARModelValueList = a_existingARModelValueList;

            if(requests == null )
                requests = new Queue<Request>();

            foreach (var item in a_requests)
            {
                requests.Enqueue(item);
            }

            requestRunning = false;
            allRequestsDone = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (allRequestsDone)
                return;

            if (!requestRunning)
            {
                if (requests != null && requests.Count > 0)
                {
                    currentRequest = requests.Peek();
                    StartRequest();
                }
            }
        }

        void StartRequest()
        {
            Debug.Log("QueuedSpawner --> StartRequest() -> " + currentRequest.id);

            handle = Addressables.LoadAssetAsync<GameObject>(currentRequest.id);
            handle.Completed += Handle_Completed;
            requestRunning = true;
        }

        private void Handle_Completed(AsyncOperationHandle<GameObject> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Handle_Completed:: Compl3t3d" + obj.Result.name);
                GameObject go = Instantiate(obj.Result, currentRequest.position, currentRequest.rotation);
                go.name = currentRequest.id;
                existingARModelValueList.Add(go);
                StartCoroutine( removeRigidbody(go) );
                MRUtil.AddObjectinMR(go, currentRequest.position, currentRequest.rotation);
                callback.Invoke(obj.Result.name);
            }
            else
            {
                Debug.LogError($"QueuedSpawner:Handle_Completed Failed -{ obj.OperationException.Message}");
            }

            requestRunning = false;

            if (requests.Count > 0)
            {
                Request r = requests.Peek();

                if (r != currentRequest)
                {
                    Debug.LogWarning($"QueuedSpawner:Handle_Completed Current request was not same as request processed. Something went wrong.");
                }
                else
                {
                    requests.Dequeue();
                }
            }

            if( requests.Count == 0)
            {
                allRequestsDone = true;
            }

        }

        IEnumerator removeRigidbody(GameObject go)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb) Destroy(rb);
        }

        public class Request
        {
            public Vector3 position;
            public Quaternion rotation;
            // addressable id
            public string id;

            public Request(Vector3 position, Quaternion rot, string id)
            {
                this.position = position;
                this.rotation = rot;
                this.id = id;
            }
            public override string ToString()
            {
                return string.Format("Request:Id:{0} , Pos:{1}, Rot:{2}" , id , position , rotation);
            }
        }

    }

}