#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<string> _usedGeneralWeaponSources { get; }
}
