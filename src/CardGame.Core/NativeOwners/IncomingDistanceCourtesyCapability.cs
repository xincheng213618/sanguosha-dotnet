#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class IncomingDistanceCourtesyCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineIncomingDistanceCourtesyEntries> getCoreGameEngineIncomingDistanceCourtesyRuntime) : IIncomingDistanceCourtesyHost
{
    public SkillProgramStepOutcome BeginIncomingDistanceCourtesy(ProgramSkillFrame f, IncomingDistanceCourtesyKind kind) => getCoreGameEngineIncomingDistanceCourtesyRuntime().BeginIncomingDistanceCourtesy(f, kind);
}
