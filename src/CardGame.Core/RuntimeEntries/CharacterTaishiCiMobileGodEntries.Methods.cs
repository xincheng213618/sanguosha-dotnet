#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMobileGodEntries
{
    internal void AnswerShenTscPowei(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShenTscPowei(choice);
    internal bool CanOfferShenTaishiCi(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShenTaishiCi(gate);
    internal global::CardGame.Core.PromptChoice ChooseShenTscPowei(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShenTscPowei(p, f);
    internal bool ContinuePaidShenTscPowei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShenTscPowei(f);
    internal void ContinueShenTscPowei(long id) => _resolve().ContinueShenTscPowei(id);
    internal bool DrainShenTscPowei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShenTscPowei(f);
    internal void FinishShenTscPowei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenTscPowei(f);
    internal void GrantShenTscRange(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GrantShenTscRange(f);
    internal bool HasShenTaishiCiPoweiRange(int actor, int target) => _resolve().HasShenTaishiCiPoweiRange(actor, target);
    internal void PublishShenTscPowei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenTscPowei(f);
    internal void ResolveShenTscDulie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveShenTscDulie(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShenTscPowei(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveShenTscPowei(supplied);
    internal bool ResumeShenTscPowei(long id) => _resolve().ResumeShenTscPowei(id);
    internal bool ReturnShenTscPowei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenTscPowei(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenTscChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenTscChoices(f);
    internal int[] ShenTscDiscardCards(int owner) => _resolve().ShenTscDiscardCards(owner);
    internal bool ShenTscMayTake(int owner, int actor) => _resolve().ShenTscMayTake(owner, actor);
    internal bool ShenTscMissionOver(int owner) => _resolve().ShenTscMissionOver(owner);
    internal bool ShenTscPoweiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShenTscPoweiMovement(f, e, p);
    internal bool ShenTscPoweiSlashRange(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().ShenTscPoweiSlashRange(q);
    internal void ShenTscSetWei(global::CardGame.Core.ProgramSkillFrame f, int seat, int count) => _resolve().ShenTscSetWei(f, seat, count);
    internal int ShenTscWei(int seat, int owner) => _resolve().ShenTscWei(seat, owner);
    internal bool TryCompleteShenTaishiCiCancelledSlash(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryCompleteShenTaishiCiCancelledSlash(window);
}
