#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess : IProgramRuntimeOperations
{
    ref bool _synchronizingHpConditionalSkillGrants { get; }

    ref readonly Dictionary<(int OwnerSeat, HpConditionalSkillGrantSource Source), string> _hpConditionalSkillInstances { get; }
}
