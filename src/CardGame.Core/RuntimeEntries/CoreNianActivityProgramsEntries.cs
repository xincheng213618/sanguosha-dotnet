#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime))]
internal sealed partial class CoreNianActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> _resolve;
    private CoreNianActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNianActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> resolve) => new(resolve);
}
