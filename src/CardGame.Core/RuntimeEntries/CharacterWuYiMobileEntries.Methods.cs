#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuYiMobileEntries
{
    internal SkillProgramStepOutcome ApplyMobileWuYiBenxi(ProgramSkillFrame supplied) => _resolve().ApplyMobileWuYiBenxi(supplied);
    internal PromptChoice MobileWuYiBenxiAi(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().MobileWuYiBenxiAi(decision, frame, effect);
}
