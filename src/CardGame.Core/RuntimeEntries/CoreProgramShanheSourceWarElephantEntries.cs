#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime))]
internal sealed partial class CoreProgramShanheSourceWarElephantEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime> _resolve;
    private CoreProgramShanheSourceWarElephantEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheSourceWarElephantEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceWarElephantRuntime> resolve) => new(resolve);
}
