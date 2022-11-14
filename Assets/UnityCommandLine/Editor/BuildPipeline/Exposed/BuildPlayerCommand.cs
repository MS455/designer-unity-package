#region File Header

// Filename: BuildPlayerCommand.cs
// Author: Elmer Nocon
// Date Created: 2019/05/16
// License: MIT

#endregion

using System;
using UnityCommandLine.BuildPipeline;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.AddressableAssets.Initialization;
/// <summary>
/// Builds a player using the given <see cref="T:UnityEditor.BuildTarget" />.
/// </summary>
/// <para>
/// Example:
/// <code>-executeMethod BuildPlayerCommand.Execute -buildTarget win64</code>
/// </para>
public class BuildPlayerCommand : BuildPlayerCommandBase
{

    #region Statics

    #region Static Methods

    /// <summary>
    /// Executes this command.
    /// </summary>
    /// <exception cref="Exception"></exception>
    [UsedImplicitly]
    public static void Execute()
    {
        var arguments = GetArguments();

        string buildTargetString;
        if (!GetArgumentValue(arguments, Values.ARG_BUILD_TARGET, out buildTargetString))
            throw new Exception(string.Format("Argument '{0}' is required.", Values.ARG_BUILD_TARGET));

        var buildTarget = buildTargetString.ToBuildTarget();

        var command = new BuildPlayerCommand(buildTarget);

        command.Run();
    }

    public static void BuildAddressable()
    {
        Debug.Log("Before Cleaning");
        AddressableAssetSettings.CleanPlayerContent(
            AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        Debug.Log("After Cleaning");
        AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("Addressable Content Creation Done");
    }

    public static void BuildAddressableWithProfiles()
    {
        Debug.Log("Entering BuildAddressableWithProfiles");

        var arguments = GetArguments();
        string addressableProfile="";
        string targetPlatform="";

        if (GetArgumentValue(arguments, Values.ARG_ADDRESSABLE_PROFILE_NAME, out addressableProfile))
            Debug.Log("Getting inside addressableprofile" + addressableProfile);
        if (GetArgumentValue(arguments, Values.ARG_BUILD_TARGET, out targetPlatform))
                Debug.Log("Getting inside targetplatform" + targetPlatform);

        string addressableBinFile = "addressables_content_state.bin";
        string localAddressableBuildPath = $"Assets/AddressableAssetsData/{targetPlatform}";
        Debug.Log("Local build path:" + localAddressableBuildPath);

        if (!AssetDatabase.IsValidFolder(localAddressableBuildPath) || !File.Exists($"{localAddressableBuildPath}/{addressableBinFile}"))
        {
            Debug.Log("Entering condition where bin file doesn't exist");
            CheckActiveAddressableProfile(addressableProfile);
            AddressableAssetSettings.BuildPlayerContent();
            CheckAddressableProfileFolderExists(localAddressableBuildPath, addressableProfile);
            var binFilePath = $"{localAddressableBuildPath}/{addressableBinFile}";
            var profileFilePath = $"{localAddressableBuildPath}/{addressableProfile}/{addressableBinFile}";
            AssetDatabase.Refresh();
            Debug.Log("BinFilePath:" + binFilePath);
            Debug.Log("ProfileFilePath:" + profileFilePath);
            bool copyAssetResult = AssetDatabase.CopyAsset(binFilePath, profileFilePath);
            Debug.Log("CopyAssetResult:" + copyAssetResult);
            Debug.Log("Copied bin file from local build path to profile folder");
        }
        else
        {
            CheckActiveAddressableProfile(addressableProfile);
            CheckAddressableProfileFolderExists(localAddressableBuildPath, addressableProfile);
            AssetDatabase.CopyAsset($"{localAddressableBuildPath}/{addressableBinFile}", $"{localAddressableBuildPath}/{addressableProfile}/{addressableBinFile}");
            AssetDatabase.CopyAsset($"{localAddressableBuildPath}/{addressableProfile}/{addressableBinFile}", $"{localAddressableBuildPath}/{addressableBinFile}");
            Debug.Log("Copied bin file from profile folder to local build path");
            AddressableAssetSettings.BuildPlayerContent();
            AssetDatabase.CopyAsset($"{localAddressableBuildPath}/{addressableBinFile}", $"{localAddressableBuildPath}/{addressableProfile}/{addressableBinFile}");
            Debug.Log("Copied bin file from local build path to profile folder");
        }
    }

    public static void CheckActiveAddressableProfile(string addressable)
    {
        if (AddressableAssetSettingsDefaultObject.Settings.activeProfileId != addressable)
        {
            var profile = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileId(addressable);
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profile;
        }
    }

    public static void CheckAddressableProfileFolderExists(string buildPath, string profile)
    {
        if (!AssetDatabase.IsValidFolder($"{buildPath}/{profile}"))
        {
            Debug.Log("Checking whether profileID folder exists");
            AssetDatabase.CreateFolder(buildPath, profile);
        }
    }

    #endregion

    #endregion

    #region Constructors & Destructors

    /// <summary>
    /// Creates an instance of <see cref="BuildPlayerCommand"/>.
    /// </summary>
    /// <param name="target">The build target.</param>
    protected BuildPlayerCommand(BuildTarget target) : base(target)
    {
    }

    #endregion
}