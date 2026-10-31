#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreNationalSkillsCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNationalSkillsCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasKemingBorrowedSkillInstance(global::CardGame.Core.CharacterState owner, string skill, string instance) => this._targetFactory().HasKemingBorrowedSkillInstance(owner, skill, instance);
    public string? KemingBorrowedSkillInstance(global::CardGame.Core.CharacterState owner, string skill) => this._targetFactory().KemingBorrowedSkillInstance(owner, skill);
    public bool HasKemingBorrowedSkill(global::CardGame.Core.CharacterState owner, string skill) => this._targetFactory().HasKemingBorrowedSkill(owner, skill);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime))]
internal sealed class CoreNationalSkillsCorePrintedLordSkillQualificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNationalSkillsCorePrintedLordSkillQualificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> _targetFactory = targetFactory;
    public bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsPrintedLordGrantQualified(owner, grant);
    public string? PreferredQualifiedPrintedLordInstance(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().PreferredQualifiedPrintedLordInstance(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime))]
internal sealed class CoreNationalSkillsCoreCurrentTurnSkillSuppressionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNationalSkillsCoreCurrentTurnSkillSuppressionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnSkillSuppressionsRuntime> _targetFactory = targetFactory;
    public bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => this._targetFactory().IsCurrentTurnSkillGrantQualified(owner, grant);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNationalSkillsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNationalSkillsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsDualGeneralMode { get => this._targetFactory().IsDualGeneralMode; }
}
