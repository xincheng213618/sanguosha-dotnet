# 界吕布、界袁绍规则与工程裁定

采用当前普通OL的两名武将，详见[来源](../sources/fenglin-fifteenth-2026-10-03.json)。

2500 `ObtainDamageTargetCardAndResolveCategory` 在真实本人Use杀对其他最终角色造成正伤害后，从HEJ获得一张。实际移动回调先冻结实得Hand凭据及打印类别；非装备令原受害者摸一张，装备由受害者选择合法另一角色，实际本人发行无色零实体Duel。owning frame与独立Duel origin持有付款和父Use关系，嵌套true/false返回保留外层实体杀。GeneralWeapon规范化至OutsideGame只留真实移动token，不伪造Hand收益；精确observer能有限返回。

2600 `GrantFactionPopulationMarker` 在真实GameStarting窗口建立时冻结逐角色effectiveFaction人数，成熟主公资格下一次授予两倍群人数的裔。新`ownerMarkerCount`只通过MarkerState依赖注入实时aggregate查询；使用新节点的catalog才纳入Role资格stamp，旧默认保持。真实普通/额外Play开始付一枚裔摸一张。

2601 `RemoveSelectedCurrentArrowBarrageTarget` 对所有本人实际Use万箭的finalized窗口提供可选移除，CardUse上的scalar凭据限制整次Use一次。同步真实余集、window候选FrozenContext和live program context，保留初始历史事实；旧CurrentCardUseTargets查询本体不变。新事件只有scalar，owning集合保持只读。

新规则事实通过AdvanceEventRulesAndQueueFact、新状态通过AdvanceRulesAndPublishState；四视角使用CreateSnapshot。源/Role/actor等HOST审计与正式命令/cold检查分开标识。

## 工程裁定

官方正文：[界吕布319](https://www.sanguosha.com/hero/319)、[界袁绍450](https://www.sanguosha.com/hero/450)；完整技能文本、HTTP时间和原立绘SHA见[来源](../sources/fenglin-fifteenth-2026-10-03.json)。API没有结构化gender，Male沿既有人物惯例。以下寿命与排序为工程默认，不能称官方FAQ。

利驭只计真实本人Use杀对其他最终角色的正实际伤害，同Use转换、链传/转移沿来源资格；自己为受害者不触发。获得按实际进Hand的打印类别分支，手中的打印装备也为装备；Obtain不经过他人Discard专用保护。获得后children移走卡牌不重新获得或改变分支。实得前失效沿成熟取消；已实得的尾段不因来源消失重付。非装备收益在本人随后死亡、受害者仍活且游戏Running时继续；受害者死亡/游戏结束不新摸牌。装备分支本人/受害者死亡或无合法目标有限结束；受害者真实选择恰一合法另一角色，真实Duel保留无双、无懈、完成与精确外层杀返回。

血裔群人数包括本人符合群势力的一席，冻结点在选将/神将势力选择后、所有GameStarting子事件前。成熟主公资格和owner+skill初始化一次，中途得技不补发；手牌上限读取启用且有资格者的实际剩余裔，失去技能不删除标记事实。每个真实普通/额外Play开始可付一裔摸一张，沿旧来源取消余指令/付款不退，不新增每回合限次。

乱击两Hand打印同花色转换沿成熟路径。finalized排除适用于实体、乱击或其他转换的所有本人真实万箭Use；Decline或移除一个实际余集目标，整次Use一次。删至零仍保留已付成本、集智/无懈/完成，无角色响应或伤害，不退款或重发。旧发行前禁止门及实体/转换ExcludeGlobalTarget builder差异保留；后置移除不能使原本非法发行合法。
