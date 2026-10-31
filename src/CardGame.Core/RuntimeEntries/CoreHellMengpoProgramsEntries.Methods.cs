#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellMengpoProgramsEntries
{
    internal SkillProgramStepOutcome BeginHellWanghun(ProgramSkillFrame supplied) => _resolve().BeginHellWanghun(supplied);
    internal bool CanContinueHellWanghun(ProgramSkillFrame frame) => _resolve().CanContinueHellWanghun(frame);
    internal void CompleteHellShiyouDiscardTransfers(HellShiyouTransferPlan plan, IReadOnlyList<CardMovementRecord> movements) => _resolve().CompleteHellShiyouDiscardTransfers(plan, movements);
    internal void ContinueHellWanghun(ProgramSkillFrame frame) => _resolve().ContinueHellWanghun(frame);
    internal bool HellEnemies(int first, int second) => _resolve().HellEnemies(first, second);
    internal CardLocation? HellShiyouOriginalSource(CardTransfer transfer, CardMoveReason reason) => _resolve().HellShiyouOriginalSource(transfer, reason);
    internal IReadOnlyList<SkillGrant> HellWanghunEligibleGrants(CharacterState player) => _resolve().HellWanghunEligibleGrants(player);
    internal bool HellWangshiCardUseForbidden(ProgramCardUseQuery query) => _resolve().HellWangshiCardUseForbidden(query);
    internal bool IsHellShiyouPaidDiscardMovement(CardMovementRecord move, CardLocation expectedSource, CardMoveReason expectedReason) => _resolve().IsHellShiyouPaidDiscardMovement(move, expectedSource, expectedReason);
    internal bool IsHellShiyouRedirectedDiscard(CardMovementRecord move) => _resolve().IsHellShiyouRedirectedDiscard(move);
    internal bool IsHellWangshiActionForbidden(int actorSeat, CardKind kind, CardActionType type) => _resolve().IsHellWangshiActionForbidden(actorSeat, kind, type);
    internal SkillProgramStepOutcome IssueHellWangshiBan(ProgramSkillFrame supplied) => _resolve().IssueHellWangshiBan(supplied);
    internal HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(IReadOnlyList<CardTransfer> input, CardMoveReason reason) => _resolve().PrepareHellShiyouDiscardTransfers(input, reason);
    internal bool ResumeHellWanghun(long id) => _resolve().ResumeHellWanghun(id);
    internal bool HasHellShiyouCapability { get => _resolve().HasHellShiyouCapability; }
}
