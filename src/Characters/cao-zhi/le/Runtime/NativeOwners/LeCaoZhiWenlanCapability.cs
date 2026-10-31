#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LeCaoZhiWenlanCapability(Func<RuntimeEntries.CharacterCaoZhiLeEntries> getCharacterCaoZhiLeRuntime) : ILeCaoZhiWenlanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoZhiLeRuntime().BeginLeCaoZhiWenlan(f);
}
