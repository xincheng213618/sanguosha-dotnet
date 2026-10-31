#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSharedSlashOffersEntries
{
    internal SkillProgramStepOutcome BeginSharedSlashGift(long frameId, int actorSeat, string bind) => _resolve().BeginSharedSlashGift(frameId, actorSeat, bind);
    internal void BeginSharedSlashUse(ProgramSkillFrame f, int targetSeat) => _resolve().BeginSharedSlashUse(f, targetSeat);
    internal void CompleteSharedSlashBenefit(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteSharedSlashBenefit(completion);
    internal bool IsSharedSlashGiftSelection(ProgramSkillFrame f, string bind) => _resolve().IsSharedSlashGiftSelection(f, bind);
    internal void IssueSharedSlashDraw(ProgramSkillFrame f, bool recipient) => _resolve().IssueSharedSlashDraw(f, recipient);
    internal bool MatchesProgramOwnedSelectionKind(ProgramSkillFrame f, string bind, CharacterState owner, Card card, CardLocation from, IReadOnlyList<CardKind> kinds) => _resolve().MatchesProgramOwnedSelectionKind(f, bind, owner, card, from, kinds);
    internal void PublishSharedSlashOffer(ProgramSkillFrame f) => _resolve().PublishSharedSlashOffer(f);
    internal void ResolveSharedSlashOfferChoice(PromptChoice choice) => _resolve().ResolveSharedSlashOfferChoice(choice);
    internal bool ResumeSharedSlashOffer(long frameId) => _resolve().ResumeSharedSlashOffer(frameId);
    internal bool ReturnRegisteredCompleteSharedSlashBenefit(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteSharedSlashBenefit(completion);
    internal PromptChoice SelectAiSharedSlashOffer(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiSharedSlashOffer(decision, f);
    internal IReadOnlyList<PromptChoice> SharedSlashChoices(ProgramSkillFrame f) => _resolve().SharedSlashChoices(f);
    internal bool SharedSlashGiftKindStillValid(ProgramSkillFrame f, Card card, CardLocation from) => _resolve().SharedSlashGiftKindStillValid(f, card, from);
    internal IEnumerable<IGameEvent> SharedSlashHistory() => _resolve().SharedSlashHistory();
    internal int[] SharedSlashLegalTargets(int actorSeat) => _resolve().SharedSlashLegalTargets(actorSeat);
    internal CardConversionSource SharedSlashSource(ProgramSkillFrame f) => _resolve().SharedSlashSource(f);
    internal bool SharedSlashUnchosenSourceValid(ProgramSkillFrame f) => _resolve().SharedSlashUnchosenSourceValid(f);
}
