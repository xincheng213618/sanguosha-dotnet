#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramDamageCardClaimNativeRulesRuntime : global::CardGame.Core.IProgramDamageCardClaimNativeRules
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries _coreExtraEffectMaterialClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    internal CoreProgramDamageCardClaimNativeRulesRuntime(global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries coreExtraEffectMaterialClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations)
    {
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreExtraEffectMaterialClaimsOperations = coreExtraEffectMaterialClaimsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
    }
}
