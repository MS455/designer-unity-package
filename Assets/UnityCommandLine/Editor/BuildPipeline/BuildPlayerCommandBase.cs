#region File Header

// Filename: BuildPlayerCommandBase.cs
// Author: Elmer Nocon
// Date Created: 2019/05/16
// License: MIT

#endregion

using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UBuildPipeline = UnityEditor.BuildPipeline;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

namespace UnityCommandLine.BuildPipeline
{
    /// <inheritdoc />
    /// <summary>
    /// The base class for all build-player-related commands executable through Unity's command line interface.
    /// </summary>
    public abstract class BuildPlayerCommandBase : BuildPipelineCommandBase
    {
        #region Statics

        #region Static Methods

        /// <summary>
        /// Builds a player.
        /// </summary>
        /// <param name="settings">The build player settings.</param>
        /// <returns>Either a BuildReport object or a string depending on the editor version used.</returns>
        private static
#if UNITY_2018_1_OR_NEWER
                BuildReport
#else
                string
#endif
                BuildPlayer(BuildPlayerSettings settings)
        {
            return UBuildPipeline.BuildPlayer(settings.Levels, settings.OutputPath, settings.Target, settings.Options);
        }

        /// <summary>
        /// Gets all the included and enabled scenes in the project.
        /// </summary>
        /// <returns>The enabled scenes path.</returns>
        private static string[] GetEnabledScenes()
        {
            return EditorBuildSettings.scenes
                                      .Where(scene => scene.enabled && !string.IsNullOrEmpty(scene.path))
                                      .Select(scene => scene.path)
                                      .ToArray();
        }

        
        private static string[] GetSelectedScene(string[] scenes)
        {
            List<string> selectedScenes=new List<string>();
            foreach (string str in scenes)
            {
                selectedScenes.Add(EditorBuildSettings.scenes
                     .Where(scene => scene.enabled && !string.IsNullOrEmpty(scene.path))
                                  .Select(scene => str).ToString());
                                  
            }
            return selectedScenes.ToArray();
         

          
        }

        /// <summary>
        /// Prints a build report.
        /// </summary>
        /// <param name="report">The build report.</param>
#if UNITY_2018_1_OR_NEWER
        private static void PrintReport(BuildReport report)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine(UnityCommandLine.Values.SEPARATOR);
            BuildReportUtils.StringifyReport(report, stringBuilder, Values.DEFAULT_BUILD_REPORT_VERBOSE);
            stringBuilder.AppendLine(UnityCommandLine.Values.SEPARATOR);
            
            PrintLine(stringBuilder.ToString());
        }
#else
        private static void PrintReport(string report)
        {
            PrintLine(report);
        }
#endif

        /// <summary>
        /// Print a <see cref="Settings"/> object.
        /// </summary>
        /// <param name="settings">The build player settings object.</param>
        /// <param name="title">The title.</param>
        private static void PrintSettings(BuildPlayerSettings settings, string title = null)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine(UnityCommandLine.Values.SEPARATOR);
            if (!string.IsNullOrEmpty(title)) stringBuilder.AppendLine(title);
            BuildPlayerSettings.Print(settings, stringBuilder);
            stringBuilder.AppendLine(UnityCommandLine.Values.SEPARATOR);
            
