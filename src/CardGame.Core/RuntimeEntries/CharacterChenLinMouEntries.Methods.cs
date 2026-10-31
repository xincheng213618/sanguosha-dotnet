#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenLinMouEntries
{
    internal void ApplyMouChenLinChain(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().ApplyMouChenLinChain(a);
    internal int ApplyMouChenLinNextDamage(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().ApplyMouChenLinNextDamage(a);
    internal global::CardGame.Core.MouChenLinDamageTicketEvent[] AvailableMouChenLinTickets(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().AvailableMouChenLinTickets(a);
    internal void BindMouChenLinNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouChenLinNativeUse(use);
    internal bool CanStartMouChenLinInvitation(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouChenLinInvitation(owner);
    internal void ConsumeMouChenLinDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeMouChenLinDamage(payload);
    internal bool IsMouChenLinHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsMouChenLinHandLimitExempt(owner, cardId);
    internal void IssueMouChenLinNonHandDonorTickets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouChenLinNonHandDonorTickets(f);
    internal void IssueMouChenLinUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouChenLinUse(f);
    internal bool MouChenLinAdvanceChildren() => _resolve().MouChenLinAdvanceChildren();
    internal int MouChenLinDamageCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().MouChenLinDamageCursor(a);
    internal global::CardGame.Core.MouChenLinDamageReceipt? MouChenLinDamageState(long id) => _resolve().MouChenLinDamageState(id);
    internal bool MouChenLinDamageTargets(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().MouChenLinDamageTargets(q);
    internal bool MouChenLinDying() => _resolve().MouChenLinDying();
    internal bool MouChenLinEdge(int i) => _resolve().MouChenLinEdge(i);
    internal bool MouChenLinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouChenLinFirstChild(f, child);
    internal bool MouChenLinMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouChenLinMovement(f, op, p);
    internal global::CardGame.Core.MouChenLinUsePlan? MouChenLinPlan(global::CardGame.Core.ProgramSkillFrame f, int id, int target) => _resolve().MouChenLinPlan(f, id, target);
    internal bool MouChenLinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouChenLinRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouChenLinRoot() => _resolve().MouChenLinRoot();
    internal bool MouChenLinSuffix(int n) => _resolve().MouChenLinSuffix(n);
    internal bool MouChenLinUseRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame u) => _resolve().MouChenLinUseRidesOn(f, u);
    internal void PrepareMouChenLinChain(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().PrepareMouChenLinChain(a);
    internal void RecordMouChenLinGift(global::CardGame.Core.ProgramSkillFrame f, int giver, int cardId, long before, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().RecordMouChenLinGift(f, giver, cardId, before, movements);
    internal bool ResumeMouChenLinAttack() => _resolve().ResumeMouChenLinAttack();
    internal bool ReturnMouChenLinUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouChenLinUse(use);
    internal void SetMouChenLinChainAmount(global::CardGame.Core.GameEngine.IDamageAttempt a, int amount) => _resolve().SetMouChenLinChainAmount(a, amount);
    internal void SetMouChenLinDamageState(long id, global::CardGame.Core.MouChenLinDamageReceipt? receipt) => _resolve().SetMouChenLinDamageState(id, receipt);
    internal bool ValidMouChenLin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouChenLin(f);
    internal void ValidateMouChenLinUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouChenLinUse(use);
    internal bool HasMouChenLin { get => _resolve().HasMouChenLin; }
    internal global::CardGame.Core.MouChenLinRuntime MouChenLinRules { get => _resolve().MouChenLinRules; }
}
