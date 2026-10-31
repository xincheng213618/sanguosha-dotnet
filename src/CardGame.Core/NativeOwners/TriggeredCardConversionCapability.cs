#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TriggeredCardConversionCapability(Func<RuntimeEntries.CoreGameEngineTriggeredCardConversionsEntries> getCoreGameEngineTriggeredCardConversionsRuntime) : ITriggeredCardConversionHost
{
    public SkillProgramStepOutcome UseBoundCardsAsSlash(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineTriggeredCardConversionsRuntime().UseTriggeredBoundCardsAsSlash(frame, effect);
}
