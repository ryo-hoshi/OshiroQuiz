using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace OshiroFirebase
{
    public class OshiroFirebases
    {
		public void Init()
		{
			Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					// Create and hold a reference to your FirebaseApp,
					// where app is a Firebase.FirebaseApp property of your application class.
					//   app = Firebase.FirebaseApp.DefaultInstance;

					// Set a flag here to indicate whether Firebase is ready to use by your app.

					var oshiroRemoteConfig = OshiroRemoteConfig.Instance();
					oshiroRemoteConfig.RemoteConfigFetch();

				} else {
					UnityEngine.Debug.LogError(System.String.Format(
					"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
					// Firebase Unity SDK is not safe to use here.
				}
			});
		}

		public void FirebaseAsyncAction(UnityAction callback)
		{
			Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					// Create and hold a reference to your FirebaseApp,
					// where app is a Firebase.FirebaseApp property of your application class.
					//   app = Firebase.FirebaseApp.DefaultInstance;

					// Set a flag here to indicate whether Firebase is ready to use by your app.

					callback?.Invoke(); 

				} else {
					UnityEngine.Debug.LogError(System.String.Format(
					"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
					// Firebase Unity SDK is not safe to use here.
				}
			});
		}
    }
}
