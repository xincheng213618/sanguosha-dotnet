#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime))]
internal sealed partial class CoreHulaoActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime> _resolve;
    private CoreHulaoActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHulaoActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHulaoActivityProgramsRuntime> resolve) => new(resolve);
}
