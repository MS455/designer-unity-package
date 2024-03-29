#region File Header

// Filename: Settings.cs
// Author: Elmer Nocon
// Date Created: 2019/05/16
// License: MIT

#endregion

using System;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace UnityCommandLine.BuildPipeline
{
    /// <summary>
    /// A container class for all values used when building a player.
    /// </summary>
    public class BuildPlayerSettings
    {
        #region Statics

        #region Static Methods

        /// <summary>
        /// Applies the values of a <see cref="BuildPlayerSettings"/> instance to <see cref="EditorUserBuildSettings"/> and <see cref="PlayerSettings"/>.
        /// </summary>
        /// <param name="settings">The build player settings instance.</param>
        public static void Apply(BuildPlayerSettings settings)
        {
            int bundleCode;
            var targetGroup = BuildTargetUtils.GetBuildTargetGroup(settings.Target);
            EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, settings.Target);
            PlayerSettings.SetApplicationIdentifier(targetGroup, settings.ApplicationIdentifier);
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId = settings.AddressableProfile;
            PlayerSettings.bundleVersion = settings.BundleVersion;
            EditorPrefs.SetString("AndroidSdkRoot", settings.AndroidSdkPath);
            PlayerSettings.Android.keyaliasName = settings.AndroidKeyAliasName;
            PlayerSettings.Android.keyaliasPass = settings.AndroidKeyAliasPass;
            PlayerSettings.Android.keystoreName = settings.AndroidKeyStoreName;
            PlayerSettings.Android.keystorePass = settings.AndroidKeyStorePass;
            Int32.TryParse(settings.BundleVersionCode, out bundleCode);
            PlayerSettings.Android.bundleVersionCode = bundleCode;
         //   PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
           


        }

        /// <summary>
        /// Creates an instance of <see cref="BuildPlayerSettings"/> with values from <see cref="EditorUserBuildSettings"/> and <see cref="PlayerSettings"/>.
        /// </summary>
        /// <returns>Returns a build player settings object.</returns>
        public static BuildPlayerSettings Create()
        {
            return new BuildPlayerSettings
            {
                Target = EditorUserBuildSettings.activeBuildTarget, 
                ApplicationIdentifier = PlayerSettings.applicationIdentifier,
                AddressableProfile = AddressableAssetSettingsDefaultObject.Settings.activeProfileId,
                BundleVersion = PlayerSettings.bundleVersion,
                AndroidSdkPath = EditorPrefs.GetString("AndroidSdkRoot"),
                AndroidKeyAliasName = PlayerSettings.Android.keyaliasName,
                AndroidKeyAliasPass = PlayerSettings.Android.keyaliasPass,
                AndroidKeyStoreName = PlayerSettings.Android.keystoreName,
                AndroidKeyStorePass = PlayerSettings.Android.keystorePass,
                BundleVersionCode = PlayerSettings.Android.bundleVersionCode.ToString(),
            };
        }

        /// <summary>
        /// Prints the values of a <see cref="BuildPlayerSettings"/> instance.
        /// </summary>
        /// <param name="settings">The build player settings instance.</param>
        /// <param name="stringBuilder">The string builder instance.</param>
        public static void Print(BuildPlayerSettings settings, StringBuilder stringBuilder)
        {
            const string listItemPrefix = "  - ";

            stringBuilder.AppendLine(string.Format("EditorUserBuildSettings.activeBuildTarget = {0} ({1})", settings.Target, settings.TargetGroup));
            stringBuilder.AppendLine(string.Format("PlayerSettings.applicationIdentifier = {0}", settings.ApplicationIdentifier));
            stringBuilder.AppendLine(string.Format("AddressableAssetSettingsDefaultObject.Settings.activeProfileId = {0}", settings.AddressableProfile));
            stringBuilder.AppendLine(string.Format("PlayerSettings.bundleVersion = {0}", settings.BundleVersion));

            if (!string.IsNullOrEmpty(settings.AndroidKeyAliasName) ||
                !string.IsNullOrEmpty(settings.AndroidKeyStoreName))
            {
                stringBuilder.AppendLine(string.Format("EditorPrefs.AndroidSdkRoot: {0}", settings.AndroidSdkPath));
                stringBuilder.AppendLine("PlayerSettings.Android:");
                stringBuilder.AppendLine(string.Format(" - KeyAliasName: {0}", settings.AndroidKeyAliasName));
                stringBuilder.AppendLine(string.Format(" - KeyAliasPass: {0}", settings.AndroidKeyAliasPass));
                stringBuilder.AppendLine(string.Format(" - KeyStoreName: {0}", settings.AndroidKeyStoreName));
                stringBuilder.AppendLine(string.Format(" - KeyStorePass: {0}", settings.AndroidKeyStorePass));
            }

            if (!string.IsNullOrEmpty(settings.OutputPath))
                stringBuilder.AppendLine(string.Format("OutputPath: {0}", settings.OutputPath));
            
            if (settings.Levels != null && settings.Levels.Length > 0)
                stringBuilder.AppendLine(string.Format("Levels:\n{0}{1}", listItemPrefix, string.Join(
                        string.Format("\n{0}", listItemPrefix), settings.Levels)));
            
            if (settings.Options != BuildOptions.None)
                stringBuilder.AppendLine(string.Format("Options: {0}", settings.Options));
        }

        #endregion

        #endregion

        #region Properties

        public string BasicUrl { get; set; }
        /// <summary>
        /// Gets and sets the <see cref="EditorUserBuildSettings.activeBuildTarget"/>.
        /// </summary>
        public BuildTarget Target { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.applicationIdentifier"/>.
        /// </summary>
        public string ApplicationIdentifier { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="AddressableAssetSettingsDefaultObject.Settings.activeProfileId"/>
        /// </summary>
        public string AddressableProfile { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.bundleVersion"/>
        /// </summary>
        public string BundleVersion { get; set; }


        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.Android.bundleVersionCode"/>
        /// </summary>
        public string BundleVersionCode { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="EditorPrefs"/>[<c>AndroidSdkRoot</c>].
        /// </summary>
        public string AndroidSdkPath { get; set; }
        
        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.Android.keyaliasName"/>.
        /// </summary>
        public string AndroidKeyAliasName { get; set; }
        
        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.Android.keystoreName"/>.
        /// </summary>
        public string AndroidKeyAliasPass { get; set; }
        
        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.Android.keyaliasName"/>.
        /// </summary>
        public string AndroidKeyStoreName { get; set; }
        
        /// <summary>
        /// Gets and sets the <see cref="PlayerSettings.Android.keystorePass"/>.
        /// </summary>
        public string AndroidKeyStorePass { get; set; }

        /// <summary>
        /// Gets best matching <see cref="BuildTargetGroup"/> for the build target <see cref="Target"/>.
        /// </summary>
        public BuildTargetGroup TargetGroup
        {
            get { return BuildTargetUtils.GetBuildTargetGroup(Target); }
        }

        /// <summary>
        /// Gets and sets the output path.
        /// </summary>
        public string OutputPath { get; set; }
        
        /// <summary>
        /// Gets and sets the list of scenes to include in the build.
        /// </summary>
        public string[] Levels { get; set; }
        
        /// <summary>
        /// Gets and sets the build options to use when building the player.
        /// </summary>
        public BuildOptions Options { get; set; }

      

       




        #endregion

        #region Methods

        /// <summary>
        /// Applies the values of this <see cref="BuildPlayerSettings"/> instance to <see cref="EditorUserBuildSettings"/> and <see cref="PlayerSettings"/>.
        /// </summary>
        public void Apply()
        {
            Apply(this);
        }

        #endregion
    }
}