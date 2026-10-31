#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGuanduEventProgramsEntries
{
    internal bool AllowsGuanduEvent(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsGuanduEvent(gate);
    internal void AssertGuanduEventFlow(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertGuanduEventFlow(f, plan);
    internal bool CanUseGuanduRoundConversion(global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, global::CardGame.Core.CharacterState owner) => _resolve().CanUseGuanduRoundConversion(instance, rule, owner);
    internal void ConsumeGuanduRoundConversion(global::CardGame.Core.CardConversionSource source) => _resolve().ConsumeGuanduRoundConversion(source);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteGuanduEvent(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteGuanduEvent(input, op);
    internal global::CardGame.Core.CardUseFrame? GuanduDamageRewardUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().GuanduDamageRewardUse(owner, context);
    internal bool GuanduRoundConversionUsageUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().GuanduRoundConversionUsageUnavailable(query);
    internal global::CardGame.Core.CardUseFrame? GuanduTenthUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().GuanduTenthUse(owner, context);
    internal bool IsGuanduArroganceDying() => _resolve().IsGuanduArroganceDying();
    internal void RecordGuanduCardAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().RecordGuanduCardAction(action, window);
    internal bool ResumeGuanduEventFlow(long id) => _resolve().ResumeGuanduEventFlow(id);
    internal bool RetainGuanduEventFlow(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainGuanduEventFlow(f);
    internal void ReturnGuanduEventDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnGuanduEventDraw(f, outcome);
    internal global::CardGame.Core.GuanduBattleEvent? CurrentGuanduEvent { get => _resolve().CurrentGuanduEvent; }
}
