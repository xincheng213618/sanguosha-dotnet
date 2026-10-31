#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaZhongMobileEntries
{
    internal bool AdmitIssuedMobileFumanUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().AdmitIssuedMobileFumanUse(candidate, context);
    internal bool CanOfferMobileFuman(ProgramActivationLegalityContext gate) => _resolve().CanOfferMobileFuman(gate);
    internal void CollectMobileFumanUse(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates) => _resolve().CollectMobileFumanUse(action, window, candidates);
    internal bool ExactMobileFumanUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ExactMobileFumanUseCandidate(candidate, context);
    internal bool KeepIssuedMobileFuman(ProgramSkillFrame frame) => _resolve().KeepIssuedMobileFuman(frame);
    internal bool KeepMobileFumanPaidGift(ProgramSkillFrame frame) => _resolve().KeepMobileFumanPaidGift(frame);
    internal CardMovementRecord? MobileFumanGiftMovement(MobileMaZhongFumanPreparedEvent prepared) => _resolve().MobileFumanGiftMovement(prepared);
    internal bool MobileFumanGiftPaid(ProgramSkillFrame frame) => _resolve().MobileFumanGiftPaid(frame);
    internal bool MobileFumanPreparedSourceValid(MobileMaZhongFumanPreparedEvent prepared) => _resolve().MobileFumanPreparedSourceValid(prepared);
    internal void PrepareMobileMaZhongFuman(ProgramSkillFrame supplied) => _resolve().PrepareMobileMaZhongFuman(supplied);
    internal bool QualifyMobileFumanUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().QualifyMobileFumanUse(candidate, context);
}
