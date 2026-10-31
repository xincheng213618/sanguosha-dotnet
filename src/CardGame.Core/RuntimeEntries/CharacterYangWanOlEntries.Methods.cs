#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangWanOlEntries
{
    internal ActualDiscardRecoveryPhaseKey? ActualOwnYouyanDiscard(int owner, ProgramSkillWindowContext? context) => _resolve().ActualOwnYouyanDiscard(owner, context);
    internal bool CanOfferActualOwnYouyan(ProgramTriggerLegalityContext gate) => _resolve().CanOfferActualOwnYouyan(gate);
    internal IReadOnlyList<int> CollectZhuihuanPendingDamagers(int ownerSeat, int chosenSeat) => _resolve().CollectZhuihuanPendingDamagers(ownerSeat, chosenSeat);
    internal IReadOnlyList<long> PendingZhuihuanArmFrameIds(int ownerSeat, int chosenSeat) => _resolve().PendingZhuihuanArmFrameIds(ownerSeat, chosenSeat);
    internal int ProgramYouyanDiscardedSuitMask(int ownerSeat, CardMovementBatchContext batch) => _resolve().ProgramYouyanDiscardedSuitMask(ownerSeat, batch);
    internal void ResolveZhuihuanArmChoice(PromptChoice selected) => _resolve().ResolveZhuihuanArmChoice(selected);
    internal PromptChoice SelectAiZhuihuanArmChoice(PendingDecision decision) => _resolve().SelectAiZhuihuanArmChoice(decision);
    internal SkillProgramStepOutcome YouyanProgramGainSuitCards(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YouyanProgramGainSuitCards(frame, effect);
    internal SkillProgramStepOutcome ZhuihuanProgramArm(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhuihuanProgramArm(frame, effect);
    internal SkillProgramStepOutcome ZhuihuanProgramRetaliate(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhuihuanProgramRetaliate(frame, effect);
}
