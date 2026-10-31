#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWuJingQueriesEntries
{
    internal bool HasWuJingCompletedUseObserver(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().HasWuJingCompletedUseObserver(window);
    internal bool IsTurnSlashChoiceUseForbidden(int actorSeat, global::CardGame.Core.PromptChoice choice) => _resolve().IsTurnSlashChoiceUseForbidden(actorSeat, choice);
}
