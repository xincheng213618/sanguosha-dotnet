#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouJieAnniversaryEntries
{
    internal bool AdvanceAnniversaryXiahouJieObserver() => _resolve().AdvanceAnniversaryXiahouJieObserver();
    internal bool AnniversaryXiahouJieFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnniversaryXiahouJieFirstChild(f, child);
    internal ProgramSkillFrame? AnniversaryXiahouJieObserverRoot() => _resolve().AnniversaryXiahouJieObserverRoot();
    internal SkillProgramStepOutcome BeginAnniversaryXiahouJie(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginAnniversaryXiahouJie(supplied, operation);
    internal bool CanOfferAnniversaryXiahouJie(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferAnniversaryXiahouJie(c, t, w);
    internal bool CompleteAnniversaryXiahouJiePreparation(ProgramLifecycleTriggerWindowFrame parent) => _resolve().CompleteAnniversaryXiahouJiePreparation(parent);
    internal void ContinueAnniversaryXiahouJie(long id) => _resolve().ContinueAnniversaryXiahouJie(id);
    internal bool DrainAnniversaryXiahouJie(long id) => _resolve().DrainAnniversaryXiahouJie(id);
    internal void FinishAnniversaryXiahouJie(ProgramSkillFrame f) => _resolve().FinishAnniversaryXiahouJie(f);
    internal bool HasAnniversaryXiahouJieDying() => _resolve().HasAnniversaryXiahouJieDying();
    internal bool IsAnniversaryXiahouJieUniqueHand(int owner) => _resolve().IsAnniversaryXiahouJieUniqueHand(owner);
    internal bool KeepPaidAnniversaryXiahouJie(ProgramSkillFrame f) => _resolve().KeepPaidAnniversaryXiahouJie(f);
    internal bool ResumeAnniversaryXiahouJie(long id) => _resolve().ResumeAnniversaryXiahouJie(id);
    internal void ReturnAnniversaryXiahouJieDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnAnniversaryXiahouJieDraw(supplied, draw);
    internal bool ReturnAnniversaryXiahouJieMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryXiahouJieMovement(f);
    internal bool HasAnniversaryXiahouJie { get => _resolve().HasAnniversaryXiahouJie; }
}
