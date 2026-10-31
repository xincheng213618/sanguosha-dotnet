#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineCardSnapshotFactsRuntime : global::CardGame.Core.ICardSkillMarkSources, global::CardGame.Core.ICardSnapshotFacts
{
    private readonly ICoreCardSnapshotFactsCharacterDaQiaoLeOperations _characterDaQiaoLeOperations;
    private readonly ICoreCardSnapshotFactsCharacterDiaoChanLeOperations _characterDiaoChanLeOperations;
    private readonly ICoreCardSnapshotFactsCharacterWeiYanHappyGodOperations _characterWeiYanHappyGodOperations;
    private readonly ICoreCardSnapshotFactsProgramPipelineHooksOperations _programPipelineHooksOperations;
    private readonly ICoreCardSnapshotFactsCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICoreCardSnapshotFactsCardSkillMarkProjectorOperations _cardSkillMarkProjectorOperations;
    internal CoreGameEngineCardSnapshotFactsRuntime(ICoreCardSnapshotFactsCharacterDaQiaoLeOperations characterDaQiaoLeOperations, ICoreCardSnapshotFactsCharacterDiaoChanLeOperations characterDiaoChanLeOperations, ICoreCardSnapshotFactsCharacterWeiYanHappyGodOperations characterWeiYanHappyGodOperations, ICoreCardSnapshotFactsProgramPipelineHooksOperations programPipelineHooksOperations, ICoreCardSnapshotFactsCoreActualTurnsOperations coreActualTurnsOperations, ICoreCardSnapshotFactsCardSkillMarkProjectorOperations cardSkillMarkProjectorOperations)
    {
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._cardSkillMarkProjectorOperations = cardSkillMarkProjectorOperations;
    }
}
