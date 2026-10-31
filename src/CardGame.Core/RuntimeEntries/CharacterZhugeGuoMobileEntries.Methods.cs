#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoMobileEntries
{
    internal bool AdvanceMobileQirangObserver() => _resolve().AdvanceMobileQirangObserver();
    internal void AssertMobileQirangGain(ProgramSkillFrame f) => _resolve().AssertMobileQirangGain(f);
    internal SkillProgramStepOutcome BeginMobileZhugeGuo(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileZhugeGuo(supplied, op);
    internal bool CanOfferMobileZhugeGuo(ProgramTriggerLegalityContext g) => _resolve().CanOfferMobileZhugeGuo(g);
    internal void CaptureMobileQirangUse(IGameEvent payload) => _resolve().CaptureMobileQirangUse(payload);
    internal MobileQirangGainReceipt? CurrentMobileQirangGain(int owner, int card) => _resolve().CurrentMobileQirangGain(owner, card);
    internal bool KeepMobileQirangGain(ProgramSkillFrame f) => _resolve().KeepMobileQirangGain(f);
    internal MobileQirangGainReceipt? MobileQirangCommittedGain(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().MobileQirangCommittedGain(c, context);
    internal bool MobileQirangDistance(ProgramCardDistanceQuery q) => _resolve().MobileQirangDistance(q);
    internal bool MobileQirangDyingProof() => _resolve().MobileQirangDyingProof();
    internal CardMovementRecord? MobileQirangEquipmentEntry(int owner, ProgramSkillWindowContext c) => _resolve().MobileQirangEquipmentEntry(owner, c);
    internal bool MobileQirangObserverChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MobileQirangObserverChild(f, child);
    internal ProgramSkillFrame? MobileQirangObserverRoot() => _resolve().MobileQirangObserverRoot();
    internal bool MobileQirangUsePaid(CardUseFrame use, MobileQirangUseReceipt q) => _resolve().MobileQirangUsePaid(use, q);
    internal CardSnapshot[] MobileYuhuaPrivateView(int viewer) => _resolve().MobileYuhuaPrivateView(viewer);
    internal bool ResumeMobileQirang(long id) => _resolve().ResumeMobileQirang(id);
}
