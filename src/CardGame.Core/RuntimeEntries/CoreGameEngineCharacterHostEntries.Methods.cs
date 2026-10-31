#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterHostEntries
{
    internal IReadOnlyList<string> AcquireRuntimeSkills(CharacterState player, string sourceSkillId, IReadOnlyList<string> skillIds) => _resolve().AcquireRuntimeSkills(player, sourceSkillId, skillIds);
    internal bool HasClassicGeneralPackage { get => _resolve().HasClassicGeneralPackage; }
}
