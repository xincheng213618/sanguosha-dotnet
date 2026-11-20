# Card-use history and resolution

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics.

## Actual-use and phase history

- Actual card-use history is derived from shared acceptance, native declaration, target-confirmation, finalization, role-update and completion facts through `IActualCardUseHistoryQueries`. Compose rule predicates and target sets with shared fact expressions; do not add per-character history predicates, collectors or proof ledgers. Preserve accepted versus confirmed/finalized/completed order, accepted declarer versus current actor, provider/responder identity, and the turn scope at each real sample. Keep latest targets, established-ever relations and window cutoffs distinct; real native uses without an Action must not acquire a fake ActionId. Freeze the history point and accepted qualification on the original frame for continuations instead of refreshing them from later nested history. Sequence rules compose accepted and immutable first-finalization views at the real window cutoff, without another pair ledger or a refreshed completion point.

- Actual-use occurrence reads preserve the original native declarer and declaration order, including their real response identity. Choose a completed-turn boundary explicitly when required; candidate-window and current-time cutoffs remain separate. Do not count accepted-only attempts as declared uses or silently widen self-supplied responses to other response origins.

- Card-use confirmation expressions read each real confirmation separately: the confirmation view retains the raw event targets, while latest/observations retain designated targets. Confirmed targets, normalized primary targets and sampled phase survive ordinary role/target updates as last-confirmation metadata; confirmation does not imply finalized/Established. Match declared kinds through the shared card-kind matcher, preserve unknown reads and real identity comparisons, and choose live qualification versus a frozen boundary cutoff explicitly.

- Actual phase history uses the original phase-start facts and shared actual-skip facts through `IActualPhaseHistoryQueries` and `phaseCount`. Restoring a phase is not a new entry; skip requests, substitutions and skill payments are not actual skips. Preserve natural, inserted and extra-draw occurrences, actor/turn scope and journal cutoffs; keep distinct cost facts where a rule counts payment rather than outcomes.

## Native execution and typed returns

- Follow a real card-use resolution chain to identify reusable facts, conditions, trigger windows and effects. Keep actual use/response facts separate from quota accounting, and keep nested resolution on owning frames; a Slash example is a path through common rules, not a separate family of skill implementations.

- Native zero-entity attacks with the standard completion protocol declare that role on their original `CardUseFrame` at issuance; `CoreNativeSlashCompletionRuntime` owns their completion window and existing typed return. Do not add per-character completion hooks for that same protocol. Keep group-card iteration, material retirement and rule-specific post-use progression in their actual owners.

- A `RequiresVirtualSlashAction` callback can request an original use action for the virtual Slash actor before its frame is issued. Mobile Sidi registers the live prediction check here and observes the first actual non-delayed use at the shared committed-use window. Full native windows independently determine alcohol and armor handling.

- Native completions with distinct semantics still include Boundary Liu Feng's next-target progression, God Xun Yu's Qizheng, and producers that return an exact ending entity or a fixed kind. Preserve these outcomes when migrating common completion. Delayed placement, producer-specific qualification and paid child returns have their own owners.

- Native card-use returns preserve the parent program's chosen inputs. Declare named use results on the original instruction and read their completed execution targets explicitly; do not rewrite parent selections or reconstruct primary returns through character adapters. Keep declaration, designated and executed targets distinct, and preserve real typed producer returns.

- Native trick continuations use `INativeTrickContinuations` over the original window's parent use, ActionId, stage and typed return. Do not rebuild character Matches/proof chains or add another identity ticket. Committed windows keep their earlier lifecycle position; legitimate actor/target changes read the original use's current state, while material residency, delayed placement and producer-specific returns retain their actual owners.

- Native execution targets are ordered occurrences on their owning frame and may repeat a seat. Mutate the actual cursor slot at one shared boundary; keep declaration/designated targets distinct from live execution targets. Shared continuations consume their original typed return and cursor facts, not character target-tail whitelists or private redirect ledgers. Completion trusts accepted native mutations instead of replaying character proof histories; public notifications are not internal execution proofs.

## Effect episodes and damage adjustments

### Native effect episodes

