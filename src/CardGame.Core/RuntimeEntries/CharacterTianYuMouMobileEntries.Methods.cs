#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuMouMobileEntries
{
    internal void AddMouTianYuRangeBonusAttackRangeContribution(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().AddMouTianYuRangeBonusAttackRangeContribution(player, contributions);
    internal bool AdvanceMouTianYuChildren() => _resolve().AdvanceMouTianYuChildren();
    internal void AnswerMouTianYu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouTianYu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouTianYu(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMouTianYu(input, op);
    internal bool CanOfferMouTianYu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferMouTianYu(candidate, t, c);
    internal void CompleteMouTianYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteMouTianYu(f);
    internal void ContinueMouTianYu(long id) => _resolve().ContinueMouTianYu(id);
    internal bool ContinuePaidMouTianYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMouTianYu(f);
    internal bool DrainMouTianYu(long id) => _resolve().DrainMouTianYu(id);
    internal bool IsMouTianYuDying() => _resolve().IsMouTianYuDying();
    internal bool IsMouTianYuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouTianYuRecoveryParent(f, recovery);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouTianYuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouTianYuChoices(f);
    internal bool MouTianYuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouTianYuFirstChild(f, child);
    internal bool MouTianYuObserverEdge(int index) => _resolve().MouTianYuObserverEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? MouTianYuObserverRoot() => _resolve().MouTianYuObserverRoot();
    internal bool MouTianYuObserverSuffix(int index) => _resolve().MouTianYuObserverSuffix(index);
    internal void MutateMouTianYu(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MouTianYuStage next, global::System.Action mutation) => _resolve().MutateMouTianYu(f, next, mutation);
    internal void PublishMouTianYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouTianYu(f);
    internal bool ResumeMouTianYu(long id) => _resolve().ResumeMouTianYu(id);
    internal bool ReturnMouTianYuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouTianYuMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiMouTianYu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMouTianYu(p, f);
    internal bool ValidMouTianYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouTianYu(f);
    internal bool HasMouTianYu { get => _resolve().HasMouTianYu; }
    internal int MouTianYuActualTurn { get => _resolve().MouTianYuActualTurn; }
}
