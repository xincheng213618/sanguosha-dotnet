#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhouQunCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhouQunOlRuntime>> getCharacterZhouQunOlRuntime) : IZhouQunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterZhouQunOlRuntime().Value.BeginZhouQun(frame, op);
}
