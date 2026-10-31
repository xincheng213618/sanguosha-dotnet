#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActivityOrdinaryTricksEntries
{
    internal global::CardGame.Core.SkillProgram ActivityRuntimeProgram(global::CardGame.Core.SkillProgram original) => _resolve().ActivityRuntimeProgram(original);
    internal global::CardGame.Core.ContentSkillDefinition ActivityRuntimeSkillDefinition(string id) => _resolve().ActivityRuntimeSkillDefinition(id);
    internal bool IsActivityOrdinaryTrickNameAvailable(global::CardGame.Core.CardKind kind) => _resolve().IsActivityOrdinaryTrickNameAvailable(kind);
    internal bool HasActivityOrdinaryTrickProjection { get => _resolve().HasActivityOrdinaryTrickProjection; }
}
