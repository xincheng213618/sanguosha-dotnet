#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ChengGongYingCapability(Func<RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime) : IChengGongYingHost
{
    public SkillProgramStepOutcome Adjust(ProgramSkillFrame frame) => getCharacterChengGongYingOlRuntime().BeginChengGongYing(frame);
}
