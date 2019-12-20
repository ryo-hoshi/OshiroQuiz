using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace OshiroFirebase
{
    public class OshiroRemoteConfig
    {
        private static OshiroRemoteConfig _Instance = new OshiroRemoteConfig();

        public static OshiroRemoteConfig Instance()
        {
            return _Instance;
        }

        private OshiroRemoteConfig()
        {
        }

        /// <summary>
        /// 強制アップデートのバージョン
        /// </summary>
        private string _ForceUpdateVersion = "1.0.0";
        public string ForceUpdateVersion
        {
            get { return _ForceUpdateVersion; }
        }

        /// <summary>
        /// キャリアアップ上限値
        /// </summary>
        private int _CareerUpLimit = 3;
        public int CareerUpLimit
        {
            get { return _CareerUpLimit; }
        }

        /// <summary>
        /// メンテナンス中フラグ
        /// </summary>
        private bool _IsMaintenance = false;
        public bool IsMaintenance
        {
            get { return _IsMaintenance; }
        }

        /// <summary>
        /// RemoteConfigのFetch成功時間
        /// </summary>
        private DateTime _FetchTime = DateTime.MinValue;
        public DateTime FetchTime
        {
            get { return _FetchTime; }
        }

        public void RemoteConfigFetch()
        {
            if (Debug.isDebugBuild)
            {
                // デバッグモードの場合はリモートコンフィグの値をキャッシュしないように
                var settings = Firebase.RemoteConfig.FirebaseRemoteConfig.Settings;
                settings.IsDeveloperMode = true;
                Firebase.RemoteConfig.FirebaseRemoteConfig.Settings = settings;
            }

            Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(new System.TimeSpan(0));
            fetchTask.ContinueWith(RemoteConfigFetchComplete);
        }

        private void RemoteConfigFetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("■■■■■Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("■■■■■Fetch error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("■■■■■Fetch completed.");
            }

            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
            switch(info.LastFetchStatus) {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();

                    _ForceUpdateVersion = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("force_update_version").StringValue;
                    _CareerUpLimit = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("career_up_limit").LongValue;
                    _IsMaintenance = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("is_maintenance").BooleanValue;

                    _FetchTime = DateTime.Now;
                    // リモートコンフィグ取得時の時間を退避
                    Debug.Log("！！！！！リモートコンフィグのフェッチ時刻の退避" + _FetchTime);

                    Debug.Log("_ForceUpdateVersion 取得結果：" + _ForceUpdateVersion);
                    Debug.Log("_CareerUpLimit 取得結果：" + _CareerUpLimit);
                    Debug.Log("_IsMaintenance 取得結果：" + _IsMaintenance);
    #if UNITY_EDITOR
                    // minimumVersion = "1.0.0";
                    // careerUpLimit = 2;
                    // isMaintenance = false;
    #endif

                    Debug.Log("Remote data loaded and ready.");
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (Firebase.RemoteConfig.FirebaseRemoteConfig.Info.LastFetchFailureReason) {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + 
                            Firebase.RemoteConfig.FirebaseRemoteConfig.Info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }

        /// <summary>
        /// RemoteConfigの再Fetch対象かどうか
        /// </summary>
        /// <returns>Fetch対象かどうか(true:対象)</returns>
        public bool IsNeedFetch()
        {
            DateTime nowDt = DateTime.Now;
            DateTime yesterday = nowDt.AddDays(-1);

            // 現在日時よりも1日以上前の場合はフェッチ対象
            return _FetchTime < yesterday;
        }

        // /// <summary>
        // /// リモートコンフィグの初期値設定
        // /// </summary>
        // public static void RemoteConfigInit()
        // {
        //     var defaults = new System.Collections.Generic.Dictionary<string, object>();

        //     defaults.Add("force_update_version", "1.0.0");
        //     defaults.Add("career_up_limit", 2);
        //     defaults.Add("is_maintenance", false);

        //     Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        // }
    }
}
