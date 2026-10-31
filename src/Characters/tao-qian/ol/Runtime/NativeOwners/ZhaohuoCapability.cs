#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ZhaohuoCapability(Func<RuntimeEntries.CharacterTaoQianOlEntries> getCharacterTaoQianOlRuntime) : IZhaohuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterTaoQianOlRuntime().ResolveZhaohuoFirstVictimGift(frame);
}
