#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineHpConditionalSkillGrantsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess
{
    // A source relation remembers its first effective instance through HP exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.HpConditionalSkillGrantSource Source), string> _hpConditionalSkillInstances = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.HpConditionalSkillGrantSource Source), string> ReferenceToHpConditionalSkillInstances => ref _hpConditionalSkillInstances;

    private bool _synchronizingHpConditionalSkillGrants = default !;
    internal ref bool ReferenceToSynchronizingHpConditionalSkillGrants => ref _synchronizingHpConditionalSkillGrants;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._synchronizingHpConditionalSkillGrants => ref _synchronizingHpConditionalSkillGrants;

    ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.HpConditionalSkillGrantSource Source), string> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances => ref _hpConditionalSkillInstances;
}
