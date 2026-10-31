#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardSnapshotFactsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreCardSnapshotFactsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouCarrier(int id);
}

internal interface ICoreCardSnapshotFactsCardSkillMarkProjectorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSkillMarkProjector Get();
}

internal interface ICoreCardSnapshotFactsCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind? ZixiQinJudgmentSlot(int id);
    bool IsZixiQinJudgmentCard(int id);
}

internal interface ICoreCardSnapshotFactsCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot? HappyBoneSlot(global::CardGame.Core.Card card);
    global::CardGame.Core.HappyWeiYanBone? HappyBoneRecord(int id);
}

internal interface ICoreCardSnapshotFactsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
}
