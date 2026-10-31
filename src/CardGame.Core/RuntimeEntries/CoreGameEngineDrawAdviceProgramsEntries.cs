#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime))]
internal sealed partial class CoreGameEngineDrawAdviceProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime> _resolve;
    private CoreGameEngineDrawAdviceProgramsEntries(Func<EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawAdviceProgramsEntries Create(Func<EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime> resolve) => new(resolve);
}
