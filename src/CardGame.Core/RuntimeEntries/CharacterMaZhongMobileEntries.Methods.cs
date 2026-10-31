#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaZhongMobileEntries
{
    internal bool AdmitIssuedMobileFumanUse(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AdmitIssuedMobileFumanUse(candidate, context);
    internal bool CanOfferMobileFuman(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().CanOfferMobileFuman(gate);
    internal void CollectMobileFumanUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> candidates) => _resolve().CollectMobileFumanUse(action, window, candidates);
    internal bool ExactMobileFumanUseCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ExactMobileFumanUseCandidate(candidate, context);
    internal bool KeepIssuedMobileFuman(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepIssuedMobileFuman(frame);
    internal bool KeepMobileFumanPaidGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepMobileFumanPaidGift(frame);
    internal global::CardGame.Core.CardMovementRecord? MobileFumanGiftMovement(global::CardGame.Core.MobileMaZhongFumanPreparedEvent prepared) => _resolve().MobileFumanGiftMovement(prepared);
    internal bool MobileFumanGiftPaid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MobileFumanGiftPaid(frame);
    internal bool MobileFumanPreparedSourceValid(global::CardGame.Core.MobileMaZhongFumanPreparedEvent prepared) => _resolve().MobileFumanPreparedSourceValid(prepared);
    internal void PrepareMobileMaZhongFuman(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PrepareMobileMaZhongFuman(supplied);
    internal bool QualifyMobileFumanUse(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyMobileFumanUse(candidate, context);
}
