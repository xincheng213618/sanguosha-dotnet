#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatGeneralsEntries
{
    internal bool AllowsDragonBoatGeneralTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsDragonBoatGeneralTrigger(gate);
    internal bool AllowsDragonBoatJianji(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsDragonBoatJianji(gate);
    internal void AnswerDragonBoatGeneral(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerDragonBoatGeneral(choice);
    internal void AssertDragonBoatGeneral(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertDragonBoatGeneral(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDragonBoatGeneral(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation, int targetSeat) => _resolve().BeginDragonBoatGeneral(supplied, operation, targetSeat);
    internal void CaptureDragonBoatFinalizedUse(long useId, global::CardGame.Core.CardActionContext action) => _resolve().CaptureDragonBoatFinalizedUse(useId, action);
    internal global::CardGame.Core.PromptChoice ChooseDragonBoatGeneral(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseDragonBoatGeneral(frame);
    internal bool ContinuePaidDragonBoatGeneral(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidDragonBoatGeneral(frame);
    internal bool DragonBoatAllies(int owner, int other) => _resolve().DragonBoatAllies(owner, other);
    internal bool DragonBoatDianhuQualified(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DragonBoatDianhuQualified(candidate, context);
    internal global::CardGame.Core.DragonBoatDianhuArmedEvent? DragonBoatDianhuTarget(int ownerSeat, string instance) => _resolve().DragonBoatDianhuTarget(ownerSeat, instance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DragonBoatGeneralChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DragonBoatGeneralChoices(frame);
    internal global::CardGame.Core.PromptChoice? DragonBoatJianjiAiChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DragonBoatJianjiAiChoice(decision, frame);
    internal global::System.Collections.Generic.IReadOnlyList<int> DragonBoatLianpianTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DragonBoatLianpianTargets(ownerSeat, context);
    internal bool DragonBoatTunjiangQualified(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DragonBoatTunjiangQualified(ownerSeat, context);
    internal void IssueDragonBoatGeneralDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipient, int count) => _resolve().IssueDragonBoatGeneralDraw(frame, recipient, count);
    internal void PublishDragonBoatGeneralChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishDragonBoatGeneralChoice(frame);
    internal bool ResumeDragonBoatGeneral(long id) => _resolve().ResumeDragonBoatGeneral(id);
    internal void ReturnDragonBoatGeneralDraw(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnDragonBoatGeneralDraw(input, outcome);
    internal bool ReturnDragonBoatGeneralMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnDragonBoatGeneralMovement(frame);
    internal void ReturnDragonBoatJianjiDraw(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnDragonBoatJianjiDraw(input, outcome);
    internal global::CardGame.Core.PromptChoice SelectActivityAwareJianjiChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectActivityAwareJianjiChoice(decision, frame, effect);
    internal void SynchronizeDragonBoatXingzhao() => _resolve().SynchronizeDragonBoatXingzhao();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CharacterState> WenjiEligibleGiftRecipients(int ownerSeat, string skillId) => _resolve().WenjiEligibleGiftRecipients(ownerSeat, skillId);
}
