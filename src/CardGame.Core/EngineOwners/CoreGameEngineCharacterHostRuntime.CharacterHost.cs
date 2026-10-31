#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCharacterHostRuntime
{
    internal bool HasClassicGeneralPackage => _coreNativeDamageHandlesOperations.UsesClassicBattleRules && _contentRegistryOperations._contentRegistry.Packages.Any(package => package.Id == "standard-classic-generals");

    internal IReadOnlyList<string> AcquireRuntimeSkills(CharacterState player, string sourceSkillId, IReadOnlyList<string> skillIds) => _coreProgramSkillChangesOperations.ProgramSkillChanges.AcquireRuntimeSkills(player.Seat, sourceSkillId, skillIds);
}
