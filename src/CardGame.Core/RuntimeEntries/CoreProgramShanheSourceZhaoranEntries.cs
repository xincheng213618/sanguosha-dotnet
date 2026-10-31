#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime))]
internal sealed partial class CoreProgramShanheSourceZhaoranEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime> _resolve;
    private CoreProgramShanheSourceZhaoranEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheSourceZhaoranEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime> resolve) => new(resolve);
}
