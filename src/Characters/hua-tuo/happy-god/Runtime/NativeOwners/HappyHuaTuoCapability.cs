#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyHuaTuoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuaTuoHappyGodRuntime>> getCharacterHuaTuoHappyGodRuntime) : IHappyHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaTuoHappyGodRuntime().Value.ResolveHappyHuaTuo(f, op);
}
