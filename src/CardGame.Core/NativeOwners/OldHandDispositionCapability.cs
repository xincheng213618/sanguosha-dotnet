#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class OldHandDispositionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOldHandDispositionEntries> getCoreGameEngineOldHandDispositionRuntime) : IOldHandDispositionHost
{
    public SkillProgramStepOutcome ReplaceDiscardWithOldHandDisposition(ProgramSkillFrame f) => getCoreGameEngineOldHandDispositionRuntime().ReplaceDiscardWithOldHandDisposition(f);
}
