#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class IncomingDistanceCourtesyCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime>> getCoreGameEngineIncomingDistanceCourtesyRuntime) : IIncomingDistanceCourtesyHost
{
    public SkillProgramStepOutcome BeginIncomingDistanceCourtesy(ProgramSkillFrame f, IncomingDistanceCourtesyKind kind) => getCoreGameEngineIncomingDistanceCourtesyRuntime().Value.BeginIncomingDistanceCourtesy(f, kind);
}
