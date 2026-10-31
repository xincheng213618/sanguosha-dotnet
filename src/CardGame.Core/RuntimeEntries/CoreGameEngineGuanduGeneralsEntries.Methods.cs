#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralsEntries
{
    internal SkillProgramStepOutcome BeginGuanduGeneral(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginGuanduGeneral(supplied, operation);
    internal void CaptureGuanduDiscardBatch(CardMovementBatchContext batch) => _resolve().CaptureGuanduDiscardBatch(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureGuanduDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureGuanduDiscardBatch(batch, completed);
    internal bool ContinueIssuedGuanduGeneral(ProgramSkillFrame f) => _resolve().ContinueIssuedGuanduGeneral(f);
    internal bool DrainGuanduGeneralChildren(long id) => _resolve().DrainGuanduGeneralChildren(id);
    internal void DrawGuanduGeneral(ProgramSkillFrame f, int recipient, int count, string next) => _resolve().DrawGuanduGeneral(f, recipient, count, next);
    internal void FinishGuanduGeneral(ProgramSkillFrame f, bool completed = true) => _resolve().FinishGuanduGeneral(f, completed);
    internal Card[] GuanduDiscardable(int chooser, int owner) => _resolve().GuanduDiscardable(chooser, owner);
    internal bool GuanduGeneralGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().GuanduGeneralGrantQualified(owner, grant);
    internal IEnumerable<RuleQueryContribution> GuanduGeneralRuleContributions(CharacterState player, SkillRuleQuery query) => _resolve().GuanduGeneralRuleContributions(player, query);
    internal Card[] GuanduOwnedCards(int seat, bool judgment = false) => _resolve().GuanduOwnedCards(seat, judgment);
    internal IEnumerable<CardSnapshot> GuanduShicaiPrivatelyViewedCards(int viewerSeat) => _resolve().GuanduShicaiPrivatelyViewedCards(viewerSeat);
    internal bool GuanduShicaiReady(int ownerSeat, string instance) => _resolve().GuanduShicaiReady(ownerSeat, instance);
    internal bool GuanduSkillEnabled(CharacterState owner, string skill) => _resolve().GuanduSkillEnabled(owner, skill);
    internal ProgramSkillFrame GuanduStage(ProgramSkillFrame f, string stage) => _resolve().GuanduStage(f, stage);
    internal bool GuanduTurnDiscarded(int seat) => _resolve().GuanduTurnDiscarded(seat);
    internal ProgramSkillFrame GuanduUpdate(ProgramSkillFrame f, GuanduGeneralReceipt r) => _resolve().GuanduUpdate(f, r);
    internal bool IsGuanduGeneralContextAllowed(string skillId, SkillProgramEffectOp operation) => _resolve().IsGuanduGeneralContextAllowed(skillId, operation);
    internal void ReturnGuanduGeneralDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnGuanduGeneralDraw(f, outcome);
    internal bool ReturnGuanduGeneralMovement(ProgramSkillFrame f) => _resolve().ReturnGuanduGeneralMovement(f);
    internal bool RunGuanduGeneral(long id) => _resolve().RunGuanduGeneral(id);
}
