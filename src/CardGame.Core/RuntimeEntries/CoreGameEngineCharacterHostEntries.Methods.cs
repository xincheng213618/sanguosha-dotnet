#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterHostEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<string> AcquireRuntimeSkills(global::CardGame.Core.CharacterState player, string sourceSkillId, global::System.Collections.Generic.IReadOnlyList<string> skillIds) => _resolve().AcquireRuntimeSkills(player, sourceSkillId, skillIds);
    internal bool HasClassicGeneralPackage { get => _resolve().HasClassicGeneralPackage; }
}
