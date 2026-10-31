#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualDiscardPhaseLedgerEntries
{
    internal SkillProgramStepOutcome BindProgramDiscardPhaseDiscards(ProgramSkillFrame frame, string resultBind) => _resolve().BindProgramDiscardPhaseDiscards(frame, resultBind);
    internal void CollectDiscardPhaseHandDiscard(Card card, CardLocation from, CardLocation to) => _resolve().CollectDiscardPhaseHandDiscard(card, from, to);
    internal int TurnOwnerDiscardPhaseHandDiscardCount { get => _resolve().TurnOwnerDiscardPhaseHandDiscardCount; }
}
