#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNationalSkillsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKemingBorrowedSkillInstance(global::CardGame.Core.CharacterState owner, string skill, string instance);
    string? KemingBorrowedSkillInstance(global::CardGame.Core.CharacterState owner, string skill);
    bool HasKemingBorrowedSkill(global::CardGame.Core.CharacterState owner, string skill);
}

internal interface ICoreNationalSkillsCorePrintedLordSkillQualificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    string? PreferredQualifiedPrintedLordInstance(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreNationalSkillsCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICoreNationalSkillsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNationalSkillsSkillBindingIndexOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchSkillBindingIndex _skillBindingIndex { get; }
}

internal interface ICoreNationalSkillsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDualGeneralMode { get; }
}
