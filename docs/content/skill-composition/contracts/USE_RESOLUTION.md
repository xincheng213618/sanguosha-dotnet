# Card-use history and resolution

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics and the
[capability map](../CAPABILITY_MAP.md) for completion status and remaining gaps.

## Actual-use and phase history

- Actual card-use history is derived from shared acceptance, native declaration, target-confirmation, finalization, role-update and completion facts through `IActualCardUseHistoryQueries`. Compose rule predicates and target sets with shared fact expressions; do not add per-character history predicates, collectors or proof ledgers. Preserve accepted versus confirmed/finalized/completed order, accepted declarer versus current actor, provider/responder identity, and the turn scope at each real sample. Keep latest targets, established-ever relations and window cutoffs distinct; real native uses without an Action must not acquire a fake ActionId. Freeze the history point and accepted qualification on the original frame for continuations instead of refreshing them from later nested history. Sequence rules compose accepted and immutable first-finalization views at the real window cutoff, without another pair ledger or a refreshed completion point.

- Actual-use occurrence reads preserve the original native declarer and declaration order, including their real response identity. Choose a completed-turn boundary explicitly when required; candidate-window and current-time cutoffs remain separate. Do not count accepted-only attempts as declared uses or silently widen self-supplied responses to other response origins.

- Card-use confirmation expressions read each real confirmation separately: the confirmation view retains the raw event targets, while latest/observations retain designated targets. Confirmed targets, normalized primary targets and sampled phase survive ordinary role/target updates as last-confirmation metadata; confirmation does not imply finalized/Established. Match declared kinds through the shared card-kind matcher, preserve unknown reads and real identity comparisons, and choose live qualification versus a frozen boundary cutoff explicitly.

- Actual phase history uses the original phase-start facts and shared actual-skip facts through `IActualPhaseHistoryQueries` and `phaseCount`. Restoring a phase is not a new entry; skip requests, substitutions and skill payments are not actual skips. Preserve natural, inserted and extra-draw occurrences, actor/turn scope and journal cutoffs; keep distinct cost facts where a rule counts payment rather than outcomes.

## Native execution and typed returns

- Follow a real card-use resolution chain to identify reusable facts, conditions, trigger windows and effects. Keep actual use/response facts separate from quota accounting, and keep nested resolution on owning frames; a Slash example is a path through common rules, not a separate family of skill implementations.

- Native zero-entity attacks with the standard completion protocol declare that role on their original `CardUseFrame` at issuance; `CoreNativeSlashCompletionRuntime` owns their completion window and existing typed return. Do not add per-character completion hooks for that same protocol. Keep group-card iteration, material retirement and rule-specific post-use progression in their actual owners.

- Native card-use returns preserve the parent program's chosen inputs. Declare named use results on the original instruction and read their completed execution targets explicitly; do not rewrite parent selections or reconstruct primary returns through character adapters. Keep declaration, designated and executed targets distinct, and preserve real typed producer returns.

- Native trick continuations use `INativeTrickContinuations` over the original window's parent use, ActionId, stage and typed return. Do not rebuild character Matches/proof chains or add another identity ticket. Committed windows keep their earlier lifecycle position; legitimate actor/target changes read the original use's current state, while material residency, delayed placement and producer-specific returns retain their actual owners.

- Native execution targets are ordered occurrences on their owning frame and may repeat a seat. Mutate the actual cursor slot at one shared boundary; keep declaration/designated targets distinct from live execution targets. Shared continuations consume their original typed return and cursor facts, not character target-tail whitelists or private redirect ledgers. Completion trusts accepted native mutations instead of replaying character proof histories; public notifications are not internal execution proofs.

## Effect episodes and damage adjustments

- Repeated effects of an accepted native card use use `INativeCardEffectEpisodes` and the original `CardUseFrame.EffectEpisodes`: one original continuation, attack seed set and group damage source, with declared priority, copies and whole-use or exact target-occurrence scope. Producers retain qualification, costs and later business effects; shared scheduling never dispatches by character or re-proves issuance from events. `HasPending` means future unstarted copies, while `IsActive` owns the current copy. Keep original-use death continuation permission separate from an extra copy's permission. Whole-use cancellation settles remaining copies without inventing starts or ordinals; target immunity and an individual counterspell do not cancel the whole queue. Restore each accepted Slash slot from its own seed, preserving repeated seats, original materials and typed returns; do not rebuild later targets from the first target's damage or restart a whole target walker for a target-local episode.

- Fixed effects already issued for a card use use `CardUseFrame.CardDamageAdjustments` and `CoreGameEngineCardUseLifecycleRuntime`; current-occurrence damage changes stay on the actual damage attempt. Preserve whole-use versus target scope and card-damage versus additional-damage layers. Do not rebuild character boost ledgers or requalify paid effects when their source disappears.

## Recovery applications

- Program recovery requests freeze `ProgramRecoveryOrigin` on the original queued `RecoveryAttempt`; actual HP applications use the original program's `RecoveryApplications`. `HealthContinuationRuntime` owns their shared receipt reads and typed replacement returns. Preserve issued source, amount, target, dual-health and native return boundaries; use the accepted candidate and original reward movement receipt instead of copying match history to prove output events. Requested, Applied and Chosen events retain their real history consumers; native rescue and dying-entry protocols keep their own facts.

- Native card recovery reads the current `CardUseFrame.RecoveryApplication` through `NativeHealthRecoveryRuntime`. Capture from the real top `RecoveryFrame` before notifications; the same application is idempotent and conflicting writes fail, while a later real recovery on the same use may replace it for another target or effect. Share the HP/material return matcher and keep original Dying, responder, Peach/self-Alcohol and native physical-group boundaries separate. Issued recovery-use links consume their original typed producer relation; do not copy event history to re-prove Applied or Issued output notifications. Zero-delta applications may retain a receipt and notification without authorizing a positive HP child.
