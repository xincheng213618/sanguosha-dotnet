#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreShanheBossProgramsRuntime))]
internal sealed partial class CoreShanheBossProgramsEntries
{
    private readonly Func<EngineOwners.CoreShanheBossProgramsRuntime> _resolve;
    private CoreShanheBossProgramsEntries(Func<EngineOwners.CoreShanheBossProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreShanheBossProgramsEntries Create(Func<EngineOwners.CoreShanheBossProgramsRuntime> resolve) => new(resolve);
}
