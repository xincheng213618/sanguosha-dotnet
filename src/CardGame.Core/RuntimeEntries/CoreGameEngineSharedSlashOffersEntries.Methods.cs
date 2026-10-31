#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSharedSlashOffersEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSharedSlashGift(long frameId, int actorSeat, string bind) => _resolve().BeginSharedSlashGift(frameId, actorSeat, bind);
    internal void BeginSharedSlashUse(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().BeginSharedSlashUse(f, targetSeat);
    internal void CompleteSharedSlashBenefit(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteSharedSlashBenefit(completion);
    internal bool IsSharedSlashGiftSelection(global::CardGame.Core.ProgramSkillFrame f, string bind) => _resolve().IsSharedSlashGiftSelection(f, bind);
    internal void IssueSharedSlashDraw(global::CardGame.Core.ProgramSkillFrame f, bool recipient) => _resolve().IssueSharedSlashDraw(f, recipient);
    internal bool MatchesProgramOwnedSelectionKind(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> kinds) => _resolve().MatchesProgramOwnedSelectionKind(f, bind, owner, card, from, kinds);
    internal void PublishSharedSlashOffer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSharedSlashOffer(f);
    internal void ResolveSharedSlashOfferChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSharedSlashOfferChoice(choice);
    internal bool ResumeSharedSlashOffer(long frameId) => _resolve().ResumeSharedSlashOffer(frameId);
    internal bool ReturnRegisteredCompleteSharedSlashBenefit(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteSharedSlashBenefit(completion);
    internal global::CardGame.Core.PromptChoice SelectAiSharedSlashOffer(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSharedSlashOffer(decision, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SharedSlashChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SharedSlashChoices(f);
    internal bool SharedSlashGiftKindStillValid(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().SharedSlashGiftKindStillValid(f, card, from);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> SharedSlashHistory() => _resolve().SharedSlashHistory();
    internal int[] SharedSlashLegalTargets(int actorSeat) => _resolve().SharedSlashLegalTargets(actorSeat);
    internal global::CardGame.Core.CardConversionSource SharedSlashSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SharedSlashSource(f);
    internal bool SharedSlashUnchosenSourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SharedSlashUnchosenSourceValid(f);
}