            PrintLine(stringBuilder.ToString());
        }

        #endregion

        #endregion

        #region Fields
        
        /// <summary>
        /// The build player settings to use when building the player.
        /// </summary>
        protected readonly BuildPlayerSettings Settings;
        
        private readonly BuildPlayerSettings _backupSettings;

        #endregion

        #region Constructors & Destructors

        /// <inheritdoc />
        /// <summary>
        /// Creates an instance of <see cref="T:UnityCommandLine.BuildPipeline.BuildPlayerCommandBase" />.
        /// </summary>
        /// <param name="target">The build target.</param>
        protected BuildPlayerCommandBase(BuildTarget target)
        {
            _backupSettings = BuildPlayerSettings.Create();
            Settings = BuildPlayerSettings.Create();

            Settings.Target = target;
            
            if (!IsBuildTargetSupported(Settings.TargetGroup, Settings.Target))
                throw new Exception(string.Format("Build target '{0}' is not supported on this editor.", target));
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Run()
        {
            try
            {
                PrintSettings(_backupSettings, "Saved settings:");
                
                InitArgs();
                
                if (IsBuildPipelineBusy())
                    throw new Exception("BuildPipeline is busy.");
            
                var report = BuildPlayer(Settings);

                PrintReport(report);

#if UNITY_2018_1_OR_NEWER
                if (report.summary.result != BuildResult.Succeeded)
                    throw new Exception("Build failed!");
#endif
            }
            finally
            {
                _backupSettings.Apply();
                
                PrintSettings(_backupSettings, "Reverted settings:");
            }
        }
        
        /// <summary>
        /// Gets the output path.
        /// </summary>
        /// <param name="outputFileName">The output file name.</param>
        /// <returns>Returns the output path.</returns>
        protected virtual string GetOutputPath(string outputFileName)
        {
            var fileExtension = BuildTargetUtils.GetFileExtension(Settings.Target);

            return outputFileName + fileExtension;
            //return CommandUtils.PathCombine(Values.DEFAULT_BUILD_FOLDER_NAME, Settings.Target.ToString(),
            //        string.Format("{0}{1}", outputFileName, fileExtension));
        }

        /// <summary>
        /// Gets the output path.
        /// </summary>
        /// <param name="outputFileName">The output file name.</param>
        /// <returns>Returns the output path.</returns>
        protected void GetUIOrientation(string value)
        {
            switch (value)
            {
                case "AutoRotation":
                    PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
                    break;
                case "LandscapeLeft": 
                    PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
                    break;
                case "LandscapeRight": 
                    PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeRight;
                    break;
                case "Portrait":
                    PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
                    break;
                case "PortraitUpsideDown": 
                    PlayerSettings.defaultInterfaceOrientation = UIOrientation.PortraitUpsideDown;
                    break;


            }
        }

        /// <summary>
        /// Initializes the arguments used by this command.
        /// </summary>
        private void InitArgs()
        {
            // Set the build name if there is one.
            string buildName;
            if (!GetArgumentValue(Values.ARG_BUILD_NAME, out buildName))
                buildName = Values.DEFAULT_BUILD_NAME;

            string outputFolder;
            if (GetArgumentValue(Values.OUTPUT_BUILD_FOLDER, out outputFolder))
            {
                if (!outputFolder.EndsWith(System.IO.Path.DirectorySeparatorChar + ""))
                    outputFolder += System.IO.Path.DirectorySeparatorChar;
                Settings.OutputPath = GetOutputPath(outputFolder + buildName);
            }
            // Set the levels to include in the build.
                    Settings.Levels = GetEnabledScenes();
            // Set the levels with added custom scenes from parameter
            string sceneName;
            if (GetArgumentValue(Values.SCENE_TOBUILD, out sceneName))
            {
                string[] values = sceneName.Split(',');
                Settings.Levels = values;           
            }

            string splashoption;
            if (GetArgumentValue(Values.ARG_APP_SPLASHSCREEN, out splashoption))
            {
                bool splashVisbility = Boolean.TryParse(splashoption, out splashVisbility);
                PlayerSettings.SplashScreen.show = splashVisbility;
            }

            string strinpingDefineSymbols;
            if (GetArgumentValue(Values.ARG_STRPING_DEFINE_SYMBOLS, out strinpingDefineSymbols))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(Settings.TargetGroup, strinpingDefineSymbols);
            }

            string portrait;
            if (GetArgumentValue(Values.ARG_PORTRAIT, out portrait))
            {
                bool portaraitVal = Boolean.TryParse(portrait, out portaraitVal);
                PlayerSettings.allowedAutorotateToPortrait = portaraitVal;
            }

            string portraitUpsideDown;
            if (GetArgumentValue(Values.ARG_PORTRAIT_UPSIDE_DOWN, out portraitUpsideDown))
            {
                bool portaraitUpsideDownVal = Boolean.TryParse(portraitUpsideDown, out portaraitUpsideDownVal);
                PlayerSettings.allowedAutorotateToPortraitUpsideDown = portaraitUpsideDownVal;
            }
            string landscapeLeft;
            if (GetArgumentValue(Values.ARG_LANDSCAPE_LEFT, out landscapeLeft))
            {
                bool landscapeLeftVal = Boolean.TryParse(landscapeLeft, out landscapeLeftVal);
                PlayerSettings.allowedAutorotateToLandscapeLeft = landscapeLeftVal;
            }

            string landscapeRight;
            if (GetArgumentValue(Values.ARG_LANDSCAPE_RIGHT, out landscapeRight))
            {
                bool landscapeRightVal = Boolean.TryParse(landscapeRight, out landscapeRightVal);
                PlayerSettings.allowedAutorotateToLandscapeRight = landscapeRightVal;

            }

            string appOrientation;
            if (GetArgumentValue(Values.ARG_APP_ORIENTATION, out appOrientation))
            {
                GetUIOrientation(appOrientation);
            }
            //Settings.Levels = GetSelectedScene(sceneName);
            // scenes= GetSelectedScenescenessceneName);


            // Set the levels to include in the build.
            // Settings.Levels = GetEnabledScenes();

            // Set android only settings.
            if (Settings.TargetGroup == BuildTargetGroup.Android)
            {
                string androidSdkPath;
                if (GetArgumentValue(Values.ARG_ANDROID_SDK_PATH, out androidSdkPath))
                    Settings.AndroidSdkPath = androidSdkPath;

                string keyAliasName;
                if (GetArgumentValue(Values.ARG_ANDROID_KEY_ALIAS_NAME, out keyAliasName))
                    Settings.AndroidKeyAliasName = keyAliasName;
                
                string keyAliasPass;
                if (GetArgumentValue(Values.ARG_ANDROID_KEY_ALIAS_PASS, out keyAliasPass))
                    Settings.AndroidKeyAliasPass = keyAliasPass;
                
                string keyStoreName;
                if (GetArgumentValue(Values.ARG_ANDROID_KEY_STORE_NAME, out keyStoreName))
                    Settings.AndroidKeyStoreName = keyStoreName;
                
                string keyStorePass;
                if (GetArgumentValue(Values.ARG_ANDROID_KEY_STORE_PASS, out keyStorePass))
                    Settings.AndroidKeyStorePass = keyStorePass;
            }

            // Set the initial build option.
            Settings.Options = Values.DEFAULT_BUILD_OPTIONS;

            // Set additional build options to use in the build.
            if (HasArgument(Values.ARG_OPTION_DEVELOPMENT))
                Settings.Options |= BuildOptions.Development;
           

            if (HasArgument(Values.ARG_OPTION_ALLOW_DEBUGGING))
                Settings.Options |= BuildOptions.AllowDebugging;

             if (HasArgument(Values.ARG_OPTION_SYMLINK_LIBRARIES))
#if UNITY_2020_3_OR_NEWER
                Settings.Options |= BuildOptions.SymlinkLibraries;
#else
                Settings.Options |= BuildOptions.SymlinkSources;
#endif


            if (HasArgument(Values.ARG_OPTION_FORCE_ENABLE_ASSERTIONS))
                Settings.Options |= BuildOptions.ForceEnableAssertions;

            if (HasArgument(Values.ARG_OPTION_COMPRESS_WITH_LZ4))
                Settings.Options |= BuildOptions.CompressWithLz4;

#if UNITY_2017_2_OR_NEWER
            if (HasArgument(Values.ARG_OPTION_COMPRESS_WITH_LZ4_HC))
                Settings.Options |= BuildOptions.CompressWithLz4HC;
#endif

            if (HasArgument(Values.ARG_OPTION_STRICT_MODE))
                Settings.Options |= BuildOptions.StrictMode;

#if UNITY_2018_1_OR_NEWER
            if (HasArgument(Values.ARG_OPTION_INCLUDE_TEST_ASSEMBLIES))
                Settings.Options |= BuildOptions.IncludeTestAssemblies;
#endif
            
            // Set the application identifier if there is one.
            string applicationIdentifier;
            if (GetArgumentValue(Values.ARG_APPLICATION_IDENTIFIER, out applicationIdentifier))
                Settings.ApplicationIdentifier = applicationIdentifier;
            
            // Set the addressable profile if there is one.
            string addressableProfile;
            if (GetArgumentValue(Values.ARG_ADDRESSABLE_PROFILE_NAME, out addressableProfile))
                Settings.AddressableProfile = addressableProfile;

            // Set the bundle version if there is one.
            string bundleVersion;
            if (GetArgumentValue(Values.ARG_BUNDLE_VERSION, out bundleVersion))
                Settings.BundleVersion = bundleVersion;

            string bundleVersionCode;
            if (GetArgumentValue(Values.ARG_BUNDLE_VERSION, out bundleVersionCode))
                Settings.BundleVersionCode = bundleVersionCode;

            // Apply the new settings.
            Settings.Apply();
                
            PrintSettings(Settings, "Overwrote settings:");
        }

      


        #endregion
    }
}
