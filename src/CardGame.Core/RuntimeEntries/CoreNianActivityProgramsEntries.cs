#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNianActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> _resolve;
    private CoreNianActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNianActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> resolve) => new(resolve);
}
