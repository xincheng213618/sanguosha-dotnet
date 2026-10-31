#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    // Registration only; the query still belongs to legacy GameEngine.WuLianjie.Policies.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 170)]
    internal void ProjectWuHuangFuSongCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(WuHfEntityMark(cardId));
    }

    internal string? WuHfEntityMark(int cardId) => HasWuHuangFuSongCapability && WuHfTurnOpen(WuHfTurn()) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongLianjieEntityEvent>().Any(e => e.CardId == cardId && e.Turn == WuHfTurn()) ? "连捷" : null;
}
