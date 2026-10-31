#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineHpConditionalSkillGrantsState : Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess
{
    // A source relation remembers its first effective instance through HP exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private Dictionary<(int OwnerSeat, HpConditionalSkillGrantSource Source), string> _hpConditionalSkillInstances = [];
    internal ref readonly Dictionary<(int OwnerSeat, HpConditionalSkillGrantSource Source), string> ReferenceToHpConditionalSkillInstances => ref _hpConditionalSkillInstances;

    private bool _synchronizingHpConditionalSkillGrants = default !;
    internal ref bool ReferenceToSynchronizingHpConditionalSkillGrants => ref _synchronizingHpConditionalSkillGrants;

    ref bool Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._synchronizingHpConditionalSkillGrants => ref _synchronizingHpConditionalSkillGrants;

    ref readonly Dictionary<(int OwnerSeat, HpConditionalSkillGrantSource Source), string> Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess._hpConditionalSkillInstances => ref _hpConditionalSkillInstances;
}
