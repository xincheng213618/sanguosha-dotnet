#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WeijieCapability(Func<RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime) : IWeijieHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterGuoTuOlRuntime().ResolveWeijie(f);
}
