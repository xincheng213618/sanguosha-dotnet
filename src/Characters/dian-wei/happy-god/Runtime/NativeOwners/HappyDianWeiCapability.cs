#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyDianWeiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDianWeiHappyGodRuntime>> getCharacterDianWeiHappyGodRuntime) : IHappyDianWeiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDianWeiHappyGodRuntime().Value.ResolveHappyDianWei(f, op);
}
