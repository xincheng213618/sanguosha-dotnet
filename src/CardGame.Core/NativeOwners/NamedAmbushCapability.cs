#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class NamedAmbushCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushEntries> getCoreGameEngineNamedAmbushRuntime) : INamedAmbushHost
{
    public SkillProgramStepOutcome ExecuteNamedAmbush(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineNamedAmbushRuntime().BeginNamedAmbush(e, f);
}
