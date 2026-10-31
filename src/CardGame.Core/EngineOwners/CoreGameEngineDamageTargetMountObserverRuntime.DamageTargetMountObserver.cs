#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetMountObserverRuntime
{

    internal static bool MountObserverCandidateMatches(ProgramSkillFrame p, ProgramTriggerCandidate c) => RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(p, c);
}
