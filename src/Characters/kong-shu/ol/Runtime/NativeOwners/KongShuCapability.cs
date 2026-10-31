#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class KongShuCapability(Func<RuntimeEntries.CharacterKongShuOlEntries> getCharacterKongShuOlRuntime) : IKongShuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterKongShuOlRuntime().BeginKongShu(f, op);
}
