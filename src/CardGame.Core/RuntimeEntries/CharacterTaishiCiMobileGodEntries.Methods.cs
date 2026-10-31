#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMobileGodEntries
{
    internal void AnswerShenTscPowei(PromptChoice choice) => _resolve().AnswerShenTscPowei(choice);
    internal bool CanOfferShenTaishiCi(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShenTaishiCi(gate);
    internal PromptChoice ChooseShenTscPowei(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShenTscPowei(p, f);
    internal bool ContinuePaidShenTscPowei(ProgramSkillFrame f) => _resolve().ContinuePaidShenTscPowei(f);
    internal void ContinueShenTscPowei(long id) => _resolve().ContinueShenTscPowei(id);
    internal bool DrainShenTscPowei(ProgramSkillFrame f) => _resolve().DrainShenTscPowei(f);
    internal void FinishShenTscPowei(ProgramSkillFrame f) => _resolve().FinishShenTscPowei(f);
    internal void GrantShenTscRange(ProgramSkillFrame f) => _resolve().GrantShenTscRange(f);
    internal bool HasShenTaishiCiPoweiRange(int actor, int target) => _resolve().HasShenTaishiCiPoweiRange(actor, target);
    internal void PublishShenTscPowei(ProgramSkillFrame f) => _resolve().PublishShenTscPowei(f);
    internal void ResolveShenTscDulie(ProgramSkillFrame f) => _resolve().ResolveShenTscDulie(f);
    internal SkillProgramStepOutcome ResolveShenTscPowei(ProgramSkillFrame supplied) => _resolve().ResolveShenTscPowei(supplied);
    internal bool ResumeShenTscPowei(long id) => _resolve().ResumeShenTscPowei(id);
    internal bool ReturnShenTscPowei(ProgramSkillFrame f) => _resolve().ReturnShenTscPowei(f);
    internal IReadOnlyList<PromptChoice> ShenTscChoices(ProgramSkillFrame f) => _resolve().ShenTscChoices(f);
    internal int[] ShenTscDiscardCards(int owner) => _resolve().ShenTscDiscardCards(owner);
    internal bool ShenTscMayTake(int owner, int actor) => _resolve().ShenTscMayTake(owner, actor);
    internal bool ShenTscMissionOver(int owner) => _resolve().ShenTscMissionOver(owner);
    internal bool ShenTscPoweiSlashRange(ProgramSlashDistanceQuery q) => _resolve().ShenTscPoweiSlashRange(q);
    internal void ShenTscSetWei(ProgramSkillFrame f, int seat, int count) => _resolve().ShenTscSetWei(f, seat, count);
    internal int ShenTscWei(int seat, int owner) => _resolve().ShenTscWei(seat, owner);
    internal bool TryCompleteShenTaishiCiCancelledSlash(ProgramCardTriggerWindowFrame window) => _resolve().TryCompleteShenTaishiCiCancelledSlash(window);
}
