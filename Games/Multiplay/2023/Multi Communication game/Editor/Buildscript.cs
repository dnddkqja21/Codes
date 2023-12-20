using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Container.Build {

    public class Buildscript
    {
        private const string APP_NAME = "메타버스 공자아카데미";
        protected const string KEYSTORE_PASS = "eoqkr!@34";
        private const string PATH = "../../build/";
        private const string AOS_PATH = PATH + "android/";
        private const string IOS_PATH = PATH + "ios/";

        private const string IOS_BUNDLE_VERSTION = "10";
        private const string IOS_BUNDLE_SHORT_VERSTION = "1.0.2";

        public const string PHOTO_LIVRARY_USAGE_DESCRIPTION = "프로필 변경을 위한 앨범 사용 권한을 허용 하시겠습니까?";
        public const string VOICE_LIVRARY_USAGE_DESCRIPTION = "버블채팅 및 강의수업을 위해 마이크 권한을 허용 하시겠습니까?";
        public const string CAMERA_LIVRARY_USAGE_DESCRIPTION = "버블채팅 및 강의수업을 위해 권한을 허용 하시겠습니까?";

        [MenuItem("Builder/build/buildAOS")]
        public static void BuildForAOS()
        {
            BuildConfig.isRelease = true;
            //string fileName = setPla
        }

        [MenuItem("Builder/build/buildIOS")]
        public static void BuildForIOS()
        {
            BuildConfig.isRelease = true;

            if (Directory.Exists(IOS_PATH)) {
                Directory.Delete(IOS_PATH, true);
            }

            BuildPlayerOptions buildOption = new BuildPlayerOptions();
            buildOption.target = BuildTarget.iOS;
            buildOption.scenes = getBuildScenes();
            buildOption.locationPathName = IOS_PATH;
            BuildPipeline.BuildPlayer(buildOption);
        }

        protected static string[] getBuildScenes()
        {
            EditorBuildSettingsScene[] s = UnityEditor.EditorBuildSettings.scenes;
            List<string> list = new List<string>();
            foreach(EditorBuildSettingsScene scene in s)
            {
                if (scene.enabled)
                {
                    list.Add(scene.path);
                }
            }
            return list.ToArray();
        }

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target , string buildPath)
        {
            if(target == BuildTarget.iOS)
            {
                string pbxProjectPath = PBXProject.GetPBXProjectPath(buildPath);
                string plistPath = Path.Combine(buildPath , "Info.plist");

                PBXProject pbxProject = new PBXProject();
                pbxProject.ReadFromFile(pbxProjectPath);


                //푸시 설정.
                string targetGuid = pbxProject.GetUnityMainTargetGuid();
                pbxProject.AddCapability(targetGuid,PBXCapabilityType.PushNotifications);

                string targetGUID = pbxProject.GetUnityFrameworkTargetGuid();

                PlistDocument pList = new PlistDocument();
                pList.ReadFromString(File.ReadAllText(plistPath));

                PlistElementDict rootDict = pList.root;

                //수출규제
                rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

                if (rootDict.values.ContainsKey("NSLocationWhenInUseUsageDescription"))
                {
                    rootDict.values.Remove("NSLocationWhenInUseUsageDescription");
                }

                // 파일권한.
                rootDict.SetString("NSPhotoLibraryAddUsageDescription", PHOTO_LIVRARY_USAGE_DESCRIPTION);
                rootDict.SetString("NSPhotoLibraryUsageDescription", PHOTO_LIVRARY_USAGE_DESCRIPTION);

                //마이크
                rootDict.SetString("NSMicrophoneUsageDescription", VOICE_LIVRARY_USAGE_DESCRIPTION);

                //카메라
                rootDict.SetString("NSCameraUsageDescription", CAMERA_LIVRARY_USAGE_DESCRIPTION);

                //빌드버전 (내부)
                rootDict.SetString("CFBundleVersion", IOS_BUNDLE_VERSTION);
                //빌드버전 (외부)
                rootDict.SetString("CFBundleShortVersionString", IOS_BUNDLE_SHORT_VERSTION);

                //PlayerSettings
                PlayerSettings.bundleVersion = IOS_BUNDLE_SHORT_VERSTION;
            }
        }
    }
}
