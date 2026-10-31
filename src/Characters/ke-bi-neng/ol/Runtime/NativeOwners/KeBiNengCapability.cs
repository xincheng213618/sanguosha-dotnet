#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class KeBiNengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterKeBiNengOlEntries> getCharacterKeBiNengOlRuntime) : IKeBiNengProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterKeBiNengOlRuntime().BeginKeBiNeng(frame);
}
