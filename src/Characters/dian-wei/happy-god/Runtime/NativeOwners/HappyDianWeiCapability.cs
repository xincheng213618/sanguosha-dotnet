#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyDianWeiCapability(Func<RuntimeEntries.CharacterDianWeiHappyGodEntries> getCharacterDianWeiHappyGodRuntime) : IHappyDianWeiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDianWeiHappyGodRuntime().ResolveHappyDianWei(f, op);
}
