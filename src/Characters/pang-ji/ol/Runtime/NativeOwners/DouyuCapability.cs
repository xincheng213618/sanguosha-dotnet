#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DouyuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangJiOlEntries> getCharacterPangJiOlRuntime) : IDouyuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterPangJiOlRuntime().ResolveDouyu(frame);
}
