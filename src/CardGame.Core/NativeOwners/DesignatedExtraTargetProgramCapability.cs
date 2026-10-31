#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DesignatedExtraTargetProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries> getCoreGameEngineDesignatedExtraTargetsRuntime) : IDesignatedExtraTargetProgramHost
{
    public SkillProgramStepOutcome AddOneDistanceFreeCurrentUseTarget(ProgramSkillFrame frame) => getCoreGameEngineDesignatedExtraTargetsRuntime().AddOneDistanceFreeCurrentUseTarget(frame);
}
