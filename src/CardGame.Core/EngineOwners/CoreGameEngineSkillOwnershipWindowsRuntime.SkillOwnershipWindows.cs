#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSkillOwnershipWindowsRuntime
{
    internal bool TryBeginAdvancedSkillsChanged(long parentFrameId) => _skillOwnershipWindowOperations.Get().Begin(parentFrameId);
}
