#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhouYuZuifengCapability(Func<RuntimeEntries.CharacterZhouYuOlMouEntries> getCharacterZhouYuOlMouRuntime) : IMouZhouYuZuifengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhouYuOlMouRuntime().BeginMouZuifeng(f);
}
