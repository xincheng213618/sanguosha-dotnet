#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageHooksRuntime
{
    internal static Type DamageHookDelegateType(ProgramDamagePhase phase) => phase switch
    {
        ProgramDamagePhase.FactModifiers => typeof(Func<GameEngine.IDamageAttempt, IEnumerable<(CardUseEffectSource Source, int Amount)>>),
        ProgramDamagePhase.AdditiveAmount => typeof(Func<GameEngine.IDamageAttempt, int>),
        ProgramDamagePhase.BeforeProgramWindow or ProgramDamagePhase.AfterEquipmentChoices => typeof(Func<GameEngine.IDamageAttempt, int, DamageNature, ProgramDamageInterception>),
        ProgramDamagePhase.AppliedBeforeDying => typeof(Func<GameEngine.IDamageAttempt, long, bool>),
        ProgramDamagePhase.AttackCompletionPrelude or ProgramDamagePhase.IssuedAttackCompletion => typeof(ProgramDamageCompletion<NativeOwners.CardAttackHandle>),
        ProgramDamagePhase.BeforeChainAdvance or ProgramDamagePhase.AfterChainAdvance => typeof(Action<GameEngine.IDamageAttempt>),
        _ => throw new ArgumentOutOfRangeException("phase")};
    internal static ProgramDamageHookCatalog NativeDamageHookCatalog => CoreProgramDamageHooksDefinitions.DamageHookMetadata.Value;
    internal ProgramDamageHooks DamageHooks => _damageHooksOperations._damageHooks ??= new(CoreProgramDamageHooksDefinitions.DamageHookMetadata.Value, row => _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(row.Method, row.DelegateType), _modeRuntimeOperations._modeRuntime.DamageRules, attack => CaptureModeDamageFacts((GameEngine.IDamageAttempt)attack));

    internal ModeDamageFacts CaptureModeDamageFacts(GameEngine.IDamageAttempt attack) => new(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, attack.CardUserSeat, attack.EffectiveCardKind, _coreNativeCardClassificationsOperations.GetDamageNature(attack), attack.IsChainPropagation, attack.IsSourceLess);
}
