#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaohuoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries> getCharacterTaoQianOlRuntime) : IZhaohuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterTaoQianOlRuntime().ResolveZhaohuoFirstVictimGift(frame);
}
