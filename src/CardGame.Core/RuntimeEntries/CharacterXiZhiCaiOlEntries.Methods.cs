#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiZhiCaiOlEntries
{
    internal bool AllowsRegisteredIsXianfuEchoTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredIsXianfuEchoTrigger(gate);
    internal SkillProgramStepOutcome ArmXianfuRelation(ProgramSkillFrame supplied, int target) => _resolve().ArmXianfuRelation(supplied, target);
    internal bool CanContinueIssuedXiZhiCai(ProgramSkillFrame f) => _resolve().CanContinueIssuedXiZhiCai(f);
    internal bool CanOfferXianfuDamage(int ownerSeat, SkillProgramTrigger trigger, GameEngine.IDamageAttempt attack) => _resolve().CanOfferXianfuDamage(ownerSeat, trigger, attack);
    internal bool CanRunXianfuEcho(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunXianfuEcho(c, context);
    internal void CaptureChouceFrozenJudgment(IGameEvent payload) => _resolve().CaptureChouceFrozenJudgment(payload);
    internal void CaptureXianfuRecoveryQualification(HpChangeContext change) => _resolve().CaptureXianfuRecoveryQualification(change);
    internal PromptChoice ChouceAiChoice(ProgramSkillFrame f) => _resolve().ChouceAiChoice(f);
    internal IReadOnlyList<PromptChoice> ChouceCardOptions(ProgramSkillFrame f, int target) => _resolve().ChouceCardOptions(f, target);
    internal IReadOnlyList<PromptChoice> ChouceChoices(ProgramSkillFrame f) => _resolve().ChouceChoices(f);
    internal bool ChouceIsRed(ProgramSkillFrame f) => _resolve().ChouceIsRed(f);
    internal GameEngine.IDamageAttempt? ChouceOriginalAttack(ChoucePointReceipt r) => _resolve().ChouceOriginalAttack(r);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectXianfuHpCandidates(HpChangeContext change) => _resolve().CollectXianfuHpCandidates(change);
    internal ProgramSkillWindowContext CreateXianfuDamageContext(DamageTriggerWindowFrame window, ProgramTriggerCandidate c) => _resolve().CreateXianfuDamageContext(window, c);
    internal ProgramSkillWindowContext CreateXianfuHpContext(HpChangeContext change, ProgramTriggerCandidate c) => _resolve().CreateXianfuHpContext(change, c);
    internal void FinishChouce(ProgramSkillFrame f) => _resolve().FinishChouce(f);
    internal void FinishXianfuEcho(ProgramSkillFrame f) => _resolve().FinishXianfuEcho(f);
    internal bool IsXianfuEchoTrigger(SkillProgramTrigger trigger) => _resolve().IsXianfuEchoTrigger(trigger);
    internal IEnumerable<XianfuRelationGrant> LiveXianfuRelations() => _resolve().LiveXianfuRelations();
    internal void ObserveXianfuDamageQualification(IGameEvent payload) => _resolve().ObserveXianfuDamageQualification(payload);
    internal bool? ProveXianfuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveXianfuHealth(trigger, candidate, context);
    internal void PublishChouceDecision(ProgramSkillFrame f) => _resolve().PublishChouceDecision(f);
    internal void ResolveChouceChoice(PromptChoice choice) => _resolve().ResolveChouceChoice(choice);
    internal SkillProgramStepOutcome ResolveChouceFrozenJudgment(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ResolveChouceFrozenJudgment(supplied, effect);
    internal bool ResumeChouce(long id) => _resolve().ResumeChouce(id);
    internal bool ResumeXianfuEcho(long id) => _resolve().ResumeXianfuEcho(id);
    internal bool ReturnChouceMovement(ProgramSkillFrame f) => _resolve().ReturnChouceMovement(f);
    internal bool ReturnXianfuEchoMovement(ProgramSkillFrame f) => _resolve().ReturnXianfuEchoMovement(f);
    internal SkillProgramStepOutcome SettleXianfuNativeEcho(ProgramSkillFrame supplied) => _resolve().SettleXianfuNativeEcho(supplied);
    internal SkillProgramStepOutcome StartChouceDamagePointJudgment(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().StartChouceDamagePointJudgment(supplied, effect);
    internal bool TryAdvanceXiZhiCaiSubtree() => _resolve().TryAdvanceXiZhiCaiSubtree();
    internal bool ValidChouce(ProgramSkillFrame f) => _resolve().ValidChouce(f);
    internal bool ValidXianfuEcho(ProgramSkillFrame f) => _resolve().ValidXianfuEcho(f);
    internal bool XiZhiCaiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().XiZhiCaiFirstChild(root, child);
    internal bool XiZhiCaiObserverEdge(int index) => _resolve().XiZhiCaiObserverEdge(index);
    internal ProgramSkillFrame? XiZhiCaiObserverRoot() => _resolve().XiZhiCaiObserverRoot();
    internal CardConversionSource XiZhiCaiSource(ProgramSkillFrame f) => _resolve().XiZhiCaiSource(f);
    internal bool XianfuEchoConsumed(XianfuEchoQualification r) => _resolve().XianfuEchoConsumed(r);
    internal XianfuEchoQualification? XianfuQualification(ProgramTriggerCandidate c, long nativeId) => _resolve().XianfuQualification(c, nativeId);
    internal bool XianfuQualifiedSourceEnabled(XianfuEchoQualification r) => _resolve().XianfuQualifiedSourceEnabled(r);
    internal bool HasXianfuRelation { get => _resolve().HasXianfuRelation; }
}
