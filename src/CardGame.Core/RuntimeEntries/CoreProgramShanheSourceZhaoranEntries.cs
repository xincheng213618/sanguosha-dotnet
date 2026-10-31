#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramShanheSourceZhaoranRuntime))]
internal sealed partial class CoreProgramShanheSourceZhaoranEntries
{
    private readonly Func<EngineOwners.CoreProgramShanheSourceZhaoranRuntime> _resolve;
    private CoreProgramShanheSourceZhaoranEntries(Func<EngineOwners.CoreProgramShanheSourceZhaoranRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheSourceZhaoranEntries Create(Func<EngineOwners.CoreProgramShanheSourceZhaoranRuntime> resolve) => new(resolve);
}
