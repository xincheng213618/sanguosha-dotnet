#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappySunQuanCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunQuanHappyGodRuntime>> getCharacterSunQuanHappyGodRuntime) : IHappySunQuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunQuanHappyGodRuntime().Value.BeginHappySunQuan(f, op);
}
