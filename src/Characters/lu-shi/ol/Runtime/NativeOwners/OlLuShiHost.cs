#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlLuShiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuShiOlRuntime>> getCharacterLuShiOlRuntime) : IOlLuShiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuShiOlRuntime().Value.BeginOlLuShi(f, op);
}
