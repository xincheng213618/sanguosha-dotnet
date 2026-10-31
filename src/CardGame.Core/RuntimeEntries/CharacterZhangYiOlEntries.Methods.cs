#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangYiOlEntries
{
    internal bool AdvanceZhangYiSubtree() => _resolve().AdvanceZhangYiSubtree();
    internal global::CardGame.Core.PromptChoice AiZhangYi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiZhangYi(p, f);
    internal void AnswerZhangYi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerZhangYi(choice);
    internal void ApplyZhangYiChainedDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ApplyZhangYiChainedDamageBonus(attack);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyZhangYiKangruiHandLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().ApplyZhangYiKangruiHandLimit(player, ordinary);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhangYi(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginZhangYi(input, op);
    internal bool CanOfferZhangYi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhangYi(candidate, trigger, context);
    internal bool ContinuePaidZhangYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidZhangYi(f);
    internal void ContinueZhangYi(long id) => _resolve().ContinueZhangYi(id);
    internal bool DrainZhangYiChildren(long id) => _resolve().DrainZhangYiChildren(id);
    internal void FinishZhangYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhangYi(f);
    internal bool IsFirstZhangYiOwnTurnDamage(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsFirstZhangYiOwnTurnDamage(c);
    internal bool IsZhangYiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsZhangYiRecoveryParent(f, recovery);
    internal void ObserveZhangYiActualDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveZhangYiActualDamage(payload);
    internal void PrepareZhangYiChainAdvance(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().PrepareZhangYiChainAdvance(attack);
    internal void PublishZhangYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangYi(f);
    internal bool ResumeZhangYi(long id) => _resolve().ResumeZhangYi(id);
    internal void ResumeZhangYiDianjunExtraPlay(long programFrameId) => _resolve().ResumeZhangYiDianjunExtraPlay(programFrameId);
    internal void SetZhangYiChainAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().SetZhangYiChainAmount(attack, amount);
    internal int ZhangYiChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ZhangYiChainCursor(attack);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangYiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangYiChoices(f);
    internal bool ZhangYiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangYiFirstChild(f, child);
    internal int ZhangYiKangruiDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ZhangYiKangruiDamageBonus(attack);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangYiObserverRoot() => _resolve().ZhangYiObserverRoot();
    internal bool ZhangYiObserverSuffix(int root) => _resolve().ZhangYiObserverSuffix(root);
    internal global::CardGame.Core.CardConversionSource ZhangYiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangYiSource(f);
    internal bool HasZhangYiCapability { get => _resolve().HasZhangYiCapability; }
}
