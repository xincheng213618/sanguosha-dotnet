#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaDaiMobileEntries
{
    internal bool CanOfferMobileMaDaiQianxi(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileMaDaiQianxi(gate);
    internal SkillProgramStepOutcome ReduceMobileMaDaiTargetMaximum(ProgramSkillFrame supplied) => _resolve().ReduceMobileMaDaiTargetMaximum(supplied);
}
