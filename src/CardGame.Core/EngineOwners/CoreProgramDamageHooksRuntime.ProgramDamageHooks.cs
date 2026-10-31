#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageHooksRuntime
{
    internal static Type DamageHookDelegateType(ProgramDamagePhase phase) => phase switch
    {
        ProgramDamagePhase.FactModifiers => typeof(Func<global::CardGame.Core.GameEngine.IDamageAttempt, IEnumerable<(CardUseEffectSource Source, int Amount)>>),
        ProgramDamagePhase.AdditiveAmount => typeof(Func<global::CardGame.Core.GameEngine.IDamageAttempt, int>),
        ProgramDamagePhase.BeforeProgramWindow or ProgramDamagePhase.AfterEquipmentChoices => typeof(Func<global::CardGame.Core.GameEngine.IDamageAttempt, int, DamageNature, ProgramDamageInterception>),
        ProgramDamagePhase.AppliedBeforeDying => typeof(Func<global::CardGame.Core.GameEngine.IDamageAttempt, long, bool>),
        ProgramDamagePhase.AttackCompletionPrelude or ProgramDamagePhase.IssuedAttackCompletion => typeof(ProgramDamageCompletion<global::CardGame.Core.NativeOwners.CardAttackHandle>),
        ProgramDamagePhase.BeforeChainAdvance or ProgramDamagePhase.AfterChainAdvance => typeof(Action<global::CardGame.Core.GameEngine.IDamageAttempt>),
        _ => throw new ArgumentOutOfRangeException("phase")};
    internal static ProgramDamageHookCatalog NativeDamageHookCatalog => global::CardGame.Core.EngineOwners.CoreProgramDamageHooksDefinitions.DamageHookMetadata.Value;
    internal ProgramDamageHooks DamageHooks => _damageHooksOperations._damageHooks ??= new(global::CardGame.Core.EngineOwners.CoreProgramDamageHooksDefinitions.DamageHookMetadata.Value, row => _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(row.Method, row.DelegateType), _modeRuntimeOperations._modeRuntime.DamageRules, attack => CaptureModeDamageFacts((global::CardGame.Core.GameEngine.IDamageAttempt)attack));

    internal ModeDamageFacts CaptureModeDamageFacts(global::CardGame.Core.GameEngine.IDamageAttempt attack) => new(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, attack.CardUserSeat, attack.EffectiveCardKind, _coreNativeCardClassificationsOperations.GetDamageNature(attack), attack.IsChainPropagation, attack.IsSourceLess);
}
