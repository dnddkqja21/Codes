public static class BuildConfig 
{

    public static bool isRelease = false;

    public const bool isKJHTest = true;
    public const bool isKCWTest = true;

    public static string roomVersion = isRelease ? "1.0" : "1.0.1";
    public static string roomName = isRelease ? "Metaverse" : "Metaverse_test";

    internal static string ios_bundleVersion;
    internal static string aos_bundleVersion;

    //PlayerSettings.bundleVersion = BuildConfig.ios_bundleVersion;
    //    PlayerSettings.bundleVersion = BuildConfig.aos_bundleVersion;
}
