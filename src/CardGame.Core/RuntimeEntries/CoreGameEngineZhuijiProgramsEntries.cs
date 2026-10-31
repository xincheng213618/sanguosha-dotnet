#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime))]
internal sealed partial class CoreGameEngineZhuijiProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> _resolve;
    private CoreGameEngineZhuijiProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhuijiProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> resolve) => new(resolve);
}
