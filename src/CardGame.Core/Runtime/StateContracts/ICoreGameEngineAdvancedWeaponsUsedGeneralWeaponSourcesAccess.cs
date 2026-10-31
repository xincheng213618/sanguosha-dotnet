#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.HashSet<string> _usedGeneralWeaponSources { get; }
}
