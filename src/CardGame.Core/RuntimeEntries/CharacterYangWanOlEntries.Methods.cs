#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangWanOlEntries
{
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? ActualOwnYouyanDiscard(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().ActualOwnYouyanDiscard(owner, context);
    internal bool CanOfferActualOwnYouyan(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferActualOwnYouyan(gate);
    internal global::System.Collections.Generic.IReadOnlyList<int> CollectZhuihuanPendingDamagers(int ownerSeat, int chosenSeat) => _resolve().CollectZhuihuanPendingDamagers(ownerSeat, chosenSeat);
    internal bool IsValidZhuihuanContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().IsValidZhuihuanContinuation(frame, paused);
    internal global::System.Collections.Generic.IReadOnlyList<long> PendingZhuihuanArmFrameIds(int ownerSeat, int chosenSeat) => _resolve().PendingZhuihuanArmFrameIds(ownerSeat, chosenSeat);
    internal int ProgramYouyanDiscardedSuitMask(int ownerSeat, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ProgramYouyanDiscardedSuitMask(ownerSeat, batch);
    internal void ResolveZhuihuanArmChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhuihuanArmChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiZhuihuanArmChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiZhuihuanArmChoice(decision);
    internal global::CardGame.Core.SkillProgramStepOutcome YouyanProgramGainSuitCards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YouyanProgramGainSuitCards(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhuihuanProgramArm(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhuihuanProgramArm(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhuihuanProgramRetaliate(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhuihuanProgramRetaliate(frame, effect);
}
