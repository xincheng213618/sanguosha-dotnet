#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAdvancedWeaponsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess
{
    private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<string>> _copiedWeaponSkills = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<string>> ReferenceToCopiedWeaponSkills => ref _copiedWeaponSkills;

    private global::System.Collections.Generic.HashSet<int> _generatedPhysicalCardIds = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToGeneratedPhysicalCardIds => ref _generatedPhysicalCardIds;

    private global::System.Collections.Generic.HashSet<int> _permanentlyRemovedWeapons = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToPermanentlyRemovedWeapons => ref _permanentlyRemovedWeapons;

    private global::System.Collections.Generic.HashSet<string> _usedGeneralWeaponSources = new(global::System.StringComparer.Ordinal);
    internal ref readonly global::System.Collections.Generic.HashSet<string> ReferenceToUsedGeneralWeaponSources => ref _usedGeneralWeaponSources;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds => ref _generatedPhysicalCardIds;

    ref readonly global::System.Collections.Generic.HashSet<string> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess._usedGeneralWeaponSources => ref _usedGeneralWeaponSources;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<string>> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._copiedWeaponSkills => ref _copiedWeaponSkills;

    ref readonly global::System.Collections.Generic.HashSet<string> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._usedGeneralWeaponSources => ref _usedGeneralWeaponSources;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._generatedPhysicalCardIds => ref _generatedPhysicalCardIds;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._permanentlyRemovedWeapons => ref _permanentlyRemovedWeapons;
}
