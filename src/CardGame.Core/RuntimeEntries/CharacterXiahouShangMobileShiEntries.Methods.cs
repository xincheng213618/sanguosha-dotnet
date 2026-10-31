#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouShangMobileShiEntries
{
    internal string MobileShiTanfengMode(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MobileShiTanfengMode(frame);
    internal void ResolveMobileShiTanfengTarget(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveMobileShiTanfengTarget(selected);
    internal global::CardGame.Core.PromptChoice SelectMobileShiTanfengAiTarget(global::CardGame.Core.PendingDecision decision) => _resolve().SelectMobileShiTanfengAiTarget(decision);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectMobileShiTanfengTarget(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectMobileShiTanfengTarget(supplied, effect);
}
