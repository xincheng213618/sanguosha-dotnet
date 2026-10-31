#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangJiaoAnniversaryGodEntries
{
    internal PromptChoice AnniversaryTianjieChoice(ProgramSkillFrame f, int? target) => _resolve().AnniversaryTianjieChoice(f, target);
    internal IReadOnlyList<PromptChoice> AnniversaryTianjieChoices(ProgramSkillFrame f) => _resolve().AnniversaryTianjieChoices(f);
    internal int AnniversaryZjRank(int owner, ProgramSkillWindowContext? context) => _resolve().AnniversaryZjRank(owner, context);
    internal bool AnniversaryZjTurnShuffled() => _resolve().AnniversaryZjTurnShuffled();
    internal void AnswerAnniversaryTianjie(PromptChoice choice) => _resolve().AnswerAnniversaryTianjie(choice);
    internal SkillProgramStepOutcome BeginAnniversaryZj(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().BeginAnniversaryZj(supplied, e);
    internal bool CanTriggerAnniversaryZj(ProgramTriggerLegalityContext g) => _resolve().CanTriggerAnniversaryZj(g);
    internal PromptChoice ChooseAnniversaryTianjie(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversaryTianjie(p, f);
    internal void ContinueAnniversaryZj(long id) => _resolve().ContinueAnniversaryZj(id);
    internal bool ContinueIssuedAnniversaryZj(ProgramSkillFrame f) => _resolve().ContinueIssuedAnniversaryZj(f);
    internal bool DrainAnniversaryZj(long id) => _resolve().DrainAnniversaryZj(id);
    internal void FinishAnniversaryZj(ProgramSkillFrame f) => _resolve().FinishAnniversaryZj(f);
    internal bool HasAnniversaryTianjieDeadOwnerEnding(int actualOwner) => _resolve().HasAnniversaryTianjieDeadOwnerEnding(actualOwner);
    internal bool IsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int actualOwner) => _resolve().IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, actualOwner);
    internal void ObtainAnniversaryZjCards(ProgramSkillFrame supplied) => _resolve().ObtainAnniversaryZjCards(supplied);
    internal bool ProveIsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate) => _resolve().ProveIsAnniversaryTianjieDeadOwnerEndingCandidate(candidate);
    internal void PublishAnniversaryTianjie(ProgramSkillFrame f) => _resolve().PublishAnniversaryTianjie(f);
    internal SkillProgramStepOutcome ResolveAnniversarySanshou(ProgramSkillFrame f, SkillProgramEffect e) => _resolve().ResolveAnniversarySanshou(f, e);
    internal bool ResumeAnniversaryZj(long id) => _resolve().ResumeAnniversaryZj(id);
    internal bool ReturnAnniversaryZjMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryZjMovement(f);
    internal int[] SampleAnniversarySijun(IReadOnlyList<Card> pool) => _resolve().SampleAnniversarySijun(pool);
}
