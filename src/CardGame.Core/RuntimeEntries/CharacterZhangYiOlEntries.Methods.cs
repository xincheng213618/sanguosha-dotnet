#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangYiOlEntries
{
    internal bool AdvanceZhangYiSubtree() => _resolve().AdvanceZhangYiSubtree();
    internal PromptChoice AiZhangYi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiZhangYi(p, f);
    internal void AnswerZhangYi(PromptChoice choice) => _resolve().AnswerZhangYi(choice);
    internal void ApplyZhangYiChainedDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ApplyZhangYiChainedDamageBonus(attack);
    internal RuleQueryEvaluation ApplyZhangYiKangruiHandLimit(CharacterState player, RuleQueryEvaluation ordinary) => _resolve().ApplyZhangYiKangruiHandLimit(player, ordinary);
    internal SkillProgramStepOutcome BeginZhangYi(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginZhangYi(input, op);
    internal bool CanOfferZhangYi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZhangYi(candidate, trigger, context);
    internal bool ContinuePaidZhangYi(ProgramSkillFrame f) => _resolve().ContinuePaidZhangYi(f);
    internal void ContinueZhangYi(long id) => _resolve().ContinueZhangYi(id);
    internal bool DrainZhangYiChildren(long id) => _resolve().DrainZhangYiChildren(id);
    internal void FinishZhangYi(ProgramSkillFrame f) => _resolve().FinishZhangYi(f);
    internal bool IsFirstZhangYiOwnTurnDamage(ProgramSkillWindowContext c) => _resolve().IsFirstZhangYiOwnTurnDamage(c);
    internal bool IsZhangYiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsZhangYiRecoveryParent(f, recovery);
    internal void ObserveZhangYiActualDamage(IGameEvent payload) => _resolve().ObserveZhangYiActualDamage(payload);
    internal void PrepareZhangYiChainAdvance(GameEngine.IDamageAttempt attack) => _resolve().PrepareZhangYiChainAdvance(attack);
    internal void PublishZhangYi(ProgramSkillFrame f) => _resolve().PublishZhangYi(f);
    internal bool ResumeZhangYi(long id) => _resolve().ResumeZhangYi(id);
    internal void ResumeZhangYiDianjunExtraPlay(long programFrameId) => _resolve().ResumeZhangYiDianjunExtraPlay(programFrameId);
    internal void SetZhangYiChainAmount(GameEngine.IDamageAttempt attack, int amount) => _resolve().SetZhangYiChainAmount(attack, amount);
    internal int ZhangYiChainCursor(GameEngine.IDamageAttempt attack) => _resolve().ZhangYiChainCursor(attack);
    internal IReadOnlyList<PromptChoice> ZhangYiChoices(ProgramSkillFrame f) => _resolve().ZhangYiChoices(f);
    internal bool ZhangYiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangYiFirstChild(f, child);
    internal int ZhangYiKangruiDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ZhangYiKangruiDamageBonus(attack);
    internal ProgramSkillFrame? ZhangYiObserverRoot() => _resolve().ZhangYiObserverRoot();
    internal bool ZhangYiObserverSuffix(int root) => _resolve().ZhangYiObserverSuffix(root);
    internal CardConversionSource ZhangYiSource(ProgramSkillFrame f) => _resolve().ZhangYiSource(f);
    internal bool HasZhangYiCapability { get => _resolve().HasZhangYiCapability; }
}
