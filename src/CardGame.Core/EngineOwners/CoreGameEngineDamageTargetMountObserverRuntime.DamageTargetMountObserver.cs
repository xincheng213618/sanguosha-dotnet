#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetMountObserverRuntime
{

    internal static bool MountObserverCandidateMatches(ProgramSkillFrame p, ProgramTriggerCandidate c) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(p, c);
}
