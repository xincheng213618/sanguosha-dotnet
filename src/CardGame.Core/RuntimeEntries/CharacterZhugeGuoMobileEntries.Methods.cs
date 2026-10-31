#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoMobileEntries
{
    internal bool AdvanceMobileQirangObserver() => _resolve().AdvanceMobileQirangObserver();
    internal void AssertMobileQirangGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileQirangGain(f);
    internal void AssertMobileZhugeGuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileZhugeGuo(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileZhugeGuo(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileZhugeGuo(supplied, op);
    internal bool CanOfferMobileZhugeGuo(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferMobileZhugeGuo(g);
    internal void CaptureMobileQirangUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureMobileQirangUse(payload);
    internal global::CardGame.Core.MobileQirangGainReceipt? CurrentMobileQirangGain(int owner, int card) => _resolve().CurrentMobileQirangGain(owner, card);
    internal bool KeepMobileQirangGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileQirangGain(f);
    internal global::CardGame.Core.MobileQirangGainReceipt? MobileQirangCommittedGain(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MobileQirangCommittedGain(c, context);
    internal bool MobileQirangDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().MobileQirangDistance(q);
    internal bool MobileQirangDyingProof() => _resolve().MobileQirangDyingProof();
    internal global::CardGame.Core.CardMovementRecord? MobileQirangEquipmentEntry(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().MobileQirangEquipmentEntry(owner, c);
    internal bool MobileQirangObserverChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileQirangObserverChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MobileQirangObserverRoot() => _resolve().MobileQirangObserverRoot();
    internal bool MobileQirangUsePaid(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MobileQirangUseReceipt q) => _resolve().MobileQirangUsePaid(use, q);
    internal global::CardGame.Core.CardSnapshot[] MobileYuhuaPrivateView(int viewer) => _resolve().MobileYuhuaPrivateView(viewer);
    internal bool ResumeMobileQirang(long id) => _resolve().ResumeMobileQirang(id);
}
