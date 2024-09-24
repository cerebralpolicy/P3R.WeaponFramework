using P3R.WeaponFramework.Interfaces.Types;
using Unreal.ObjectsEmitter.Interfaces;
using FName = P3R.WeaponFramework.Interfaces.Types.FName;
using FString = P3R.WeaponFramework.Interfaces.Types.FString;

namespace P3R.WeaponFramework.Interfaces;
/// <summary>
/// <b>Borrowed from <see cref="IUnreal"/></b><br/>
/// </summary>
public unsafe interface IWFUnreal
{
    /// <summary>
    /// <inheritdoc cref="IWFUnreal"/>
    /// FName constructor/getter.
    /// </summary>
    /// <param name="str">String of FName.</param>
    /// <param name="findType">Find type, defaults to find or add.</param>
    /// <returns>FName instance.</returns>
    FName* FName(string str, Emitter.EFindName findType = Emitter.EFindName.FName_Add);

    /// <summary>
    /// <inheritdoc cref="IWFUnreal"/>
    /// Get the string of the given <see cref="FName"/>.
    /// </summary>
    /// <param name="name">FName instance.</param>
    /// <returns><see cref="FName"/> string value.</returns>
    string GetName(FName* name);

    /// <summary>
    /// <inheritdoc cref="IWFUnreal"/>
    /// Get the string of the given <see cref="FName"/>.
    /// </summary>
    /// <param name="name">FName instance.</param>
    /// <returns><see cref="FName"/> string value.</returns>
    string GetName(FName name);

    /// <summary>
    /// <inheritdoc cref="IWFUnreal"/>
    /// Get the string of the name at the given location in name pool.
    /// </summary>
    /// <param name="poolLoc">Location in name pool.</param>
    /// <returns><see cref="Types.FName"/> string value.</returns>
    string GetName(uint poolLoc);

    /// <summary>
    /// <inheritdoc cref="IWFUnreal"/>
    /// Assign a new string to an FName with the given string value.
    /// Essentially acts as overwriting an FName at runtime.
    /// </summary>
    /// <param name="modName">Mod name.</param>
    /// <param name="fnameString">String value of FName to set.</param>
    /// <param name="newString">New string value.</param>
    void AssignFName(string modName, string fnameString, string newString);

    /// <summary>
    /// Reverts an FName to its original path.
    /// Allows Weapon Framework to "undo" changes made with <see cref="AssignFName(string, string, string)"/>
    /// </summary>
    /// <param name="modName"></param>
    /// <param name="fnameString"></param>
    void RevertFName(string modName, string fnameString);

    nint FMalloc(long size, int alignment);

    FString FString(string str);

    FNamePool* GetPool();


}
