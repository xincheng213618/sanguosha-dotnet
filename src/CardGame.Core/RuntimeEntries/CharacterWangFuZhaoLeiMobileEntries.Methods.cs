#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangFuZhaoLeiMobileEntries
{
    internal bool AdvanceXunyiSubtree() => _resolve().AdvanceXunyiSubtree();
    internal void BeginNextXunyiPoint(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginNextXunyiPoint(frame);
    internal bool CanOfferXunyi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferXunyi(candidate, trigger, context);
    internal void CaptureXunyiActualDamage(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureXunyiActualDamage(payload);
    internal void ChangeXunyiRelation(global::CardGame.Core.ProgramSkillFrame frame, bool transfer) => _resolve().ChangeXunyiRelation(frame, transfer);
    internal bool ContinueIssuedXunyi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueIssuedXunyi(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteXunyi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ExecuteXunyi(supplied, operation);
    internal bool HasXunyiBeforeDamageObserver(long id) => _resolve().HasXunyiBeforeDamageObserver(id);
    internal bool HasXunyiDamageObserver(long id) => _resolve().HasXunyiDamageObserver(id);
    internal bool IsXunyiMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation continuation) => _resolve().IsXunyiMovement(frame, effect, continuation);
    internal bool IsXunyiRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().IsXunyiRecoveryParent(frame, child);
    internal void PayXunyiPoint(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Card? card, global::CardGame.Core.CardLocation? from) => _resolve().PayXunyiPoint(frame, card, from);
    internal void ResolveXunyiDiscard(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveXunyiDiscard(choice);
    internal bool ResumeXunyi(long id) => _resolve().ResumeXunyi(id);
    internal bool ReturnXunyiMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnXunyiMovement(frame);
    internal void SynchronizeXunyiMarker(global::CardGame.Core.ProgramSkillFrame frame, int holder) => _resolve().SynchronizeXunyiMarker(frame, holder);
    internal bool ValidXunyi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidXunyi(frame);
    internal global::CardGame.Core.PromptChoice XunyiAiChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().XunyiAiChoice(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XunyiChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().XunyiChoices(frame);
    internal bool XunyiFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().XunyiFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? XunyiObserverRoot() => _resolve().XunyiObserverRoot();
    internal global::CardGame.Core.GameEngine.IDamageAttempt? XunyiOriginalAttack(global::CardGame.Core.XunyiDamageReceipt receipt) => _resolve().XunyiOriginalAttack(receipt);
    internal global::CardGame.Core.XunyiDamageReceipt? XunyiQualification(int owner, string skill, string instance, long damage) => _resolve().XunyiQualification(owner, skill, instance, damage);
    internal global::CardGame.Core.XunyiRelationGrant? XunyiRelation(int owner, string skill, string instance) => _resolve().XunyiRelation(owner, skill, instance);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.XunyiRelationGrant> XunyiRelations() => _resolve().XunyiRelations();
    internal bool TracksXunyi { get => _resolve().TracksXunyi; }
}
