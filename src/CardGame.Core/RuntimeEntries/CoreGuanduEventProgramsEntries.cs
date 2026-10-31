#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGuanduEventProgramsRuntime))]
internal sealed partial class CoreGuanduEventProgramsEntries
{
    private readonly Func<EngineOwners.CoreGuanduEventProgramsRuntime> _resolve;
    private CoreGuanduEventProgramsEntries(Func<EngineOwners.CoreGuanduEventProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGuanduEventProgramsEntries Create(Func<EngineOwners.CoreGuanduEventProgramsRuntime> resolve) => new(resolve);
}
