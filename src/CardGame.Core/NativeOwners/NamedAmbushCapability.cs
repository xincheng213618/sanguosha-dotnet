#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class NamedAmbushCapability(Func<RuntimeEntries.CoreGameEngineNamedAmbushEntries> getCoreGameEngineNamedAmbushRuntime) : INamedAmbushHost
{
    public SkillProgramStepOutcome ExecuteNamedAmbush(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineNamedAmbushRuntime().BeginNamedAmbush(e, f);
}
