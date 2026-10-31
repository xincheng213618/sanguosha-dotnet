#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCardClassificationsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNatureBeforeMobileMaChao(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeCardClassificationsCharacterMaChaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature MobileMaChaoYuliNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.DamageNature priorNature);
}
