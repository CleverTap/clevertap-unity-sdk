#!/usr/bin/env bash

# Parse command line options
while getopts u: option
do
    case "${option}" in
        u) UNITY_BIN=${OPTARG};; # Path to the Unity binary
    esac
done

# Define essential paths
PROJECT="$PWD/CTExporter" # Path to the Unity project
PACKAGE="$PWD/CleverTapSDK.unitypackage" # Output package file path
SYMBOLIC_LINK_PATH="$PROJECT/Assets/CleverTap"
MANIFEST_JSON_PATH="$PROJECT/Packages/manifest.json"
ORIGINAL_ANDROID_LIB_FOLDER_PATH="$PWD/CleverTap/Plugins/Android/clevertap-android-wrapper.androidlib"
RENAMED_ANDROID_LIB_FOLDER_PATH="$PWD/CleverTap/Plugins/Android/clevertap-android-wrapper"

echo $ORIGINAL_ANDROID_LIB_FOLDER_PATH

# Rename the folder
if [ -d "$ORIGINAL_ANDROID_LIB_FOLDER_PATH" ]; then
    mv "$ORIGINAL_ANDROID_LIB_FOLDER_PATH" "$RENAMED_ANDROID_LIB_FOLDER_PATH"
    mv "$ORIGINAL_ANDROID_LIB_FOLDER_PATH.meta" "$RENAMED_ANDROID_LIB_FOLDER_PATH.meta"
else
    echo "Original folder not found!"
    exit 1
fi

# Ensure the necessary folders are in place
ln -s "$PWD/CleverTap" "$PROJECT/Assets" # Create a symbolic link to CleverTap

# Update the manifest file to exclude certain packages
awk '!/com.clevertap.clevertap-sdk-unity/' $MANIFEST_JSON_PATH > temp && mv temp $MANIFEST_JSON_PATH

# Find folders to export
FOLDERS_TO_EXPORT=$(cd $PROJECT; find Assets/CleverTap/* Assets/PlayServicesResolver Assets/ExternalDependencyManager -type d -prune)

# Check if script is run from the correct location
if ! [ -d "$PROJECT" ]; then
    echo "Run this script from the root folder of the repository (e.g. ./scripts/create_unity_package.sh)."
    rm $SYMBOLIC_LINK_PATH
    exit 1
fi

# Check if Unity binary is specified
if [ -z "$UNITY_BIN" ]; then
    echo "😞 Unity not passed as parameter!"
    echo "Pass the location of Unity. Something like ./scripts/create_unity_package.sh -u /Applications/Unity/Hub/Editor/2019.3.10f1/Unity.app/Contents/MacOS/Unity"
    rm $SYMBOLIC_LINK_PATH
    exit 1
fi

# Handle PlayServicesResolver and ExternalDependencyManager
if [ -d "$PROJECT/Assets/PlayServicesResolver" ]; then
    echo "PlayServicesResolver folder found in assets. It will be deleted and reimported."
    rm -rf $PROJECT/Assets/PlayServicesResolver
fi

if [ -d "$PROJECT/Assets/ExternalDependencyManager" ]; then
    echo "ExternalDependencyManager folder found in assets. It will be deleted and reimported."
    rm -rf $PROJECT/Assets/ExternalDependencyManager
fi

# Download the external dependency manager if not present
if [ -f $PROJECT/external-dependency-manager-*.unitypackage ]; then
    echo "👌 External dependency manager plugin found. It will be added to the unitypackage."
else
    wget https://github.com/googlesamples/unity-jar-resolver/raw/master/external-dependency-manager-latest.unitypackage -P $PROJECT
fi

# Remove old package if it exists
if [ -f $PACKAGE ]; then
    echo "📦 Old package found. Removing it."
    rm $PACKAGE
fi

# Create the Unity package
echo "📦 Creating CleverTapSDK.unitypackage, this may take a minute."
$UNITY_BIN -gvh_disable \
-nographics \
-ct-export \
-projectPath $PROJECT \
-force-free -quit -batchmode -logFile exportlog.txt \
-importPackage $PROJECT/external-dependency-manager-latest.unitypackage \
-exportPackage $FOLDERS_TO_EXPORT $PACKAGE

echo "Unity package created"

# Cleanup
rm $SYMBOLIC_LINK_PATH

# Revert the folder name back to original after the build
mv "$RENAMED_ANDROID_LIB_FOLDER_PATH" "$ORIGINAL_ANDROID_LIB_FOLDER_PATH"
mv "$RENAMED_ANDROID_LIB_FOLDER_PATH.meta" "$ORIGINAL_ANDROID_LIB_FOLDER_PATH.meta"