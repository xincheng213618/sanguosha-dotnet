#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhenJiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhenJiHappyGodRuntime>> getCharacterZhenJiHappyGodRuntime) : IHappyZhenJiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhenJiHappyGodRuntime().Value.BeginHappyZj(f, op);
}
