#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAdvancedWeaponsState : Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess, Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess, Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess
{
    private Dictionary<int, IReadOnlyList<string>> _copiedWeaponSkills = [];
    internal ref readonly Dictionary<int, IReadOnlyList<string>> ReferenceToCopiedWeaponSkills => ref _copiedWeaponSkills;

    private HashSet<int> _generatedPhysicalCardIds = [];
    internal ref readonly HashSet<int> ReferenceToGeneratedPhysicalCardIds => ref _generatedPhysicalCardIds;

    private HashSet<int> _permanentlyRemovedWeapons = [];
    internal ref readonly HashSet<int> ReferenceToPermanentlyRemovedWeapons => ref _permanentlyRemovedWeapons;

    private HashSet<string> _usedGeneralWeaponSources = new(StringComparer.Ordinal);
    internal ref readonly HashSet<string> ReferenceToUsedGeneralWeaponSources => ref _usedGeneralWeaponSources;

    ref readonly HashSet<int> Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds => ref _generatedPhysicalCardIds;

    ref readonly HashSet<string> Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess._usedGeneralWeaponSources => ref _usedGeneralWeaponSources;

    ref readonly Dictionary<int, IReadOnlyList<string>> Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._copiedWeaponSkills => ref _copiedWeaponSkills;

    ref readonly HashSet<string> Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._usedGeneralWeaponSources => ref _usedGeneralWeaponSources;

    ref readonly HashSet<int> Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._generatedPhysicalCardIds => ref _generatedPhysicalCardIds;

    ref readonly HashSet<int> Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess._permanentlyRemovedWeapons => ref _permanentlyRemovedWeapons;
}
