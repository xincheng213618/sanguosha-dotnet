#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OldHandDispositionCapability(Func<RuntimeEntries.CoreGameEngineOldHandDispositionEntries> getCoreGameEngineOldHandDispositionRuntime) : IOldHandDispositionHost
{
    public SkillProgramStepOutcome ReplaceDiscardWithOldHandDisposition(ProgramSkillFrame f) => getCoreGameEngineOldHandDispositionRuntime().ReplaceDiscardWithOldHandDisposition(f);
}
