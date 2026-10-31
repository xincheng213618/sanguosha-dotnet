#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangJiaoAnniversaryGodEntries
{
    internal global::CardGame.Core.PromptChoice AnniversaryTianjieChoice(global::CardGame.Core.ProgramSkillFrame f, int? target) => _resolve().AnniversaryTianjieChoice(f, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversaryTianjieChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryTianjieChoices(f);
    internal int AnniversaryZjRank(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().AnniversaryZjRank(owner, context);
    internal bool AnniversaryZjTurnShuffled() => _resolve().AnniversaryZjTurnShuffled();
    internal void AnswerAnniversaryTianjie(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryTianjie(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryZj(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginAnniversaryZj(supplied, e);
    internal bool CanTriggerAnniversaryZj(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanTriggerAnniversaryZj(g);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryTianjie(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversaryTianjie(p, f);
    internal void ContinueAnniversaryZj(long id) => _resolve().ContinueAnniversaryZj(id);
    internal bool ContinueIssuedAnniversaryZj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedAnniversaryZj(f);
    internal bool DrainAnniversaryZj(long id) => _resolve().DrainAnniversaryZj(id);
    internal void FinishAnniversaryZj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishAnniversaryZj(f);
    internal bool HasAnniversaryTianjieDeadOwnerEnding(int actualOwner) => _resolve().HasAnniversaryTianjieDeadOwnerEnding(actualOwner);
    internal bool IsAnniversaryTianjieDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int actualOwner) => _resolve().IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, actualOwner);
    internal void ObtainAnniversaryZjCards(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ObtainAnniversaryZjCards(supplied);
    internal bool ProveIsAnniversaryTianjieDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().ProveIsAnniversaryTianjieDeadOwnerEndingCandidate(candidate);
    internal void PublishAnniversaryTianjie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversaryTianjie(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveAnniversarySanshou(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect e) => _resolve().ResolveAnniversarySanshou(f, e);
    internal bool ResumeAnniversaryZj(long id) => _resolve().ResumeAnniversaryZj(id);
    internal bool ReturnAnniversaryZjMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryZjMovement(f);
    internal int[] SampleAnniversarySijun(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pool) => _resolve().SampleAnniversarySijun(pool);
}
