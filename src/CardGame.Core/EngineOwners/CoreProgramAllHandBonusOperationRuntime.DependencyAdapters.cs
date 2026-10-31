#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCoreBoundDiscardSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCoreBoundDiscardSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasTurnSlashSuitAllowance(int owner, global::CardGame.Core.Suit? suit) => this._targetFactory().HasTurnSlashSuitAllowance(owner, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCoreHandComparisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCoreHandComparisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandComparisonRuntime> _targetFactory = targetFactory;
    public bool HasSelfCardTargetProhibition(int seat) => this._targetFactory().HasSelfCardTargetProhibition(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCharacterSimaYiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCharacterSimaYiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> _targetFactory = targetFactory;
    public bool AllowsHappySimaPeach(int actor) => this._targetFactory().AllowsHappySimaPeach(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public bool HasPhaseSuitAllowance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasPhaseSuitAllowance(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouMobileRuntime))]
internal sealed class CoreProgramAllHandBonusOperationCharacterLuMengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAllHandBonusOperationCharacterLuMengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouLuMengPeachRescueAllowed(int actor) => this._targetFactory().IsMouLuMengPeachRescueAllowed(actor);
}
