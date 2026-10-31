#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualDiscardPhaseLedgerEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BindProgramDiscardPhaseDiscards(global::CardGame.Core.ProgramSkillFrame frame, string resultBind) => _resolve().BindProgramDiscardPhaseDiscards(frame, resultBind);
    internal void CollectDiscardPhaseHandDiscard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().CollectDiscardPhaseHandDiscard(card, from, to);
    internal int TurnOwnerDiscardPhaseHandDiscardCount { get => _resolve().TurnOwnerDiscardPhaseHandDiscardCount; }
}