- `INativeCardEffectEpisodes` schedules repeated effects on the original `CardUseFrame.EffectEpisodes`. The use holds one native `Trick` or `Simple` continuation, attack seeds indexed by actual execution slot, the Fangtian target progression and the group-damage source. Each producer issues its own episode with source, instruction and issuance slots, priority, copy count, scope and continuation kind. The shared scheduler orders by priority and actual issuance, using the accepted frame data without character dispatch or event proofs.
- Declare whole-use, frozen-target, live target-occurrence and paired-target scopes explicitly. A target-local copy ends its cursor and clears the group continuation at that slot. Whole-use Slash restores each slot from its own captured seed, including repeated seats, and requires a real seed for any later target. Accumulated damage, material and typed returns stay with the original use.
- `HasPending` reports unstarted, uncancelled copies while the match is ongoing and the use remains effective. `IsActive` reports whether a copy still owns native continuation, including the last copy's material-retirement timing and movement reason. An empty target set or a delayed entity that cannot be placed again can consume a copy and advance the queue. `IsCompleted` supports completed returns and `IsSettled` includes cancellation. Actual damage, recovery and movement results retain their own facts.
- An issued episode survives the producer's later death or skill loss. Read `ContinueAfterActorDeath` for the active copy. When there is no active copy, use `ContinueOriginalAfterActorDeath` for the original effect only if an actual producer issued that permission. It defaults to off.
- After the current native child returns, [whole-use invalidation](RESPONSES_AND_VISIBILITY.md#card-action-invalidation) releases the active cursor and settles the remaining queue. Unstarted copies are cancelled without starts or ordinal increments. Target immunity and an individual counterspell affect their own scope.

| Producer | Rule-specific boundary |
| --- | --- |
| Same-kind payment, automatic trick repeats, Guandu and mode repeats | Issue whole-use effects. Payment and mode eligibility stay with the producer. Guandu requires current native target legality before starting its copy. |
| 费祎合众 | Bind the original turn grant and rank match to the actual use before issuing a whole-use episode. A returned grant frame can supply its original source identity. |
| 乐大乔姊希 | Issue the exact live execution slot after selection and judgment. Borrowed Sword retains its execution pair. Repeated seats are resolved by slot. |
| 魔孙权权御、谋黄月英秉才 | Issue paid whole-use effects. 秉才's zero-entity self-rescue Alcohol repeats recovery using the original typed producer and Dying return without recording another use or rescue. |
| Incoming single-target repeat with range-based draw | After all episodes settle, the reward owner evaluates its own survival, source effectiveness and current attack range. Episode permission does not grant reward eligibility. |
| 谋黄权巧对、谋蒋济应时、school repeats | 巧对 keeps its gift payment and whole-use invalidation separate, and consumes a repeat of a delayed entity without placing it again. 应时 keeps its frozen targets and Borrowed Sword pair. School repeats require the actual damage-trick qualification. |

`issueCurrentCardEffectEpisode` declares scope, copies and continuation policy in
the original `CardUseCommitted` or `CardUseTargetsFinalized` window, with an
explicit execution slot where required. Native producers use the same issuance
boundary. Later first-issuance windows are outside the current contract.
Remaining composition gaps include native qualification, choice and judgment,
payment, matching future grants, post-completion rewards and special native
issuance. Those rules and their receipts stay with their producers.

### Damage adjustments

- The native damage application records the target's HP on the original `DamageFrame` immediately after its armor and HP changes. A dying observer such as Shajue reads this value through the original dying parent. Its card causes and non-damage dying eligibility retain their own rules.

- Shanhe Sanshou armor admits its damage-prevention candidate through the effective field equipment and its original equipment skill instance. Armor suppression and bypass qualify against the actual damage attempt. The issued skill finishes its reveal and cleanup on the original frame.

- Huxin Mirror uses effective field equipment and its exact skill instance for the offer and the unpaid admission. Once the mirror actually leaves equipment as payment, the original damage owner reads that movement receipt to finish prevention through any movement children.

- Kunlun Mirror uses effective field equipment and its exact treasure skill instance for both learning and prevention. The holder learns the effective trick name after actual applied damage. Later prevention reads that holder's remembered name and current equipment on the actual damage attempt.

- Fixed effects already issued for a card use use `CardUseFrame.CardDamageAdjustments` and `CoreGameEngineCardUseLifecycleRuntime`; current-occurrence damage changes stay on the actual damage attempt. Preserve whole-use versus target scope and card-damage versus additional-damage layers. Do not rebuild character boost ledgers or requalify paid effects when their source disappears.

- Barbarian Assault keeps its original card user and assigned damage source on the group continuation. A living assigned source remains responsible after skill loss. If that replacement source dies, a later failed response starts a source-less damage attempt while the original use continues with its assigned seats.

## Recovery applications

- `CoreNativeRecoveryFramesRuntime.ApplyRecovery` applies capped HP recovery from the current original `RecoveryFrame`. Ordinary Peach and the program `Recover` step use the returned before and after HP. Request amounts, replacement admission, dual-health selection, and the typed continuation stay with their native or program producers.

- Program recovery requests freeze `ProgramRecoveryOrigin` on the original queued `RecoveryAttempt`; actual HP applications use the original program's `RecoveryApplications`. `HealthContinuationRuntime` owns their shared receipt reads and typed replacement returns. Preserve issued source, amount, target, dual-health and native return boundaries; use the accepted candidate and original reward movement receipt instead of copying match history to prove output events. Requested, Applied and Chosen events retain their real history consumers; native rescue and dying-entry protocols keep their own facts.

- Native card recovery reads the current `CardUseFrame.RecoveryApplication` through `NativeHealthRecoveryRuntime`. Capture from the real top `RecoveryFrame` before notifications; the same application is idempotent and conflicting writes fail, while a later real recovery on the same use may replace it for another target or effect. Share the HP/material return matcher and keep original Dying, responder, Peach/self-Alcohol and native physical-group boundaries separate. Issued recovery-use links consume their original typed producer relation; do not copy event history to re-prove Applied or Issued output notifications. Zero-delta applications may retain a receipt and notification without authorizing a positive HP child.

- The ordinary rescue Peach and the issued zero-entity Peach from the four-suit discard use the same native rescue calculation. The bonus uses the effective faction of the actual Peach user and the original dying target. The zero-entity use freezes rescue-only policies on its simple-card continuation at issuance and still evaluates broader native Peach policies at their existing recovery-effect boundary.
