#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatGeneralsEntries
{
    internal bool AllowsDragonBoatGeneralTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsDragonBoatGeneralTrigger(gate);
    internal bool AllowsDragonBoatJianji(ProgramActivationLegalityContext gate) => _resolve().AllowsDragonBoatJianji(gate);
    internal void AnswerDragonBoatGeneral(PromptChoice choice) => _resolve().AnswerDragonBoatGeneral(choice);
    internal SkillProgramStepOutcome BeginDragonBoatGeneral(ProgramSkillFrame supplied, SkillProgramEffectOp operation, int targetSeat) => _resolve().BeginDragonBoatGeneral(supplied, operation, targetSeat);
    internal void CaptureDragonBoatFinalizedUse(long useId, CardActionContext action) => _resolve().CaptureDragonBoatFinalizedUse(useId, action);
    internal PromptChoice ChooseDragonBoatGeneral(ProgramSkillFrame frame) => _resolve().ChooseDragonBoatGeneral(frame);
    internal bool ContinuePaidDragonBoatGeneral(ProgramSkillFrame frame) => _resolve().ContinuePaidDragonBoatGeneral(frame);
    internal bool DragonBoatAllies(int owner, int other) => _resolve().DragonBoatAllies(owner, other);
    internal bool DragonBoatDianhuQualified(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DragonBoatDianhuQualified(candidate, context);
    internal DragonBoatDianhuArmedEvent? DragonBoatDianhuTarget(int ownerSeat, string instance) => _resolve().DragonBoatDianhuTarget(ownerSeat, instance);
    internal IReadOnlyList<PromptChoice> DragonBoatGeneralChoices(ProgramSkillFrame frame) => _resolve().DragonBoatGeneralChoices(frame);
    internal PromptChoice? DragonBoatJianjiAiChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().DragonBoatJianjiAiChoice(decision, frame);
    internal IReadOnlyList<int> DragonBoatLianpianTargets(int ownerSeat, ProgramSkillWindowContext context) => _resolve().DragonBoatLianpianTargets(ownerSeat, context);
    internal bool DragonBoatTunjiangQualified(int ownerSeat, ProgramSkillWindowContext context) => _resolve().DragonBoatTunjiangQualified(ownerSeat, context);
    internal void IssueDragonBoatGeneralDraw(ProgramSkillFrame frame, int recipient, int count) => _resolve().IssueDragonBoatGeneralDraw(frame, recipient, count);
    internal void PublishDragonBoatGeneralChoice(ProgramSkillFrame frame) => _resolve().PublishDragonBoatGeneralChoice(frame);
    internal bool ResumeDragonBoatGeneral(long id) => _resolve().ResumeDragonBoatGeneral(id);
    internal void ReturnDragonBoatGeneralDraw(ProgramSkillFrame input, NativeDrawOutcome outcome) => _resolve().ReturnDragonBoatGeneralDraw(input, outcome);
    internal bool ReturnDragonBoatGeneralMovement(ProgramSkillFrame frame) => _resolve().ReturnDragonBoatGeneralMovement(frame);
    internal void ReturnDragonBoatJianjiDraw(ProgramSkillFrame input, NativeDrawOutcome outcome) => _resolve().ReturnDragonBoatJianjiDraw(input, outcome);
    internal PromptChoice SelectActivityAwareJianjiChoice(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SelectActivityAwareJianjiChoice(decision, frame, effect);
    internal void SynchronizeDragonBoatXingzhao() => _resolve().SynchronizeDragonBoatXingzhao();
    internal IEnumerable<CharacterState> WenjiEligibleGiftRecipients(int ownerSeat, string skillId) => _resolve().WenjiEligibleGiftRecipients(ownerSeat, skillId);
}
