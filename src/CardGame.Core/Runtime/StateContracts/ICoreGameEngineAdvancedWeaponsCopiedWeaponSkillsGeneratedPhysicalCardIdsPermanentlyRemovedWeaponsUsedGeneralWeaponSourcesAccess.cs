#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, IReadOnlyList<string>> _copiedWeaponSkills { get; }

    ref readonly HashSet<string> _usedGeneralWeaponSources { get; }

    ref readonly HashSet<int> _generatedPhysicalCardIds { get; }

    ref readonly HashSet<int> _permanentlyRemovedWeapons { get; }
}
