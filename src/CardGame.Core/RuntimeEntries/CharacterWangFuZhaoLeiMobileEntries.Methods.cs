#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangFuZhaoLeiMobileEntries
{
    internal bool AdvanceXunyiSubtree() => _resolve().AdvanceXunyiSubtree();
    internal void BeginNextXunyiPoint(ProgramSkillFrame frame) => _resolve().BeginNextXunyiPoint(frame);
    internal bool CanOfferXunyi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferXunyi(candidate, trigger, context);
    internal void CaptureXunyiActualDamage(IGameEvent payload) => _resolve().CaptureXunyiActualDamage(payload);
    internal void ChangeXunyiRelation(ProgramSkillFrame frame, bool transfer) => _resolve().ChangeXunyiRelation(frame, transfer);
    internal bool ContinueIssuedXunyi(ProgramSkillFrame frame) => _resolve().ContinueIssuedXunyi(frame);
    internal SkillProgramStepOutcome ExecuteXunyi(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().ExecuteXunyi(supplied, operation);
    internal bool IsXunyiRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame child) => _resolve().IsXunyiRecoveryParent(frame, child);
    internal void PayXunyiPoint(ProgramSkillFrame frame, Card? card, CardLocation? from) => _resolve().PayXunyiPoint(frame, card, from);
    internal void ResolveXunyiDiscard(PromptChoice choice) => _resolve().ResolveXunyiDiscard(choice);
    internal bool ResumeXunyi(long id) => _resolve().ResumeXunyi(id);
    internal bool ReturnXunyiMovement(ProgramSkillFrame frame) => _resolve().ReturnXunyiMovement(frame);
    internal void SynchronizeXunyiMarker(ProgramSkillFrame frame, int holder) => _resolve().SynchronizeXunyiMarker(frame, holder);
    internal bool ValidXunyi(ProgramSkillFrame frame) => _resolve().ValidXunyi(frame);
    internal PromptChoice XunyiAiChoice(ProgramSkillFrame frame) => _resolve().XunyiAiChoice(frame);
    internal IReadOnlyList<PromptChoice> XunyiChoices(ProgramSkillFrame frame) => _resolve().XunyiChoices(frame);
    internal bool XunyiFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().XunyiFirstChild(frame, child);
    internal ProgramSkillFrame? XunyiObserverRoot() => _resolve().XunyiObserverRoot();
    internal GameEngine.IDamageAttempt? XunyiOriginalAttack(XunyiDamageReceipt receipt) => _resolve().XunyiOriginalAttack(receipt);
    internal XunyiDamageReceipt? XunyiQualification(int owner, string skill, string instance, long damage) => _resolve().XunyiQualification(owner, skill, instance, damage);
    internal XunyiRelationGrant? XunyiRelation(int owner, string skill, string instance) => _resolve().XunyiRelation(owner, skill, instance);
    internal IEnumerable<XunyiRelationGrant> XunyiRelations() => _resolve().XunyiRelations();
    internal bool TracksXunyi { get => _resolve().TracksXunyi; }
}
