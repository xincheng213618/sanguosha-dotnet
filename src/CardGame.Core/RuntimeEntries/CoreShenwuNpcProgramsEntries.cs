#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreShenwuNpcProgramsRuntime))]
internal sealed partial class CoreShenwuNpcProgramsEntries
{
    private readonly Func<EngineOwners.CoreShenwuNpcProgramsRuntime> _resolve;
    private CoreShenwuNpcProgramsEntries(Func<EngineOwners.CoreShenwuNpcProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreShenwuNpcProgramsEntries Create(Func<EngineOwners.CoreShenwuNpcProgramsRuntime> resolve) => new(resolve);
}
