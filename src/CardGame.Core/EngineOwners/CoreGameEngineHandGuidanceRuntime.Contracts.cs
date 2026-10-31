#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHandGuidanceCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreHandGuidanceCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreHandGuidanceCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashAllowanceForAnyTarget(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardKind>? kinds = default);
}

internal interface ICoreHandGuidanceCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreHandGuidanceCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.WushuangJinzhiMaterialSelection? HumanWushuangJinzhiMaterials();
}

internal interface ICoreHandGuidanceCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind baseEffectiveKind);
}

internal interface ICoreHandGuidanceCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> GetHumanLegalActions();
}

internal interface ICoreHandGuidanceCoreCommandSessionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long Revision { get; }
}

internal interface ICoreHandGuidanceCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreHandGuidanceCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreHandGuidanceCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreHandGuidanceCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreHandGuidanceCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string EnabledSkillNames(global::CardGame.Core.CharacterState player);
}
