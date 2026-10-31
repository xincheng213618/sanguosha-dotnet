#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class KongShuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterKongShuOlEntries> getCharacterKongShuOlRuntime) : IKongShuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterKongShuOlRuntime().BeginKongShu(f, op);
}
