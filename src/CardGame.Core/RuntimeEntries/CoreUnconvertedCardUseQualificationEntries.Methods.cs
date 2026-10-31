#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreUnconvertedCardUseQualificationEntries
{
    internal bool IsUnconvertedActualCardUse(CardActionContext action, SkillProgramTriggerWindow window, ProgramCardContinuation? continuation) => _resolve().IsUnconvertedActualCardUse(action, window, continuation);
}
