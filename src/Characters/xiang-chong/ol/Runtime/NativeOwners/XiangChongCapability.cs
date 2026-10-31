#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class XiangChongCapability(Func<RuntimeEntries.CharacterXiangChongOlEntries> getCharacterXiangChongOlRuntime) : IXiangChongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect effect) => getCharacterXiangChongOlRuntime().BeginXiangChong(f, effect);
}
