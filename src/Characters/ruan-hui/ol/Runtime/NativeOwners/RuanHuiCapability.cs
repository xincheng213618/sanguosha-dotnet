#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuanHuiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterRuanHuiOlRuntime>> getCharacterRuanHuiOlRuntime) : IRuanHuiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterRuanHuiOlRuntime().Value.BeginRuanHui(f, op);
}
