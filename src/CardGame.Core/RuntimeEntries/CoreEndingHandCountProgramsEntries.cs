#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreEndingHandCountProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreEndingHandCountProgramsRuntime> _resolve;
    private CoreEndingHandCountProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreEndingHandCountProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreEndingHandCountProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreEndingHandCountProgramsRuntime> resolve) => new(resolve);
}
