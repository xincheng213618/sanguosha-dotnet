#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime))]
internal sealed partial class CoreGameEngineAllHandChallengeReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> _resolve;
    private CoreGameEngineAllHandChallengeReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAllHandChallengeReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAllHandChallengeReceiptsRuntime> resolve) => new(resolve);
}
