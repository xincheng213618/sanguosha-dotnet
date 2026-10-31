#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime))]
internal sealed partial class CoreShenwuNpcProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> _resolve;
    private CoreShenwuNpcProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreShenwuNpcProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> resolve) => new(resolve);
}
