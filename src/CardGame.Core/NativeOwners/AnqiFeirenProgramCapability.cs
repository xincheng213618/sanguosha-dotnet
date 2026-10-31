#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnqiFeirenProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineAnqiFeirenRuntime>> getCoreGameEngineAnqiFeirenRuntime) : IAnqiFeirenHost
{
    public SkillProgramStepOutcome ResolveAnqiFeiren(ProgramSkillFrame frame) => getCoreGameEngineAnqiFeirenRuntime().Value.ResolveAnqiFeiren(frame);
}
