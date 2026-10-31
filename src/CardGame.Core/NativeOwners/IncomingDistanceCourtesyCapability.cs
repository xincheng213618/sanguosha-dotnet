#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class IncomingDistanceCourtesyCapability(Func<RuntimeEntries.CoreGameEngineIncomingDistanceCourtesyEntries> getCoreGameEngineIncomingDistanceCourtesyRuntime) : IIncomingDistanceCourtesyHost
{
    public SkillProgramStepOutcome BeginIncomingDistanceCourtesy(ProgramSkillFrame f, IncomingDistanceCourtesyKind kind) => getCoreGameEngineIncomingDistanceCourtesyRuntime().BeginIncomingDistanceCourtesy(f, kind);
}
