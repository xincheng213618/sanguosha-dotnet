#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DouyuCapability(Func<RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime) : IDouyuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterPangJiOlRuntime().ResolveDouyu(frame);
}
