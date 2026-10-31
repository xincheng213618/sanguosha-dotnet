#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellMengpoProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHellWanghun(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHellWanghun(supplied);
    internal bool CanContinueHellWanghun(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueHellWanghun(frame);
    internal void CompleteHellShiyouDiscardTransfers(global::CardGame.Core.HellShiyouTransferPlan plan, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CompleteHellShiyouDiscardTransfers(plan, movements);
    internal void ContinueHellWanghun(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueHellWanghun(frame);
    internal bool HellEnemies(int first, int second) => _resolve().HellEnemies(first, second);
    internal global::CardGame.Core.CardLocation? HellShiyouOriginalSource(global::CardGame.Core.CardTransfer transfer, global::CardGame.Core.CardMoveReason reason) => _resolve().HellShiyouOriginalSource(transfer, reason);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillGrant> HellWanghunEligibleGrants(global::CardGame.Core.CharacterState player) => _resolve().HellWanghunEligibleGrants(player);
    internal bool HellWangshiCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().HellWangshiCardUseForbidden(query);
    internal bool IsHellShiyouPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason) => _resolve().IsHellShiyouPaidDiscardMovement(move, expectedSource, expectedReason);
    internal bool IsHellShiyouRedirectedDiscard(global::CardGame.Core.CardMovementRecord move) => _resolve().IsHellShiyouRedirectedDiscard(move);
    internal bool IsHellWangshiActionForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsHellWangshiActionForbidden(actorSeat, kind, type);
    internal global::CardGame.Core.SkillProgramStepOutcome IssueHellWangshiBan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueHellWangshiBan(supplied);
    internal global::CardGame.Core.HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardTransfer> input, global::CardGame.Core.CardMoveReason reason) => _resolve().PrepareHellShiyouDiscardTransfers(input, reason);
    internal bool ResumeHellWanghun(long id) => _resolve().ResumeHellWanghun(id);
    internal bool HasHellShiyouCapability { get => _resolve().HasHellShiyouCapability; }
}
