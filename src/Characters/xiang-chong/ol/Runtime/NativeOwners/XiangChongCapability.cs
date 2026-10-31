#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class XiangChongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiangChongOlEntries> getCharacterXiangChongOlRuntime) : IXiangChongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect effect) => getCharacterXiangChongOlRuntime().BeginXiangChong(f, effect);
}
