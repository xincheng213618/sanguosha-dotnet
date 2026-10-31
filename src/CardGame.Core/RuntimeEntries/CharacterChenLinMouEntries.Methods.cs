#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenLinMouEntries
{
    internal void ApplyMouChenLinChain(GameEngine.IDamageAttempt a) => _resolve().ApplyMouChenLinChain(a);
    internal int ApplyMouChenLinNextDamage(GameEngine.IDamageAttempt a) => _resolve().ApplyMouChenLinNextDamage(a);
    internal MouChenLinDamageTicketEvent[] AvailableMouChenLinTickets(GameEngine.IDamageAttempt a) => _resolve().AvailableMouChenLinTickets(a);
    internal void BindMouChenLinNativeUse(CardUseFrame use) => _resolve().BindMouChenLinNativeUse(use);
    internal bool CanStartMouChenLinInvitation(CharacterState owner) => _resolve().CanStartMouChenLinInvitation(owner);
    internal void ConsumeMouChenLinDamage(IGameEvent payload) => _resolve().ConsumeMouChenLinDamage(payload);
    internal bool IsMouChenLinHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsMouChenLinHandLimitExempt(owner, cardId);
    internal void IssueMouChenLinNonHandDonorTickets(ProgramSkillFrame f) => _resolve().IssueMouChenLinNonHandDonorTickets(f);
    internal void IssueMouChenLinUse(ProgramSkillFrame f) => _resolve().IssueMouChenLinUse(f);
    internal bool MouChenLinAdvanceChildren() => _resolve().MouChenLinAdvanceChildren();
    internal int MouChenLinDamageCursor(GameEngine.IDamageAttempt a) => _resolve().MouChenLinDamageCursor(a);
    internal MouChenLinDamageReceipt? MouChenLinDamageState(long id) => _resolve().MouChenLinDamageState(id);
    internal bool MouChenLinDamageTargets(ProgramTargetCardWindowQuery q) => _resolve().MouChenLinDamageTargets(q);
    internal bool MouChenLinDying() => _resolve().MouChenLinDying();
    internal bool MouChenLinEdge(int i) => _resolve().MouChenLinEdge(i);
    internal bool MouChenLinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouChenLinFirstChild(f, child);
    internal bool MouChenLinMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => _resolve().MouChenLinMovement(f, op, p);
    internal MouChenLinUsePlan? MouChenLinPlan(ProgramSkillFrame f, int id, int target) => _resolve().MouChenLinPlan(f, id, target);
    internal bool MouChenLinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouChenLinRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouChenLinRoot() => _resolve().MouChenLinRoot();
    internal bool MouChenLinSuffix(int n) => _resolve().MouChenLinSuffix(n);
    internal bool MouChenLinUseRidesOn(ProgramSkillFrame f, CardUseFrame u) => _resolve().MouChenLinUseRidesOn(f, u);
    internal void PrepareMouChenLinChain(GameEngine.IDamageAttempt a) => _resolve().PrepareMouChenLinChain(a);
    internal void RecordMouChenLinGift(ProgramSkillFrame f, int giver, int cardId, long before, IReadOnlyList<CardMovementRecord> movements) => _resolve().RecordMouChenLinGift(f, giver, cardId, before, movements);
    internal bool ResumeMouChenLinAttack() => _resolve().ResumeMouChenLinAttack();
    internal bool ReturnMouChenLinUse(CardUseFrame use) => _resolve().ReturnMouChenLinUse(use);
    internal void SetMouChenLinChainAmount(GameEngine.IDamageAttempt a, int amount) => _resolve().SetMouChenLinChainAmount(a, amount);
    internal void SetMouChenLinDamageState(long id, MouChenLinDamageReceipt? receipt) => _resolve().SetMouChenLinDamageState(id, receipt);
    internal bool ValidMouChenLin(ProgramSkillFrame f) => _resolve().ValidMouChenLin(f);
    internal void ValidateMouChenLinUse(CardUseFrame use) => _resolve().ValidateMouChenLinUse(use);
    internal bool HasMouChenLin { get => _resolve().HasMouChenLin; }
    internal MouChenLinRuntime MouChenLinRules { get => _resolve().MouChenLinRules; }
}
