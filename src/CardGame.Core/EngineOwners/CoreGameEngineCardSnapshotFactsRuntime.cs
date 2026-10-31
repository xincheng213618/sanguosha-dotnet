#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineCardSnapshotFactsRuntime : global::CardGame.Core.ICardSkillMarkSources, global::CardGame.Core.ICardSnapshotFacts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly ICoreCardSnapshotFactsCardSkillMarkProjectorOperations _cardSkillMarkProjectorOperations;
    internal CoreGameEngineCardSnapshotFactsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, ICoreCardSnapshotFactsCardSkillMarkProjectorOperations cardSkillMarkProjectorOperations)
    {
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._cardSkillMarkProjectorOperations = cardSkillMarkProjectorOperations;
    }
}
