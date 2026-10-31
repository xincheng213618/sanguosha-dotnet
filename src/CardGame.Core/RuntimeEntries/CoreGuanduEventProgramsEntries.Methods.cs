#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGuanduEventProgramsEntries
{
    internal bool AllowsGuanduEvent(ProgramTriggerLegalityContext gate) => _resolve().AllowsGuanduEvent(gate);
    internal bool CanUseGuanduRoundConversion(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) => _resolve().CanUseGuanduRoundConversion(instance, rule, owner);
    internal void ConsumeGuanduRoundConversion(CardConversionSource source) => _resolve().ConsumeGuanduRoundConversion(source);
    internal SkillProgramStepOutcome ExecuteGuanduEvent(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().ExecuteGuanduEvent(input, op);
    internal CardUseFrame? GuanduDamageRewardUse(int owner, ProgramSkillWindowContext context) => _resolve().GuanduDamageRewardUse(owner, context);
    internal bool GuanduRoundConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().GuanduRoundConversionUsageUnavailable(query);
    internal CardUseFrame? GuanduTenthUse(int owner, ProgramSkillWindowContext context) => _resolve().GuanduTenthUse(owner, context);
    internal void RecordGuanduCardAction(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().RecordGuanduCardAction(action, window);
    internal bool ResumeGuanduEventFlow(long id) => _resolve().ResumeGuanduEventFlow(id);
    internal bool RetainGuanduEventFlow(ProgramSkillFrame f) => _resolve().RetainGuanduEventFlow(f);
    internal void ReturnGuanduEventDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnGuanduEventDraw(f, outcome);
    internal GuanduBattleEvent? CurrentGuanduEvent { get => _resolve().CurrentGuanduEvent; }
}
