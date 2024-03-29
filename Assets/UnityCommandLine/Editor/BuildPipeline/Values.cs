#region File Header

// Filename: Values.cs
// Author: Elmer Nocon
// Date Created: 2019/05/16
// License: MIT

#endregion

using UnityEditor;

namespace UnityCommandLine.BuildPipeline
{
    /// <summary>
    /// A container class for frequently used values in build-player-related commands.
    /// </summary>
    public static class Values
    {
        #region Constants

        /// <summary>
        /// The default indent string.
        /// </summary>
        public const string BASIC_URL = "-basicUrl";
        /// <summary>
        /// The default indent string.
        /// </summary>
        public const string SCENE_TOBUILD = "-scene";
        /// <summary>
        /// The default indent string.
        /// </summary>
        public const string DEFAULT_INDENT = " ";

        /// <summary>
        /// The default build folder name.
        /// </summary>
        public const string DEFAULT_BUILD_FOLDER_NAME = "Builds";

        /// <summary>
        /// The default indent string.
        /// </summary>
        public const string OUTPUT_BUILD_FOLDER = "-outputDestination";

        /// <summary>
        /// The default asset bundles folder name.
        /// </summary>
        public const string DEFAULT_BUNDLE_FOLDER_NAME = "AssetBundles";

        /// <summary>
        /// The default build file name.
        /// </summary>
        public const string DEFAULT_BUILD_NAME = "build";

        /// <summary>
        /// The default asset bundle folder name.
        /// </summary>
        public const string DEFAULT_BUNDLE_NAME = "bundle";

        /// <summary>
        /// The default build options.
        /// </summary>
        public const BuildOptions DEFAULT_BUILD_OPTIONS = BuildOptions.None;

        /// <summary>
        /// The default bundle options.
        /// </summary>
        public const BuildAssetBundleOptions DEFAULT_BUNDLE_OPTIONS = BuildAssetBundleOptions.None;

        /// <summary>
        /// The default value of print report verbose.
        /// </summary>
        public const bool DEFAULT_BUILD_REPORT_VERBOSE = false;

        /// <summary>
        /// The argument key for the build target.
        /// </summary>
        public const string ARG_BUILD_TARGET = "-buildTarget";

        /// <summary>
        /// The argument key for the addressable profile
        /// </summary>
        public const string ARG_ADDRESSABLE_PROFILE_NAME = "-addressableprofile";

        /// <summary>
        /// The argument key for the build file name.
        /// </summary>
        public const string ARG_BUILD_NAME = "-buildName";

        /// <summary>
        /// The argument key for <see cref="PlayerSettings.applicationIdentifier"/>.
        /// </summary>
        public const string ARG_APPLICATION_IDENTIFIER = "-applicationIdentifier";

        /// <summary>
        /// The argument key for <see cref="PlayerSettings.bundleVersion"/>.
        /// </summary>
        public const string ARG_BUNDLE_VERSION = "-bundleVersion";

        /// <summary>
        /// The argument key for <see cref="EditorPrefs"/>[<c>AndroidSdkRoot</c>].
        /// </summary>
        public const string ARG_ANDROID_SDK_PATH = "-androidSdkPath";

        /// <summary>
        /// The argument key for <see cref="PlayerSettings.Android.keyaliasName"/>.
        /// </summary>
        public const string ARG_ANDROID_KEY_ALIAS_NAME = "-androidKeyAliasName";

        /// <summary>
        /// The argument key for <see cref="PlayerSettings.Android.keyaliasPass"/>.
        /// </summary>
        public const string ARG_ANDROID_KEY_ALIAS_PASS = "-androidKeyAliasPass";

        /// <summary>
        /// The argument key for <see cref="PlayerSettings.Android.keystoreName"/>.
        /// </summary>
        public const string ARG_ANDROID_KEY_STORE_NAME = "-androidKeyStoreName";

        /// <summary>
        /// The argument key for <see cref="PlayerSettings.Android.keystorePass"/>.
        /// </summary>
        public const string ARG_ANDROID_KEY_STORE_PASS = "-androidKeyStorePass";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.Development"/>.
        /// </summary>
        public const string ARG_OPTION_DEVELOPMENT = "-optionDevelopment";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.AllowDebugging"/>.
        /// </summary>
        public const string ARG_OPTION_ALLOW_DEBUGGING = "-optionAllowDebugging";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.SymlinkLibraries"/>.
        /// </summary>
        public const string ARG_OPTION_SYMLINK_LIBRARIES = "-optionSymlinkLibraries";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.ForceEnableAssertions"/>.
        /// </summary>
        public const string ARG_OPTION_FORCE_ENABLE_ASSERTIONS = "-optionForceEnableAssertions";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.CompressWithLz4"/>.
        /// </summary>
        public const string ARG_OPTION_COMPRESS_WITH_LZ4 = "-optionCompressWithLz4";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.CompressWithLz4HC"/>.
        /// </summary>
        public const string ARG_OPTION_COMPRESS_WITH_LZ4_HC = "-optionCompressWithLz4HC";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.StrictMode"/> or <see cref="BuildAssetBundleOptions.StrictMode"/>.
        /// </summary>
        public const string ARG_OPTION_STRICT_MODE = "-optionStrictMode";

