#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyDiaoChanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDiaoChanHappyGodRuntime>> getCharacterDiaoChanHappyGodRuntime) : IHappyDiaoChanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDiaoChanHappyGodRuntime().Value.BeginHappyDc(f, op);
}
