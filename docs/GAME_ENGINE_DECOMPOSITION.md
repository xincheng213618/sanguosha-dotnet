# GameEngine 职责与装配

`GameEngine` 提供公开命令入口、对局装配和核心协调。人物与机制的触发、资格、费用、选择和结算由独立职责类执行，不依靠 `partial GameEngine` 共享私有算法。

## 职责落点

| 位置 | 职责 |
| --- | --- |
| `Characters/<人物>/<版本>/Runtime/` | 人物规则、专属查询及操作契约；源码文件按业务命名 |
| `CardGame.Core/EngineOwners/`、`Runtime/Rules/` | 独立的共享规则、查询、投影及结算机制 |
| `CardGame.Core/NativeOwners/`、人物的 `Runtime/NativeOwners/` | 原生宿主与执行器；依赖类型化状态、查询及延迟入口，不保存完整引擎 |
| `CardGame.Core/EngineStates/`、`Runtime/StateContracts/` | 对局状态的唯一所有者及按职责提供的窄状态契约 |
| `CardGame.Core/RuntimeEntries/` | 每个目标模块共用的延迟操作入口，不按消费方复制契约和适配器 |
| `CardGame.Core/Runtime/Definitions/` | 不可变定义、注册元数据与未绑定工厂 |
| `Session/`、`Resolution/`、`Runtime/Composition/` | 命令事务、帧生命周期及本局依赖装配 |

同一职责可以拆成同一个独立类的多个源文件。核心保留的嵌套枚举、DTO、收据和结构值用于数据表达及既有 CLR 身份，不承担人物算法，也不是运行宿主。

## 按局装配与身份

[MatchRuntime](../src/CardGame.Core/Runtime/MatchRuntime.cs) 拥有本局模块、能力及处理器绑定。全局目录保存不可变定义和未绑定元数据；模块实例、实际状态及已绑定委托属于各自对局，不进入静态缓存或 Checkpoint。

模块通过 `[ProgramRuntimeModule]` 显式注册。构造函数接收共用的查询、操作接口或目标模块入口并保存依赖，按第一次实际使用创建实例。授技、替换、升级及变身可能引入新的能力，因此目录不按开局上场名单裁掉处理器。

内部模块的纯依赖构造使用 C# 12 主构造函数；直接使用捕获的依赖参数，不再重复声明只读字段和逐项赋值。依赖解析仍按参数类型及顺序进行，跨 partial 文件使用同一份依赖。具有额外初始化行为的构造保留原流程。

[模块目录](../src/CardGame.Core/ProgramRuntimeModules.cs) 拒绝嵌在 `GameEngine` 中的运行模块、非恒定静态状态和综合执行宿主。模块字段、非构造方法及注入接口的槽位都不能暴露 `GameEngine`；检查覆盖引用、数组、泛型和委托签名，不能把完整引擎藏进 `Func<GameEngine, ...>`。

共享入口只保存唯一目标的解析函数。只涉及直接读取和简单参数的调用使用 `DeferredRuntime<T>.Value` 访问实际模块，省去逐方法转发；涉及参数副作用、方法组或复杂赋值求值顺序的调用保留 `[ProgramRuntimeEntry(typeof(TargetRuntime))]` 入口。`MatchRuntime` 缓存本局入口，核心装配和各消费方取得同一对象；实际访问时才创建目标模块，避免构造期间相互解析。相同目标不再为每个消费方生成一套 `Operations` 接口和转发类，规则也不获得通用模块查询器。

核心装配适配器可以持有引擎，负责定位本局状态、绑定原语与处理器；规则只获得其窄接口。具名的延迟入口属性返回同一个缓存代理，不提前创建目标模块。调用参数、赋值右侧、空值短路、`ref/out/in` 和引用返回保持各自的求值位置；重复绑定保留同局入口身份，跨局不共享实例。首次构造失败不会缓存为成功，实际构造循环会被明确拒绝。

能力和处理器属性属于隐式注册根，不能因没有显式类型引用就删除。规则注册留在实际实现端，纯装配适配器不复制规则。

## 唯一状态与执行边界

[CommandSession](../src/CardGame.Core/Session/CommandSession.cs) 拥有命令事务、Revision、可信命令前缀和故障恢复。`CardZoneStore` 拥有实体牌位置；`ResolutionScheduler` 的 `FrameStore` 拥有结算栈；角色、随机源、历史和移牌批次继续使用本局原对象。

独立状态所有者保存原字段，不维护同步副本。状态契约仅在实际所有者、槽位、类型、可空性、引用修饰、属性及底层字段全部相同时合并；不同所有者不能拼成一个状态包。原只读字段通过只读引用公开，构造期初始化保持一次性约束，不能由后续规则重新赋值。

候选、支付、暂停、游标和子结算凭据保留在所属帧。子帧通过原 push、replace、complete 协议返回原父帧，恢复后不重复支付已完成的代价。运行模块不另外保存 pending 副本，也不代替帧承担存档状态。

规则事实经 `AdvanceEventRulesAndQueueFact` 进入观察链，状态推进经 `AdvanceRulesAndPublishState` 打开相应窗口。[PendingGameEventOutput](../src/CardGame.Core/Session/PendingGameEventOutput.cs) 只完成输出投影、原日志追加及历史捕获，不作为规则入口。

玩家数据通过 `CreateSnapshot(viewerSeat)` 投影。嵌套集合在提交准备阶段冻结，随后才提交历史和派发观察者。`ResolutionStack` 等完整诊断仍属于可信宿主材料，不能直接作为玩家视图。

恢复重建本局对象，再按既有命令、内容指纹和父帧协议推进。职责拆分不增加全局规则版本，也不改变命令身份、Checkpoint 格式或技能 JSON schema。

## 维护与验证

新增人物优先组合现有内容操作：摸牌 `Draw`、掉血 `LoseHp`、伤害 `Damage`、翻面 `TurnOver`、盖牌 `SetFaceState`、指定目标 `SelectTarget` / `SelectTargets`、加伤 `GrantTurnCardDamageModifier`。规则专属资格、费用及阶段条件留在所属规则类；只有现有操作无法表达的效果才增加机制实现，不为普通效果复制人物专属执行、证明和事件体系。

测试中的私有反射由 `tests/Shared/FixtureRuntimeAccess.cs` 路由到实际状态或本局入口。它只保存元数据，不缓存对局对象；逻辑成员名与当前实际类型绑定分别核对，不恢复旧引擎嵌套宿主作为反射占位。

开发使用现有 `--filter=<名称>` 检查相关主流程，覆盖原支付子帧、原生返回、提交、隐私、牌移动及冷恢复。集成后使用 `tools/Test-Changed.ps1` 的常规范围；`-Full` 或无过滤的完整 runner 仅在用户明确要求时运行。构建、依赖验证和测试过程耗时分别记录，不以文件数或类数推断性能改善。

相关约定：[整体架构](ARCHITECTURE.md)、[人物模块](CHARACTER_MODULES.md)、[结算协议](RESOLUTION_PROTOCOLS.md)、[开发门禁](DEVELOPMENT_GATES.md)。
