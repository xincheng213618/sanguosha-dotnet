#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineCardSnapshotFactsRuntime : ICardSkillMarkSources, ICardSnapshotFacts
{
    private readonly RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly ICoreCardSnapshotFactsCardSkillMarkProjectorOperations _cardSkillMarkProjectorOperations;
    internal CoreGameEngineCardSnapshotFactsRuntime(RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations, RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, ICoreCardSnapshotFactsCardSkillMarkProjectorOperations cardSkillMarkProjectorOperations)
    {
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._cardSkillMarkProjectorOperations = cardSkillMarkProjectorOperations;
    }
}