        /// <summary>
        /// The argument switch for <see cref="BuildOptions.IncludeTestAssemblies"/>.
        /// </summary>
        public const string ARG_OPTION_INCLUDE_TEST_ASSEMBLIES = "-optionIncludeTestAssemblies";

        /// <summary>
        /// The argument key for the bundle folder name.
        /// </summary>
        public const string ARG_BUNDLE_NAME = "-bundleName";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.UncompressedAssetBundle"/>.
        /// </summary>
        public const string ARG_OPTION_UNCOMPRESSED_ASSET_BUNDLE = "-optionUncompressedAssetBundle";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.DisableWriteTypeTree"/>.
        /// </summary>
        public const string ARG_OPTION_DISABLE_WRITE_TYPE_TREE = "-optionDisableWriteTypeTree";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.DeterministicAssetBundle"/>.
        /// </summary>
        public const string ARG_OPTION_DETERMINISTIC_ASSET_BUNDLE = "-optionDeterministicAssetBundle";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.ForceRebuildAssetBundle"/>.
        /// </summary>
        public const string ARG_OPTION_FORCE_REBUILD_ASSET_BUNDLE = "-optionForceRebuildAssetBundle";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.IgnoreTypeTreeChanges"/>.
        /// </summary>
        public const string ARG_OPTION_IGNORE_TYPE_TREE_CHANGES = "-optionIgnoreTypeTreeChanges";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.AppendHashToAssetBundleName"/>.
        /// </summary>
        public const string ARG_OPTION_APPEND_HASH_TO_ASSET_BUNDLE_NAME = "-optionAppendHashToAssetBundleName";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.ChunkBasedCompression"/>.
        /// </summary>
        public const string ARG_OPTION_CHUNK_BASED_COMPRESSION = "-optionChunkBasedCompression";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.OmitClassVersions"/>.
        /// </summary>
        public const string ARG_OPTION_OMIT_CLASS_VERSIONS = "-optionOmitClassVersions";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.DryRunBuild"/>.
        /// </summary>
        public const string ARG_OPTION_DRY_RUN_BUILD = "-optionDryRunBuild";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.DisableLoadAssetByFileName"/>.
        /// </summary>
        public const string ARG_OPTION_DISABLE_LOAD_ASSET_BY_FILE_NAME = "-optionDisableLoadAssetByFileName";

        /// <summary>
        /// The argument switch for <see cref="BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension	"/>.
        /// </summary>
        public const string ARG_OPTION_DISABLE_LOAD_ASSET_BY_FILE_NAME_WITH_EXTENSION = "-optionDisableLoadAssetByFileNameWithExtension	d";


        /// <summary>
        /// The argument key for project id.
        /// </summary>
        public const string ARG_PROJECT_ID = "-projectId";

        /// <summary>
        /// The argument key for app bundle version code.
        /// </summary>
        public const string ARG_BUNDLEVERSION_CODE = "-appBundleVersionCode";

        /// <summary>
        /// The argument key for app Orienation.
        /// </summary>
        public const string ARG_APP_ORIENTATION = "-appOrientation";

        /// <summary>
        /// The argument key for app SplashScreen.
        /// </summary>
        public const string ARG_APP_SPLASHSCREEN = "-appSplashScreen";
        /// <summary>
        /// The argument key for striping define symbols.
        /// </summary>
        public const string ARG_STRPING_DEFINE_SYMBOLS = "-stripingDefineSymbols";
        
        /// <summary>
        /// The argument key for portrait (Allowed auto rotation orientation).
        /// </summary>
        public const string ARG_PORTRAIT = "-portrait";
        /// <summary>
        /// The argument key for PortraitUpSideDown (Allowed auto rotation orientation).
        /// </summary>
        public const string ARG_PORTRAIT_UPSIDE_DOWN = "-portraitUpSideDown";
        /// <summary>
        /// The argument key for LandscapeRight (Allowed auto rotation orientation).
        /// </summary>
        public const string ARG_LANDSCAPE_RIGHT = "-landscapeRight";
        /// <summary>
        /// The argument key for LandscapeLeft (Allowed auto rotation orientation).
        /// </summary>
        public const string ARG_LANDSCAPE_LEFT = "-landscapeLeft";
        #endregion
    }
}