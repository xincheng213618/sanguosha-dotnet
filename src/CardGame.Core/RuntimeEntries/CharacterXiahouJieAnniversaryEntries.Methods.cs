#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouJieAnniversaryEntries
{
    internal bool AdvanceAnniversaryXiahouJieObserver() => _resolve().AdvanceAnniversaryXiahouJieObserver();
    internal bool AnniversaryXiahouJieFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryXiahouJieFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryXiahouJieObserverRoot() => _resolve().AnniversaryXiahouJieObserverRoot();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryXiahouJie(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginAnniversaryXiahouJie(supplied, operation);
    internal bool CanOfferAnniversaryXiahouJie(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferAnniversaryXiahouJie(c, t, w);
    internal long CaptureAnniversaryXiahouJieQualificationStamp(long underlying) => _resolve().CaptureAnniversaryXiahouJieQualificationStamp(underlying);
    internal bool CompleteAnniversaryXiahouJiePreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame parent) => _resolve().CompleteAnniversaryXiahouJiePreparation(parent);
    internal void ContinueAnniversaryXiahouJie(long id) => _resolve().ContinueAnniversaryXiahouJie(id);
    internal bool DrainAnniversaryXiahouJie(long id) => _resolve().DrainAnniversaryXiahouJie(id);
    internal void ExpireAnniversaryXiahouJie(global::CardGame.Core.TurnEndedEvent ended) => _resolve().ExpireAnniversaryXiahouJie(ended);
    internal void FinishAnniversaryXiahouJie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishAnniversaryXiahouJie(f);
    internal bool HasAnniversaryXiahouJieDying() => _resolve().HasAnniversaryXiahouJieDying();
    internal bool IsAnniversaryXiahouJieGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsAnniversaryXiahouJieGrantQualified(owner, grant);
    internal bool IsAnniversaryXiahouJieUniqueHand(int owner) => _resolve().IsAnniversaryXiahouJieUniqueHand(owner);
    internal bool KeepPaidAnniversaryXiahouJie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidAnniversaryXiahouJie(f);
    internal void RequireAnniversaryXiahouJie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireAnniversaryXiahouJie(f);
    internal bool ResumeAnniversaryXiahouJie(long id) => _resolve().ResumeAnniversaryXiahouJie(id);
    internal void ReturnAnniversaryXiahouJieDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnAnniversaryXiahouJieDraw(supplied, draw);
    internal bool ReturnAnniversaryXiahouJieMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryXiahouJieMovement(f);
    internal bool HasAnniversaryXiahouJie { get => _resolve().HasAnniversaryXiahouJie; }
}
