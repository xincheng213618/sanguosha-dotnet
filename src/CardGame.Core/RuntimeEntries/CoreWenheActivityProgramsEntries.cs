#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime))]
internal sealed partial class CoreWenheActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime> _resolve;
    private CoreWenheActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreWenheActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreWenheActivityProgramsRuntime> resolve) => new(resolve);
}
