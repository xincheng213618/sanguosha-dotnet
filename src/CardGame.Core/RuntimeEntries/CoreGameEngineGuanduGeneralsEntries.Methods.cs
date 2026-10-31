#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGuanduGeneral(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginGuanduGeneral(supplied, operation);
    internal void CaptureGuanduDiscardBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureGuanduDiscardBatch(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureGuanduDiscardBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureGuanduDiscardBatch(batch, completed);
    internal bool ContinueIssuedGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedGuanduGeneral(f);
    internal bool DrainGuanduGeneralChildren(long id) => _resolve().DrainGuanduGeneralChildren(id);
    internal void DrawGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, int recipient, int count, string next) => _resolve().DrawGuanduGeneral(f, recipient, count, next);
    internal void FinishGuanduGeneral(global::CardGame.Core.ProgramSkillFrame f, bool completed = true) => _resolve().FinishGuanduGeneral(f, completed);
    internal global::CardGame.Core.Card[] GuanduDiscardable(int chooser, int owner) => _resolve().GuanduDiscardable(chooser, owner);
    internal bool GuanduGeneralGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().GuanduGeneralGrantQualified(owner, grant);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> GuanduGeneralRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().GuanduGeneralRuleContributions(player, query);
    internal global::CardGame.Core.Card[] GuanduOwnedCards(int seat, bool judgment = false) => _resolve().GuanduOwnedCards(seat, judgment);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> GuanduShicaiPrivatelyViewedCards(int viewerSeat) => _resolve().GuanduShicaiPrivatelyViewedCards(viewerSeat);
    internal bool GuanduShicaiReady(int ownerSeat, string instance) => _resolve().GuanduShicaiReady(ownerSeat, instance);
    internal bool GuanduSkillEnabled(global::CardGame.Core.CharacterState owner, string skill) => _resolve().GuanduSkillEnabled(owner, skill);
    internal global::CardGame.Core.ProgramSkillFrame GuanduStage(global::CardGame.Core.ProgramSkillFrame f, string stage) => _resolve().GuanduStage(f, stage);
    internal bool GuanduTurnDiscarded(int seat) => _resolve().GuanduTurnDiscarded(seat);
    internal global::CardGame.Core.ProgramSkillFrame GuanduUpdate(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GuanduGeneralReceipt r) => _resolve().GuanduUpdate(f, r);
    internal bool IsGuanduGeneralContextAllowed(string skillId, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().IsGuanduGeneralContextAllowed(skillId, operation);
    internal void ReturnGuanduGeneralDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnGuanduGeneralDraw(f, outcome);
    internal bool ReturnGuanduGeneralMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnGuanduGeneralMovement(f);
    internal bool RunGuanduGeneral(long id) => _resolve().RunGuanduGeneral(id);
}
