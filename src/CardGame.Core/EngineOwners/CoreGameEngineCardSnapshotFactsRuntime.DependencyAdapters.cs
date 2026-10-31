#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreCardSnapshotFactsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardSnapshotFactsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsDiouCarrier(int id) => this._targetFactory().IsDiouCarrier(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardSkillMarkProjector))]
internal sealed class CoreCardSnapshotFactsCardSkillMarkProjectorAdapter(global::System.Func<global::CardGame.Core.CardSkillMarkProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardSnapshotFactsCardSkillMarkProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardSkillMarkProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSkillMarkProjector Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreCardSnapshotFactsCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardSnapshotFactsCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind? ZixiQinJudgmentSlot(int id) => this._targetFactory().ZixiQinJudgmentSlot(id);
    public bool IsZixiQinJudgmentCard(int id) => this._targetFactory().IsZixiQinJudgmentCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreCardSnapshotFactsCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardSnapshotFactsCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot? HappyBoneSlot(global::CardGame.Core.Card card) => this._targetFactory().HappyBoneSlot(card);
    public global::CardGame.Core.HappyWeiYanBone? HappyBoneRecord(int id) => this._targetFactory().HappyBoneRecord(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreCardSnapshotFactsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardSnapshotFactsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card) => this._targetFactory().GetJudgmentEffectiveCardKind(card);
}
