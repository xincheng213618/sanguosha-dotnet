#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuMouMobileEntries
{
    internal void AddMouTianYuRangeBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => _resolve().AddMouTianYuRangeBonusAttackRangeContribution(player, contributions);
    internal bool AdvanceMouTianYuChildren() => _resolve().AdvanceMouTianYuChildren();
    internal void AnswerMouTianYu(PromptChoice choice) => _resolve().AnswerMouTianYu(choice);
    internal SkillProgramStepOutcome BeginMouTianYu(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginMouTianYu(input, op);
    internal bool CanOfferMouTianYu(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferMouTianYu(candidate, t, c);
    internal void CompleteMouTianYu(ProgramSkillFrame f) => _resolve().CompleteMouTianYu(f);
    internal void ContinueMouTianYu(long id) => _resolve().ContinueMouTianYu(id);
    internal bool ContinuePaidMouTianYu(ProgramSkillFrame f) => _resolve().ContinuePaidMouTianYu(f);
    internal bool DrainMouTianYu(long id) => _resolve().DrainMouTianYu(id);
    internal bool IsMouTianYuDying() => _resolve().IsMouTianYuDying();
    internal bool IsMouTianYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouTianYuRecoveryParent(f, recovery);
    internal IReadOnlyList<PromptChoice> MouTianYuChoices(ProgramSkillFrame f) => _resolve().MouTianYuChoices(f);
    internal bool MouTianYuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouTianYuFirstChild(f, child);
    internal bool MouTianYuObserverEdge(int index) => _resolve().MouTianYuObserverEdge(index);
    internal ProgramSkillFrame? MouTianYuObserverRoot() => _resolve().MouTianYuObserverRoot();
    internal bool MouTianYuObserverSuffix(int index) => _resolve().MouTianYuObserverSuffix(index);
    internal void MutateMouTianYu(ProgramSkillFrame f, MouTianYuStage next, Action mutation) => _resolve().MutateMouTianYu(f, next, mutation);
    internal void PublishMouTianYu(ProgramSkillFrame f) => _resolve().PublishMouTianYu(f);
    internal bool ResumeMouTianYu(long id) => _resolve().ResumeMouTianYu(id);
    internal bool ReturnMouTianYuMovement(ProgramSkillFrame f) => _resolve().ReturnMouTianYuMovement(f);
    internal PromptChoice SelectAiMouTianYu(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiMouTianYu(p, f);
    internal bool ValidMouTianYu(ProgramSkillFrame f) => _resolve().ValidMouTianYu(f);
    internal bool HasMouTianYu { get => _resolve().HasMouTianYu; }
    internal int MouTianYuActualTurn { get => _resolve().MouTianYuActualTurn; }
}
