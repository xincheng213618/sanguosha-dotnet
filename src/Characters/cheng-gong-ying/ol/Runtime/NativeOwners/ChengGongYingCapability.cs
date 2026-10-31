#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ChengGongYingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime) : IChengGongYingHost
{
    public SkillProgramStepOutcome Adjust(ProgramSkillFrame frame) => getCharacterChengGongYingOlRuntime().BeginChengGongYing(frame);
}
