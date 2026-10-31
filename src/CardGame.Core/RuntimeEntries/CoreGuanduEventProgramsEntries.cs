#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGuanduEventProgramsRuntime))]
internal sealed partial class CoreGuanduEventProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGuanduEventProgramsRuntime> _resolve;
    private CoreGuanduEventProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGuanduEventProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGuanduEventProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGuanduEventProgramsRuntime> resolve) => new(resolve);
}
