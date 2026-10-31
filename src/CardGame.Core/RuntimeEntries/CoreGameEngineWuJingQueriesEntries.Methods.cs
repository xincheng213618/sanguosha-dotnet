#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWuJingQueriesEntries
{
    internal bool IsTurnSlashChoiceUseForbidden(int actorSeat, global::CardGame.Core.PromptChoice choice) => _resolve().IsTurnSlashChoiceUseForbidden(actorSeat, choice);
}
