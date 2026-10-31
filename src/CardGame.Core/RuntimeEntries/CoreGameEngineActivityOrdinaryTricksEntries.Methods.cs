#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActivityOrdinaryTricksEntries
{
    internal SkillProgram ActivityRuntimeProgram(SkillProgram original) => _resolve().ActivityRuntimeProgram(original);
    internal ContentSkillDefinition ActivityRuntimeSkillDefinition(string id) => _resolve().ActivityRuntimeSkillDefinition(id);
    internal bool IsActivityOrdinaryTrickNameAvailable(CardKind kind) => _resolve().IsActivityOrdinaryTrickNameAvailable(kind);
    internal bool HasActivityOrdinaryTrickProjection { get => _resolve().HasActivityOrdinaryTrickProjection; }
}
