#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _synchronizingHpConditionalSkillGrants { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int OwnerSeat, global::CardGame.Core.HpConditionalSkillGrantSource Source), string> _hpConditionalSkillInstances { get; }
}
