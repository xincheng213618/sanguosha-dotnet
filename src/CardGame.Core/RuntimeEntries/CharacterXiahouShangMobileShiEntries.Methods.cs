#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouShangMobileShiEntries
{
    internal string MobileShiTanfengMode(ProgramSkillFrame frame) => _resolve().MobileShiTanfengMode(frame);
    internal void ResolveMobileShiTanfengTarget(PromptChoice selected) => _resolve().ResolveMobileShiTanfengTarget(selected);
    internal PromptChoice SelectMobileShiTanfengAiTarget(PendingDecision decision) => _resolve().SelectMobileShiTanfengAiTarget(decision);
    internal SkillProgramStepOutcome SelectMobileShiTanfengTarget(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().SelectMobileShiTanfengTarget(supplied, effect);
}
