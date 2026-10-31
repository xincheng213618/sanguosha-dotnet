#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class KeBiNengCapability(Func<RuntimeEntries.CharacterKeBiNengOlEntries> getCharacterKeBiNengOlRuntime) : IKeBiNengProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterKeBiNengOlRuntime().BeginKeBiNeng(frame);
}
