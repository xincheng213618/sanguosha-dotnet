#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiZhiCaiOlEntries
{
    internal bool AllowsRegisteredIsXianfuEchoTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredIsXianfuEchoTrigger(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ArmXianfuRelation(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().ArmXianfuRelation(supplied, target);
    internal bool CanContinueIssuedXiZhiCai(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedXiZhiCai(f);
    internal bool CanOfferXianfuDamage(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CanOfferXianfuDamage(ownerSeat, trigger, attack);
    internal bool CanRunXianfuEcho(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunXianfuEcho(c, context);
    internal void CaptureChouceFrozenJudgment(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureChouceFrozenJudgment(payload);
    internal void CaptureXianfuRecoveryQualification(global::CardGame.Core.HpChangeContext change) => _resolve().CaptureXianfuRecoveryQualification(change);
    internal global::CardGame.Core.PromptChoice ChouceAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChouceAiChoice(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChouceCardOptions(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().ChouceCardOptions(f, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChouceChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChouceChoices(f);
    internal bool ChouceIsRed(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChouceIsRed(f);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? ChouceOriginalAttack(global::CardGame.Core.ChoucePointReceipt r) => _resolve().ChouceOriginalAttack(r);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectXianfuHpCandidates(global::CardGame.Core.HpChangeContext change) => _resolve().CollectXianfuHpCandidates(change);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateXianfuDamageContext(global::CardGame.Core.DamageTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateXianfuDamageContext(window, c);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateXianfuHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateXianfuHpContext(change, c);
    internal void FinishChouce(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishChouce(f);
    internal void FinishXianfuEcho(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXianfuEcho(f);
    internal bool IsXianfuEchoTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().IsXianfuEchoTrigger(trigger);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.XianfuRelationGrant> LiveXianfuRelations() => _resolve().LiveXianfuRelations();
    internal void ObserveXianfuDamageQualification(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveXianfuDamageQualification(payload);
    internal bool? ProveXianfuHealth(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveXianfuHealth(trigger, candidate, context);
    internal void PublishChouceDecision(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishChouceDecision(f);
    internal void ResolveChouceChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveChouceChoice(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveChouceFrozenJudgment(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveChouceFrozenJudgment(supplied, effect);
    internal bool ResumeChouce(long id) => _resolve().ResumeChouce(id);
    internal bool ResumeXianfuEcho(long id) => _resolve().ResumeXianfuEcho(id);
    internal bool ReturnChouceMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnChouceMovement(f);
    internal bool ReturnXianfuEchoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXianfuEchoMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleXianfuNativeEcho(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SettleXianfuNativeEcho(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome StartChouceDamagePointJudgment(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().StartChouceDamagePointJudgment(supplied, effect);
    internal bool TryAdvanceXiZhiCaiSubtree() => _resolve().TryAdvanceXiZhiCaiSubtree();
    internal bool ValidChouce(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidChouce(f);
    internal bool ValidXianfuEcho(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidXianfuEcho(f);
    internal bool XiZhiCaiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().XiZhiCaiFirstChild(root, child);
    internal bool XiZhiCaiObserverEdge(int index) => _resolve().XiZhiCaiObserverEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? XiZhiCaiObserverRoot() => _resolve().XiZhiCaiObserverRoot();
    internal global::CardGame.Core.CardConversionSource XiZhiCaiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XiZhiCaiSource(f);
    internal bool XianfuEchoConsumed(global::CardGame.Core.XianfuEchoQualification r) => _resolve().XianfuEchoConsumed(r);
    internal global::CardGame.Core.XianfuEchoQualification? XianfuQualification(global::CardGame.Core.ProgramTriggerCandidate c, long nativeId) => _resolve().XianfuQualification(c, nativeId);
    internal bool XianfuQualifiedSourceEnabled(global::CardGame.Core.XianfuEchoQualification r) => _resolve().XianfuQualifiedSourceEnabled(r);
    internal bool HasXianfuRelation { get => _resolve().HasXianfuRelation; }
}
