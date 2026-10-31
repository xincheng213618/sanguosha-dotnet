#nullable enable

namespace CardGame.Core.NativeOwners;

internal abstract class ProgramTriggerWindowHost()
    {
        internal abstract bool TryReadPending(out ProgramTriggerWindowEntry entry);
        internal abstract bool TrySkip(ProgramTriggerCandidate candidate);
        internal abstract bool TryResolveGroup(ProgramTriggerCandidate first, ProgramSkillWindowContext context,
            PromptChoice selected, string? action);
    }
