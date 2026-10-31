#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhugeProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime) : IMouZhugeProgramHost
{
    public SkillProgramStepOutcome ExecuteMouZhuge(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhugeLiangMouRuntime().ExecuteMouZhuge(frame, effect);
}
