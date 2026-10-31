#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<string>> _copiedWeaponSkills { get; }

    ref readonly global::System.Collections.Generic.HashSet<string> _usedGeneralWeaponSources { get; }

    ref readonly global::System.Collections.Generic.HashSet<int> _generatedPhysicalCardIds { get; }

    ref readonly global::System.Collections.Generic.HashSet<int> _permanentlyRemovedWeapons { get; }
}
