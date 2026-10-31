#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreNativeCardClassificationsCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardClassificationsCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNatureBeforeMobileMaChao(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNatureBeforeMobileMaChao(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime))]
internal sealed class CoreNativeCardClassificationsCharacterMaChaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardClassificationsCharacterMaChaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature MobileMaChaoYuliNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.DamageNature priorNature) => this._targetFactory().MobileMaChaoYuliNature(attack, priorNature);
}
