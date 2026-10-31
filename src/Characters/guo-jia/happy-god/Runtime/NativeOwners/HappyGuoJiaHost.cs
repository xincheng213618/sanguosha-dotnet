#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyGuoJiaHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuoJiaHappyGodRuntime>> getCharacterGuoJiaHappyGodRuntime) : IHappyGuoJiaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoJiaHappyGodRuntime().Value.BeginHappyGuoJia(f, op);
}
