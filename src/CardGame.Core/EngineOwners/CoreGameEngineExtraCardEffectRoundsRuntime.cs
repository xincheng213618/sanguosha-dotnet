#nullable enable
namespace CardGame.Core.EngineOwners;
[ProgramRuntimeModule]
internal sealed partial class CoreGameEngineExtraCardEffectRoundsRuntime(INativeCardEffectEpisodes _episodes,
    INativeCardEffectEpisodeExecution _execution,
    RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _uses,
    RuntimeEntries.CoreGameEngineCardAttackStateEntries _attacks,
    RuntimeEntries.CoreNativeSlashUseEntries _slash,
    RuntimeEntries.CoreNativeAttackCompletionEntries _completion)
{
}
