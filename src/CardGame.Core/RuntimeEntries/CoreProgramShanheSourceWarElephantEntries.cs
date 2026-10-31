#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramShanheSourceWarElephantRuntime))]
internal sealed partial class CoreProgramShanheSourceWarElephantEntries
{
    private readonly Func<EngineOwners.CoreProgramShanheSourceWarElephantRuntime> _resolve;
    private CoreProgramShanheSourceWarElephantEntries(Func<EngineOwners.CoreProgramShanheSourceWarElephantRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheSourceWarElephantEntries Create(Func<EngineOwners.CoreProgramShanheSourceWarElephantRuntime> resolve) => new(resolve);
}
